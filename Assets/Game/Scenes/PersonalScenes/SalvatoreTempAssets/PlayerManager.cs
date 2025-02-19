using UnityEngine;
using UnityEngine.InputSystem;

namespace SalvatoreTempClasses
{
    public class PlayerManager : MonoBehaviour
    {
        public GameObject playerPrefab;
        public PlayerCharacter character;
        public int playerNumber;
        public int chosenCharacterNumber;


        public void OnControllerDisconnected() {
            if (character != null)
                character.manager = null;
            Destroy(this.gameObject); return;
        }

        //public void OnMove(InputAction.CallbackContext context)
        //{
        //    character.OnMove(context);
        //}
    }
}
