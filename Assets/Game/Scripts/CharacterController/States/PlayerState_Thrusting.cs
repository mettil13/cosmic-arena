using UnityEngine;

namespace CharacterLogic
{
    using System.Linq;
    using CommonLogic;
    using physics;
    using StateMachine;

    [CreateAssetMenu(fileName = "PlayerState_Thrusting", menuName = "Scriptable Objects/PlayerState/Thrusting")]
    public class PlayerState_Thrusting : APlayerState
    {
        [SerializeField] float inputThreshold = 0.1f;
        [SerializeField] float impulseDuration = 0.1f;
        [SerializeField] bool canStopEarly;
        [SerializeField] float torqueIntensity;
        [SerializeField] float continiousForce = 2f;
        [SerializeField] ForceMode continiousForceMode = ForceMode.Acceleration;
        [SerializeField] float firstImpluse = 10f;
        [SerializeField] ForceMode firstImpulseMode = ForceMode.Impulse;
        [SerializeField] bool hasCooldown = true;
        [SerializeField] PlayerThrustCooldown cooldown;


        CommonLogic.Timer timer;
        public override void OnEntry()
        {
            base.OnEntry();
            AddForceInInputDirection(firstImpluse, firstImpulseMode);
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

            //AddForceInInputDirection(continiousForce, continiousForceMode);
        }

        void AddForceInInputDirection(float magnitude, ForceMode forceMode)
        {

            Vector2 velocityDirection = new Vector2(
                characterManager.rigidbody.linearVelocity.x,
                characterManager.rigidbody.linearVelocity.z).normalized;
            Vector2 direction = Input.LastValidDirection;
            float dot = Vector2.Dot(direction, velocityDirection);
            //Debug.LogWarning("DOT value : " + dot + " of movement direction : " + movementDirection + " and velocity direction : " + velocityDirection + 
            //    " in rigidbody of : " + movementInfo.body + " " + movementInfo.body.name);
            dot = (dot + 1) / 2;


            if (characterManager.modifiers.Count != 0)
            {
                characterManager.rigidbody.AddForce(
                new Vector3(direction.x, 0, direction.y) * magnitude * characterManager.modifiers.Values.ToList()[0],
                    forceMode);
            }
            else
            {
                if (dot < 0) dot = 0;
                characterManager.rigidbody.linearVelocity *= dot;
                characterManager.rigidbody.AddForce(
                    new Vector3(direction.x, 0, direction.y) * magnitude,
                    forceMode);
            }

            characterManager.characterMovementAesthetic.ApplyRandomTorque(torqueIntensity);

            //Vector3 force = (Vector3.forward * Input.Direction.y + Vector3.right * Input.Direction.x).normalized;
            //force *= magnitude;
            //characterManager.rigidbody.AddForce(force, forceMode);
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
