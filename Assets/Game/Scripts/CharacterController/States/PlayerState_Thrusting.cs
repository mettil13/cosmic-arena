using Unity.VisualScripting;
using UnityEngine;

namespace CharacterLogic
{
    [CreateAssetMenu(fileName = "PlayerState_Thrusting", menuName = "Scriptable Objects/PlayerState/Thrusting")]
    public class PlayerState_Thrusting : APlayerState
    {
        [SerializeField] float inputThreshold = 0.1f;
        [SerializeField] float impulseDuration = 0.1f;
        [SerializeField] bool canStopEarly;
        [SerializeField] float continiousForce = 2f;
        [SerializeField] ForceMode continiousForceMode = ForceMode.Acceleration;
        [SerializeField] float firstImpluse = 10f;
        [SerializeField] ForceMode firstImpulseMode = ForceMode.Impulse;
        [SerializeField] bool hasCooldown = true;
        [SerializeField] PlayerThrustCooldown cooldown;


        Timer timer;
        public override void OnEntry()
        {
            base.OnEntry();

            timer = new Timer(impulseDuration).AddCallBack(Stop);
        }
        public override void OnUpdate(ref float delta)
        {
            base.OnUpdate(ref delta);
            timer.Update(ref delta);
            if (canStopEarly && Input.Thrust < inputThreshold)
            {
                stateMachine.ChangeState(Player_State.Idle);
            }
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            AddForceInInputDirection(continiousForce, continiousForceMode);
        }

        void AddForceInInputDirection(float magnitude, ForceMode forceMode)
        {
            Vector3 force = (Vector3.up * Input.Direction.y + Vector3.left * Input.Direction.x).normalized;
            force *= magnitude;
            characterManager.rigidbody.AddForce(force, forceMode);
        }

        void Stop()
        {
            stateMachine.ChangeState(Player_State.Idle);
        }

        public override void OnExit()
        {
            base.OnExit();
            timer.Stop();
            if (hasCooldown) stateMachine.AddModifier(cooldown);
        }


    }

}
