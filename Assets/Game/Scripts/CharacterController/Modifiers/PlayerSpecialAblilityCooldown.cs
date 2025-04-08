using CharacterLogic;
using StateMachine;
using UnityEngine;

namespace CharacterLogic
{
    [System.Serializable]
    public class PlayerSpecialAbilityCooldown : APlayerTimedModifier
    {
        public override string Name => "SpecialAbilityCooldown";

        [SerializeField] Player_Status removedStatus = Player_Status.CanSpecialAbility;

        public PlayerSpecialAbilityCooldown(float duration) : base(duration) {
        }

        public override void OnEntry(StateMachine<Player_State, Player_Status> stateMachine)
        {
            base.OnEntry(stateMachine);
            stateMachine.AddStatusModifier(Name, ~removedStatus);
        }

        public override void OnExit()
        {
            base.OnExit();
            stateMachine.RemoveStatusModifier(Name);
        }

    }

}