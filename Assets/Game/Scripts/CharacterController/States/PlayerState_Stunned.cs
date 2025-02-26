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
            characterManager.characterPhysics.Controlled = false;
        }

        public override void OnUpdate(ref float delta) {
            base.OnUpdate(ref delta);
            timer.Update(ref delta);
        }

        public void Expire()
        {
            characterManager.characterPhysics.Controlled = true;
            stateMachine.ChangeState(Player_State.Idle);
        }


    }
}
