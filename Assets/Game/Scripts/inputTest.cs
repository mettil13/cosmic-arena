using System.Collections;
using UnityEngine;

namespace physics
{
    [System.Serializable] public struct ChildTorqueInfo
    {
        [SerializeField] public GraphicTest graphicBody;
        [SerializeField] public float startTorqueIntensity;
        [SerializeField] public float collisionTorqueIntensity;
        [SerializeField] public float MoveTorqueIntensity;
        [SerializeField] public float nonControlTime;
    }
    [System.Serializable] public struct MovementInfo
    {
        [SerializeField] public Rigidbody body;
        [SerializeField] public float movementIntensity;
    }

    public class inputTest : MonoBehaviour
    {
        public Transform cursor;
        float lastTimeClicked = 0;
        public float coolDown = 0.5f;
        private Vector2 direction;

        [SerializeField] MovementInfo movementInfo;
        [SerializeField] ChildTorqueInfo childTorqueInfo;

        private Coroutine nonControlCR;

        private void Awake()
        {
            childTorqueInfo.graphicBody.Init();
            childTorqueInfo.graphicBody.ApplyRandomTorque(childTorqueInfo.startTorqueIntensity);
        }
        private void Update()
        {
            Vector2 currentDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
            if (currentDirection.x != 0 || currentDirection.y != 0)
            {
                if (childTorqueInfo.graphicBody.Controlled && currentDirection != direction)
                {
                    childTorqueInfo.graphicBody.ApplyDirection(currentDirection);
                }

                direction = currentDirection;
            }
            cursor.eulerAngles = new Vector3(0, -Vector2.SignedAngle(Vector2.right, direction), 0);
            childTorqueInfo.graphicBody.transform.localPosition = Vector3.zero;
            if (Input.GetKeyDown(KeyCode.Return) && Time.time - lastTimeClicked > coolDown)
            {
                ApplyNonControl();
                MoveWithVector(direction);
                lastTimeClicked = Time.time;
            }
        }

        public void MoveWithVector(Vector2 movementDirection)
        {
            movementInfo.body.AddForce(
                new Vector3(movementDirection.x, 0, movementDirection.y) * movementInfo.movementIntensity, 
                ForceMode.Impulse);
            childTorqueInfo.graphicBody.ApplyRandomTorque(childTorqueInfo.MoveTorqueIntensity);
        }
        public void MoveWithVectorReset(Vector2 movementDirection)
        {
            movementInfo.body.linearVelocity = Vector3.zero;
            MoveWithVector(movementDirection);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.tag == "Wall")
            {
                movementInfo.body.angularVelocity = -movementInfo.body.angularVelocity;
                childTorqueInfo.graphicBody.ApplyRandomTorque(childTorqueInfo.collisionTorqueIntensity);
            }
        }

        public void ApplyNonControl()
        {
            if (nonControlCR != null) StopCoroutine(nonControlCR);
            nonControlCR = StartCoroutine(NonControlCR());
        }
        public IEnumerator NonControlCR()
        {
            childTorqueInfo.graphicBody.Controlled = false;
            yield return new WaitForSeconds(childTorqueInfo.nonControlTime);
            childTorqueInfo.graphicBody.Controlled = true;
        }
    }
}