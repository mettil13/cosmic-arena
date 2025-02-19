using UnityEngine;
using StateMachine;

namespace CharacterLogic
{
    public abstract class APlayerState_MeleeAttack : APlayerState
    {
        [SerializeField] PlayerMeleeAttackCooldown cooldown;
        [SerializeField] Timer CastTime;
        public override void OnEntry()
        {
            base.OnEntry();

        }




        public override void OnExit()
        {
            base.OnExit();
            stateMachine.AddModifier(cooldown);
        }
    }

}
