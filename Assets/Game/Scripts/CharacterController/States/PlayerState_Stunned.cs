using UnityEngine;

namespace CharacterLogic
{

    using CommonLogic;
    [CreateAssetMenu(fileName = "PlayerState_Stunned", menuName = "Scriptable Objects/PlayerState/Stunned")]
    public class PlayerState_Stunned : APlayerState
    {
        Timer timer;
        [SerializeField] float duration;
        public override void OnEntry()
        {
            base.OnEntry();
            timer = new Timer(duration).AddCallBack(Expire);
        }

        public void Expire()
        {
            stateMachine.ChangeState(Player_State.Idle);
        }


    }
}
