using StateMachine;
using UnityEngine;
namespace CharacterLogic
{
    [System.Serializable]
    public class PlayerThrustCooldown : APlayerTimedModifier
    {
        public override string Name => "ThrustCooldown";

        [SerializeField] Player_Status removedStatus = Player_Status.CanThrust;

        public PlayerThrustCooldown(float duration) : base(duration) {
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
