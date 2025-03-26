using CharacterLogic;
using UnityEngine;
using UnityEngine.Rendering;

namespace CharacterLogic 
{
    using CommonLogic;
    [CreateAssetMenu(fileName = "PlayerState_Dead", menuName = "Scriptable Objects/PlayerState/Dead")]
    public class PlayerState_Dead : APlayerState {
        [SerializeField] float deathDuration = 3;

        Timer timer;
        public override void OnEntry() {
            base.OnEntry();
            timer = new Timer(deathDuration).AddCallBack(DestroySelf);
            characterManager.stateMachine.AddStatusModifier("None", 0);
            characterManager.animator.CrossFade("Death", characterManager.fadeTime);
        }

        public override void OnUpdate(ref float delta) {
            base.OnUpdate(ref delta);
            timer.Update(ref delta);
        }

        private void DestroySelf() {
            Destroy(characterManager.gameObject);
            
        }

    }
}
