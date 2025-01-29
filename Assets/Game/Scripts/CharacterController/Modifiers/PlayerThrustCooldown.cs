using StateMachine;
using UnityEngine;
namespace CharacterLogic
{
    [System.Serializable]
    public class PlayerThrustCooldown : APlayerTimedModifier
    {
        public override string Name => "ThrustCooldown";

        [SerializeField] Player_Status removedStatus = Player_Status.CanThrust;


        public override void OnEntry(StateMachine<Player_State, Player_Status> stateMachine)
        {
            base.OnEntry(stateMachine);
            stateMachine.AddStatusModifier(Name, ~removedStatus);
            Debug.Log("start Cooldown Thrust");
        }

        public override void OnExit()
        {
            base.OnExit();
            stateMachine.RemoveStatusModifier(Name);
            Debug.Log("elapsed Cooldown Thrust");
        }

    }  

}
