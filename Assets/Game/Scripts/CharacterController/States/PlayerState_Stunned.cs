using UnityEngine;

namespace CharacterLogic
{

    using CommonLogic;
    [CreateAssetMenu(fileName = "PlayerState_Stunned", menuName = "Scriptable Objects/PlayerState/Stunned")]
    public class PlayerState_Stunned : APlayerState
    {
        Timer timer;
        [SerializeField] float duration;

        public override void Init(CharacterManager characterManager)
        {
            base.Init(characterManager);
            duration = characterManager.characterStats.stunTime;
        }
        public override void OnEntry()
        {
            base.OnEntry();
            timer = new Timer(characterManager.characterStats.stunTime).AddCallBack(Expire);
            characterManager.characterMovementAesthetic.Controlled = false;
            characterManager.animator.CrossFade("Stun", characterManager.fadeTime);
        }

        public override void OnUpdate(ref float delta) {
            base.OnUpdate(ref delta);
            timer.Update(ref delta);
        }

        public void Expire()
        {
            characterManager.characterMovementAesthetic.Controlled = true;
            stateMachine.ChangeState(Player_State.Idle);
        }


    }
}
