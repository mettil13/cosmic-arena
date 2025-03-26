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
        private Vector2 initialDirection = Vector2.zero;

        Timer timer;

        public override void Init(CharacterManager characterManager)
        {
            base.Init(characterManager);
            damage = characterManager.characterStats.baseAttackDamage;
        }
        public override void OnEntry()
        {
            base.OnEntry();
            cooldown.SetTimerTime(characterManager.characterStats.baseAttackCooldown);
            timer = new Timer(attackDuration).AddCallBack(Stop);
            initialDirection = new Vector2(Input.LastValidDirection.x, Input.LastValidDirection.y);
        }

        public override void OnUpdate(ref float delta) {
            base.OnUpdate(ref delta);
            timer.Update(ref delta);

            Vector3 inputDirection = new Vector3(initialDirection.x, 0, initialDirection.y);
            Vector3 capsuleCheckHead = characterManager.transform.position + Quaternion.FromToRotation(Vector3.right, inputDirection) * new Vector3(attackOffsetX, 0, attackOffsetY + attackAreaY / 2); 
            Vector3 capsuleCheckTail = characterManager.transform.position + Quaternion.FromToRotation(Vector3.right, inputDirection) * new Vector3(attackOffsetX, 0, attackOffsetY - attackAreaY / 2); 
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
                if(character != null && character != characterManager) {
                    if(characterHealth != null) {
                        characterHealth.TakeDamage(damage, characterManager.gameObject);
                    }

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
