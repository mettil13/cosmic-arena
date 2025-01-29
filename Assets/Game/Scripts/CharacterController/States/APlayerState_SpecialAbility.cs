using UnityEngine;
using StateMachine;

namespace CharacterLogic
{
    public abstract class APlayerState_SpecialAbility : APlayerState
    {
        [SerializeField] PlayerSpecialAbilityCooldown cooldown;






        public override void OnExit()
        {
            base.OnExit();
            stateMachine.AddModifier(cooldown);
        }
    }

}
