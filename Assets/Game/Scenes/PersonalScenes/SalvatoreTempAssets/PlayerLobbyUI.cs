using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLobbyUI : MonoBehaviour
{
    [SerializeField] PlayerSlotUI[] slotArray = new PlayerSlotUI[6];

    private void Awake() {
        DevicesManager.instance.OnPlayerJoinedGame += PlayerRegister;
        DevicesManager.instance.OnPlayerLeftGame += PlayerUnregister;
    }
    private void PlayerRegister(PlayerInput playerInput) {
        for (int i = 0; i < slotArray.Length; i++) {
            if (DevicesManager.instance.playerArray[i] == playerInput) {
                slotArray[i].PlayerON(playerInput);
                return;
            }
        }
    }

    private void PlayerUnregister(PlayerInput playerInput) {
        for (int i = 0; i < slotArray.Length; i++) {
            if (slotArray[i].playerInput == playerInput) {
                slotArray[i].PlayerOFF();
                return;
            }
        }
    }
}
