using UnityEngine;

namespace CharacterLogic
{
    public abstract class APlayerState_SpecialAbility : APlayerState
    {
        [SerializeField] PlayerSpecialAbilityCooldown cooldown;

        public override void OnEntry() {
            base.OnEntry();
            characterManager.animator.CrossFade("SpecialAttack", characterManager.fadeTime);
        }

        public override void OnExit()
        {
            base.OnExit();
            stateMachine.AddModifier(cooldown);
        }
    }

}
