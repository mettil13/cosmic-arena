using DG.Tweening;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

namespace physics
{
    public class CharacterMovementEstetic : MonoBehaviour
    {
        [SerializeField] private Rigidbody body;
        [SerializeField] private bool controlled = true;
        [SerializeField] private float goOnRotationSpeed = 2f;
        [SerializeField] private float dragOnControlled = 1f;
        [SerializeField] private float dragOnNonControlled = 0.5f;
        [SerializeField] private float minMagnitudeToStartLerp = 1f;

        public bool Controlled
        {
            set
            {
                if (value == controlled) return;

                if (value)
                {
                    //body.constraints = RigidbodyConstraints.FreezeAll;
                    body.angularDamping = dragOnControlled;
                }
                else
                {
                    //body.constraints = RigidbodyConstraints.FreezePosition;
                    body.angularDamping = dragOnNonControlled;
                    body.DOKill();
                }

                controlled = value;
            }
            get => controlled;
        }

        public void Init()
        {
            body = GetComponent<Rigidbody>();
            Controlled = false;
            Controlled = true;
        }

        public void ApplyRandomTorque(float intensity)
        {
            body.angularVelocity = Vector3.zero;
            Vector3 random = Random.insideUnitSphere * intensity / 1000;
            body.AddTorque(random);
            //Debug.LogError("random velocity : " + random + " intensity : " + intensity);
        }
        public void InvertTorque()
        {
            body.angularVelocity = -body.angularVelocity;
        }
        public void ResetTorque()
        {
            body.angularVelocity = Vector3.zero;
        }

        public void ApplyDirectionIfControlled(Vector2 direction)
        {
            if (controlled) { ApplyDirection(direction); }
        }
        public void ApplyDirection(Vector2 direction)
        {
            Vector3 rotation = new Vector3(0, -Vector2.SignedAngle(Vector2.right, direction), 0);
            
            if (body.angularVelocity.magnitude < minMagnitudeToStartLerp) 
            {
                body.angularVelocity = Vector3.zero;
                body.DOKill();
                body.DORotate(rotation, goOnRotationSpeed);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
        }
    }
}