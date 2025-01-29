using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace physics
{
    public class GraphicTest : MonoBehaviour
    {
        [SerializeField] private Rigidbody body;
        [SerializeField] private bool controlled = true;

        public bool Controlled
        {
            set
            {
                if (value == controlled) return;

                if (value)
                {
                    body.constraints = RigidbodyConstraints.FreezeAll;
                }
                else
                {
                    body.constraints = RigidbodyConstraints.FreezePosition;
                    transform.DOKill();
                }

                controlled = value;
            }
            get => controlled;
        }

        public void Init()
        {
            body = GetComponent<Rigidbody>();
            Controlled = true;
        }
        public void ApplyRandomTorque(float intensity)
        {
            body.angularVelocity = Vector3.zero;
            //Debug.LogError("angular velocity : " + body.angularVelocity);
            body.AddTorque(Random.insideUnitSphere * intensity);
        }
        public void InvertTorque()
        {
            body.angularVelocity = -body.angularVelocity;
        }
        public void ResetTorque()
        {
            body.angularVelocity = Vector3.zero;
        }
        public void ApplyDirection(Vector2 direction)
        {
            Vector3 rotation = new Vector3(0, -Vector2.SignedAngle(Vector2.right, direction), 0);
            transform.DOKill();
            transform.DORotate(rotation, 2);
        }

        private void OnCollisionEnter(Collision collision)
        {
            //Debug.Log("a");
        }
    }
}