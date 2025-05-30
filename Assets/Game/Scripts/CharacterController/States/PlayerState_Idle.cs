using UnityEngine;
namespace CharacterLogic
{
    [CreateAssetMenu(fileName = "PlayerState_Idle", menuName = "Scriptable Objects/PlayerState/Idle")]
    public class PlayerState_Idle : APlayerState
    {

        public override void OnEntry()
        {
            base.OnEntry();
            characterManager.characterMovementAesthetic.Controlled = true;
            characterManager.animator.CrossFade("Idle", characterManager.fadeTime);
        }

        public override void OnUpdate(ref float delta)
        {
            base.OnUpdate(ref delta);

            if (Input.Thrust > 0) 
            {
                stateMachine.ChangeState(Player_State.Thrusting);
            }

            if (Input.Brake > 0)
            {
                stateMachine.ChangeState(Player_State.Braking);
            }

            if(Input.Attack > 0)
            {
                stateMachine.ChangeState(Player_State.MeleeAttack);
                Input.Attack = 0;
            }

            if (Input.SpecialAbility > 0)
            {
                stateMachine.ChangeState(Player_State.SpecialAbility);
                Input.SpecialAbility = 0;

            }

        }
    }
}
