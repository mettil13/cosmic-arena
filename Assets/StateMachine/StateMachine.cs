using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    [Serializable]
    public class StateMachine <StateEnum, StatusEnum> 
        where StateEnum : System.Enum 
        where StatusEnum : System.Enum
    {

        #region State Variables

        [OdinSerialize] public Dictionary<StateEnum, IState> states;
        [SerializeField] StateEnum startingState;
        [SerializeField] private StateEnum currentStateEnum;
        private IState currentState;

        [SerializeField] private StatusEnum status;
        public Dictionary<string, StatusEnum> statusModifiers;
        List<string> statusModifiersBinQueue = new List<string>();

        #endregion

        #region Modifier Variables

        [OdinSerialize] public Dictionary<string, IModifier<StateEnum, StatusEnum>> modifiers = new Dictionary<string, IModifier<StateEnum, StatusEnum>>();
        List<string> modifiersBinQueue = new List<string>();

        #endregion

        #region Common Metods
        public IState CurrentState => currentState;
        public StateEnum CurrentStateEnum => currentStateEnum;
        public StatusEnum Status => status;
        public StateMachine()
        {
            this.currentState = null;
            states = new Dictionary<StateEnum, IState>();
        }

        public StateMachine(Dictionary<StateEnum, IState> states, IState currentState)
        {
            this.states = new Dictionary<StateEnum, IState>(states);
            this.currentState = currentState;
        }

        public void SetProperites(Dictionary<StateEnum, IState> states, StateEnum currentState, StatusEnum status)
        {
            this.states = new Dictionary<StateEnum, IState>(states);
            ChangeState(currentState);
            this.status = status;
        }

        public void Init()
        {
            ChangeState(startingState);
            modifiersBinQueue = new List<string>();
            statusModifiersBinQueue = new List<string>();
        }
        public void Execute(float delta)
        {
            StateExecute(ref delta);
            ModifierExecute(ref delta);
        }
        
        public void LateExecute()
        {
            StateLateExecute();
            ModifierLateExecute();
        }

        public void FixedExecute()
        {
            StateFixedExecute();
            ModifierFixedExecute();
        }


        #endregion

        #region State Metods

        public void StateExecute(ref float delta)
        {
            if (currentState == null) return;
            currentState.OnUpdate(ref delta);
            currentState.OnSecondUpdate(ref delta);

        }
        public void StateLateExecute()
        {
            if (currentState == null) return;
            currentState.OnLateUpdate();
        }

        public void StateFixedExecute()
        {
            if (currentState == null) return;
            currentState.OnFixedUpdate();
        }
        public void ChangeState(StateEnum newState)
        {
            if (states.TryGetValue(newState, out IState state))
            {
                if (CanChangeStateByStatus(newState))
                {
                    ApplyChangeState(state, newState);                    
                }
            }
            else 
            {
                throw new NotImplementedException("State not implemented or not found, newState = " + Convert.ToString(newState));
            }
        }

        bool CanChangeStateByStatus(StateEnum state)
        {
            return (Convert.ToInt32(status) & (1 << Convert.ToInt32(state))) == 1 << Convert.ToInt32(state);
        }




        private void ApplyChangeState(IState state, StateEnum newState)
        {
            if (Convert.ToString(newState) == Convert.ToString(currentStateEnum))
            {
                Debug.Log("same state");
                return;
            }

            if (currentState == state)
                return;

            if (currentState != null)
                currentState.OnExit();
            if (state.CheckChangeTo())
            {
                currentState = state;
                currentStateEnum = newState;
                state.OnEntry();
            }
        }

        //public bool CheckChangeTo(StateEnum stateEnum)
        //{
        //    if (states.TryGetValue(stateEnum, out IState state))
        //        return state.CheckChangeTo();

        //    return false;
        //}

        public void AddStatusModifier(string name, StatusEnum status)
        {
            statusModifiers.Add(name, status);
            UpdateStatus();
        }
        public void RemoveStatusModifier(string name)
        {
            statusModifiersBinQueue.Add(name);

        }

        private void UpdateStatus()
        {
            int tempStatus = ~0;
            foreach (StatusEnum status in statusModifiers.Values)
            {
                tempStatus &= Convert.ToInt32(status);
            }

            status = (StatusEnum)(object)tempStatus;
        }

        

        public bool TryGetState(StateEnum stateEnum, out IState state)
        {
            if (states.TryGetValue(stateEnum, out state))
                return true;
            return false;
        }
        #endregion

        #region Modifier Metods

        public void ModifierExecute(ref float delta)
        {
            foreach(var modifier in modifiers.Values)
            {
                modifier.OnUpdate(ref delta);
                modifier.OnSecondUpdate(ref delta);
            }
            ExecuteBinQueues();
        }

        public void ModifierLateExecute()
        {
            foreach (var modifier in modifiers.Values)
            {
                modifier.OnLateUpdate();
            }
            ExecuteBinQueues();
        }

        private void ModifierFixedExecute()
        {
            foreach (var modifier in modifiers.Values)
            {
                modifier.OnFixedUpdate();
            }
            ExecuteBinQueues();
        }

        public void AddModifier(IModifier<StateEnum, StatusEnum> modifier)
        {
            if (modifiers.TryGetValue(modifier.Name, out var stackedModifier))
            {
                stackedModifier.OnModifierStack(modifier, this);
            }
            else
            {
                modifiers.Add(modifier.Name, modifier);
                modifier.OnEntry(this);
            }
        }

        public void RemoveModifier(string modifierName)
        {
            if (modifiers.TryGetValue(modifierName, out var stackedModifier))
            {
                stackedModifier.OnExit();
                modifiersBinQueue.Add(modifierName);
            }
        }
        void ExecuteBinQueues()
        {
            if (modifiersBinQueue.Count == 0) return;

            string[] modQueue = modifiersBinQueue.ToArray();
            foreach (var mod in modQueue)
            {
                modifiers.Remove(mod);
            }
            modifiersBinQueue = new List<string>();

            string[] statusModQueue = statusModifiersBinQueue.ToArray();
            foreach (var stMod in statusModQueue)
            {
                statusModifiers.Remove(stMod);
                UpdateStatus();
            }
            statusModifiersBinQueue = new List<string>();
        }

        #endregion
    }

}