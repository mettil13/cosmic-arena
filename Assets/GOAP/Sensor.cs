using System;
using UnityEngine;
namespace GOAP
{
    using CommonLogic;
    using System.Runtime.CompilerServices;

    [RequireComponent(typeof(SphereCollider))]
    public class Sensor : MonoBehaviour
    {
        [SerializeField] float detectionRadius = 5f;
        [SerializeField] float timerInterval = 1f;

        SphereCollider detectionRange;

        public event Action OnTargetChanged = delegate { };

        public Vector3 TargetPosition => target ? target.transform.position: Vector3.zero;
        public bool IsTargetInRange => TargetPosition != Vector3.zero;
        GameObject target;
        Vector3 lastKnownPosition;

        Timer timer;

        private void Awake()
        {
            detectionRange = GetComponent<SphereCollider>();
            detectionRange.isTrigger = true;
            detectionRange.radius = detectionRadius;
        }

        private void Start()
        {
            timer = new Timer(timerInterval);
            timer.OnEnd += () => 
            { 
                UpdateTargetPosition(target); 
                timer.Reset(); 
            };
        }
        private void Update()
        {
            float dt = Time.deltaTime;
            timer.Update(ref dt);
        }
        private void UpdateTargetPosition(GameObject target = null)
        {
            this.target = target;
            if (IsTargetInRange && (lastKnownPosition != TargetPosition || lastKnownPosition != Vector3.zero))
            {
                lastKnownPosition = TargetPosition;
                OnTargetChanged.Invoke();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            UpdateTargetPosition(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            UpdateTargetPosition();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = IsTargetInRange ? Color.red : Color.green;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }

    }

}
