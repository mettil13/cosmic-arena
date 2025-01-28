using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SalvatoreTempClasses
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerControllerTemp : MonoBehaviour
    {
        private CharacterController controller;
        private Vector3 playerVelocity;
        //private bool groundedPlayer;
        [SerializeField] private float playerSpeed = 2.0f;
        //private float jumpHeight = 1.0f;
        //private float gravityValue = -9.81f;

        private Vector3 move;

        private void Awake()
        {
            controller = gameObject.GetComponent<CharacterController>();
        }

        void Update()
        {
            //groundedPlayer = controller.isGrounded;
            //if (groundedPlayer && playerVelocity.y < 0)
            //{
            //    playerVelocity.y = 0f;
            //}

            controller.Move(move * Time.deltaTime * playerSpeed);

            if (move != Vector3.zero)
            {
                gameObject.transform.forward = move;
            }

            // Makes the player jump
            //if (Input.GetButtonDown("Jump") && groundedPlayer)
            //{
            //    playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
            //}

            //playerVelocity.y += gravityValue * Time.deltaTime;
            //controller.Move(playerVelocity * Time.deltaTime);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Vector2 movement = context.ReadValue<Vector2>();
            move = new Vector3(movement.x,0,movement.y);
            Debug.Log(move);
        }
    }
}
