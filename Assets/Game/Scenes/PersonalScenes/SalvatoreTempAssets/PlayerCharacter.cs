using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SalvatoreTempClasses
{
    public class PlayerCharacter : MonoBehaviour
    {
        public PlayerManager manager;
        public int playerNumber;

        //public void OnMove(InputAction.CallbackContext context)
        //{
        //    Vector2 movement = context.ReadValue<Vector2>();
        //    move = new Vector3(movement.x,0,movement.y);
        //    Debug.Log(move);
        //}
    }
}
