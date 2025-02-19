using UnityEngine;
using StateMachine;
namespace CharacterLogic
{
    [System.Serializable]
    public abstract class APlayerModifier : IModifier<Player_State, Player_Status>
    {


        public abstract string Name { get; }

        protected StateMachine<Player_State, Player_Status> stateMachine;

        public virtual void OnEntry(StateMachine<Player_State, Player_Status> stateMachine)
        {
            this.stateMachine = stateMachine;
        }
        public virtual void OnExit() 
        { 

        }
        public virtual void OnFixedUpdate()
        {

        }
        public virtual void OnLateUpdate()
        {

        }
        public virtual void OnModifierStack(IModifier<Player_State, Player_Status> newModifier, StateMachine<Player_State, Player_Status> stateMachine)
        {

        }
        public virtual void OnSecondUpdate(ref float delta)
        {

        }
        public virtual void OnUpdate(ref float delta)
        {

        }
    }

}
