using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace physics
{
    [System.Serializable] public struct ChildTorqueInfo
    {
        [SerializeField] public CharacterMovementEstetic graphicBody;
        [SerializeField] public float startTorqueIntensity;
        [SerializeField] public float collisionTorqueIntensity;
        [SerializeField] public float MoveTorqueIntensity;
        [SerializeField] public float nonControlTime;
        [SerializeField] public float swerveTime;
    }
    [System.Serializable] public struct MovementInfo
    {
        [SerializeField] public Rigidbody body;
        [SerializeField] public float movementIntensity;
        [SerializeField] public float dynamicDragThresholdImpulse;
        [SerializeField] public float dynamicDragThresholdExplosion;
    }

    public class CharacterPhysics : MonoBehaviour
    {
        public Transform cursor;
        float lastTimeClicked = 0;
        public float coolDown = 0.5f;
        private Vector2 direction;

        [SerializeField] MovementInfo movementInfo;
        [SerializeField] ChildTorqueInfo childTorqueInfo;
        [SerializeField] Dictionary<GameObject, float> modifiers;
        

        private Coroutine nonControlCR;
        private Coroutine swerveCR;
        private bool isSwerving = false;

        private void Awake()
        {
            childTorqueInfo.graphicBody.Init();
            childTorqueInfo.graphicBody.ApplyRandomTorque(childTorqueInfo.startTorqueIntensity);
            direction = Vector2.right;
            modifiers = new Dictionary<GameObject, float>();
        }
        private void Update()
        {
            Vector2 currentDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

            if (currentDirection.x != 0 || currentDirection.y != 0)
            {
                direction = currentDirection;
            }

            if (childTorqueInfo.graphicBody.Controlled)
            {
                childTorqueInfo.graphicBody.ApplyDirection(direction);
            }

            cursor.eulerAngles = new Vector3(0, -Vector2.SignedAngle(Vector2.right, direction), 0);
            childTorqueInfo.graphicBody.transform.localPosition = Vector3.zero;
            if (Input.GetKeyDown(KeyCode.Return) && Time.time - lastTimeClicked > coolDown && isSwerving == false)
            {
                ApplyNonControl();
                MoveWithVector(direction);
                lastTimeClicked = Time.time;
            }

            if (movementInfo.body.linearVelocity.magnitude < movementInfo.dynamicDragThresholdImpulse) {
                movementInfo.body.linearDamping = 0.2f;
                movementInfo.body.angularDamping = 0.05f;
            } else if (movementInfo.body.linearVelocity.magnitude > movementInfo.dynamicDragThresholdImpulse &&
                    movementInfo.body.linearVelocity.magnitude < movementInfo.dynamicDragThresholdExplosion) {
                        movementInfo.body.linearDamping = 0.2f + movementInfo.body.linearVelocity.magnitude / 50.0f;
                        movementInfo.body.angularDamping = 0.05f + movementInfo.body.linearVelocity.magnitude / 500.0f;
            } else {
                movementInfo.body.linearDamping = 0.2f + movementInfo.body.linearVelocity.magnitude / 25.0f;
                movementInfo.body.angularDamping = 0.05f + movementInfo.body.linearVelocity.magnitude / 250.0f;
            }

            //Debug.Log("velocità: " + movementInfo.body.linearVelocity.magnitude);
        }

        public void MoveWithVector(Vector2 movementDirection)
        {
            Vector2 velocityDirection = new Vector2(movementInfo.body.linearVelocity.x, movementInfo.body.linearVelocity.z).normalized;
            float dot = Vector2.Dot(movementDirection, velocityDirection);
            //Debug.LogWarning("DOT value : " + dot + " of movement direction : " + movementDirection + " and velocity direction : " + velocityDirection + 
            //    " in rigidbody of : " + movementInfo.body + " " + movementInfo.body.name);

            if (modifiers.Count != 0)
            {
                movementInfo.body.AddForce(
                    new Vector3(movementDirection.x, 0, movementDirection.y) * movementInfo.movementIntensity * modifiers.Values.ToList()[0],
                    ForceMode.Impulse);
            }
            else
            {
                if (dot < 0) dot = 0;
                    movementInfo.body.linearVelocity *= dot;
                movementInfo.body.AddForce(
                    new Vector3(movementDirection.x, 0, movementDirection.y) * movementInfo.movementIntensity,
                    ForceMode.Impulse);
            }
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

        public void Kick(Transform propPosition, float intensity) {
            Vector3 kickDirection = (transform.position - propPosition.position).normalized * intensity;
            movementInfo.body.linearVelocity = Vector3.zero;
            isSwerving = true;
            movementInfo.body.AddForce(kickDirection, ForceMode.Impulse);
            childTorqueInfo.graphicBody.ApplyRandomTorque(childTorqueInfo.collisionTorqueIntensity * intensity);
            Swerve();
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

        public void Swerve() {
            if (swerveCR != null) StopCoroutine(swerveCR);
            swerveCR = StartCoroutine(SwerveCR());
        }
        public IEnumerator SwerveCR() {
            yield return new WaitForSeconds(childTorqueInfo.swerveTime);
            isSwerving = false;
        }

        public void GenerateMovementModifier(GameObject applier, float modifier)
        {
            if (modifiers.ContainsKey(applier)) modifiers[applier] = modifier;
            else modifiers.Add(applier, modifier);
        }
        public void ClearMovementModifier(GameObject applier)
        {
            modifiers.Remove(applier);
        }
    }
}