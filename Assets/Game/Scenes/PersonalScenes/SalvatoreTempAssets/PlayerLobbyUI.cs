using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLobbyUI : MonoBehaviour
{
    [SerializeField] PlayerSlotUI[] slotArray = new PlayerSlotUI[6];
    public CharactersSO charactersDatas;

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
                RearrangePlayers(i);
                return;
            }
        }
    }

    public void RearrangePlayers(int number)
    {
        DevicesManager.instance.RearrangePlayers();
        for (int i = number; i < slotArray.Length; i++) {
            for (int j = 1; i + j < slotArray.Length; j++) {
                if (slotArray[i + j].playerInput == null) continue;
                //Modifica dopo
            }
        }
    }

    public void isReadyCheck()
    {
        for (int i = 0; i < slotArray.Length; i++)
        {
            if (slotArray[i].playerInput != null)
            {
                if (!slotArray[i].isReady)
                    return;
            }
        }
        Debug.Log("CARICAMENTO SCENA");
    }
}
