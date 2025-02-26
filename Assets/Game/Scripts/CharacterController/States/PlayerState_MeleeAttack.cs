using UnityEngine;
using StateMachine;

namespace CharacterLogic
{
    using CommonLogic;
    [CreateAssetMenu(fileName = "PlayerState_MeleeAttack", menuName = "Scriptable Objects/PlayerState/MeleeAttack")]
    public class PlayerState_MeleeAttack : APlayerState
    {
        [SerializeField] PlayerMeleeAttackCooldown cooldown;
        [SerializeField] Timer CastTime;
        [SerializeField] float attackDuration = 0.5f;
        [SerializeField] float attackAreaX = 0.5f, attackAreaY = 0.5f;
        [SerializeField] float attackOffsetX = 0.5f, attackOffsetY = 0f;
        [SerializeField] float damage = 1;

        Timer timer;
        public override void OnEntry()
        {
            base.OnEntry();
            timer = new Timer(attackDuration).AddCallBack(Stop);
        }

        public override void OnUpdate(ref float delta) {
            base.OnUpdate(ref delta);
            timer.Update(ref delta);

            Vector3 inputDirection = new Vector3(-Input.LastValidDirection.x, Input.LastValidDirection.y, 0);
            Vector3 capsuleCheckHead = characterManager.transform.position + Quaternion.FromToRotation(Vector3.right, inputDirection) * new Vector3(attackOffsetX, attackOffsetY + attackAreaY / 2, 0); 
            Vector3 capsuleCheckTail = characterManager.transform.position + Quaternion.FromToRotation(Vector3.right, inputDirection) * new Vector3(attackOffsetX, attackOffsetY - attackAreaY / 2, 0); 
            Debug.DrawLine(capsuleCheckHead, characterManager.transform.position, Color.yellow, 1);
            Debug.DrawLine(capsuleCheckTail, characterManager.transform.position, Color.yellow, 1);
            Debug.DrawLine(capsuleCheckHead, capsuleCheckTail, Color.yellow, 1);
            //Debug.Log(inputDirection);
            //Debug.Log(capsuleCheckHead + " " +  capsuleCheckTail + " " + characterManager.transform.position);
            Collider[] overlappingColliders = Physics.OverlapCapsule(capsuleCheckHead, capsuleCheckTail, attackAreaX / 2, LayerMask.GetMask("Player"));
            if(overlappingColliders.Length > 0) {
                //Debug.Log(overlappingColliders[0].name);
                CharacterManager character = overlappingColliders[0].GetComponent<CharacterManager>();
                CharacterHealth characterHealth = overlappingColliders[0].GetComponent<CharacterHealth>();
                if(characterHealth != null) {
                    characterHealth.TakeDamage(damage, characterManager.gameObject);
                }
                if(character != null) {
                    character.stateMachine.ChangeState(Player_State.Stunned);
                }
                Stop();
            }

        }

        void Stop() {
            stateMachine.ChangeState(Player_State.Idle);
        }


        public override void OnExit()
        {
            base.OnExit();
            stateMachine.AddModifier(cooldown);
        }
    }

}
