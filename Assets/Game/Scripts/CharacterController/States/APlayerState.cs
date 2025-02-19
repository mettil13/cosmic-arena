using UnityEngine;
using StateMachine;
using System;
namespace CharacterLogic
{
    public enum Player_State
    {
        Pause = 0,
        Idle = 1,
        Thrusting = 2,
        Braking = 3,
        SpecialAbility = 4,
        MeleeAttack = 5,
    }


    [Flags]
    public enum Player_Status
    {
        None = 0,
        CanAll= -1,
        CanPause = 1 << Player_State.Pause,
        CanIdle = 1 << Player_State.Idle,
        CanThrust = 1 << Player_State.Thrusting,
        CanBrake = 1 << Player_State.Braking,
        CanSpecialAbility = 1 << Player_State.SpecialAbility,
        CanMeleeAttack = 1 << Player_State.MeleeAttack,
    }


    public abstract class APlayerState : AStateSO
    {
        protected CharacterManager characterManager;

        protected CharacterInputAdapter Input => characterManager.characterInputAdapter;
        protected StateMachine<Player_State,Player_Status> stateMachine => characterManager.stateMachine;


        public void Init(CharacterManager characterManager)
        {
            this.characterManager = characterManager;
        }

        public override bool CheckChangeTo()
        {
            return true;
        }

        public override void OnEntry()
        {
        
        }

        public override void OnExit()
        {
        
        }

        public override void OnLateUpdate()
        {
        
        }

        public override void OnSecondUpdate(ref float delta)
        {
        
        }

        public override void OnUpdate(ref float delta)
        {
        
        }

        public override void OnFixedUpdate()
        {
        
        }
    }

}
