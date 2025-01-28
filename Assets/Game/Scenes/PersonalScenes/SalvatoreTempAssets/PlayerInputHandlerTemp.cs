using UnityEngine;
using UnityEngine.InputSystem;

namespace SalvatoreTempClasses
{
    public class PlayerInputHandlerTemp : MonoBehaviour
    {
        public GameObject playerPrefab;
        public PlayerControllerTemp controller;

        private void Awake()
        {
            if (playerPrefab == null) return;
            controller = Instantiate(playerPrefab, GameManagerTemp.instance.spawnPoints[0].transform.position, transform.rotation).GetComponent<PlayerControllerTemp>();
            transform.parent = controller.transform;
            transform.position = controller.transform.position;
        }
        public void OnMove(InputAction.CallbackContext context)
        {
            controller.OnMove(context);
        }
    }
}
