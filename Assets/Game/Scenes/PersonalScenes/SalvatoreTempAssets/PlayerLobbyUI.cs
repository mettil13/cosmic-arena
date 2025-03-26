using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerLobbyUI : MonoBehaviour
{
    //[SerializeField] PlayerSlotUI[] slotArray = new PlayerSlotUI[6];

    [SerializeField] PlayerSlotUI slotTemplate;
    [SerializeField] List<PlayerSlotUI> slotList = new List<PlayerSlotUI>();
    public CharactersListSO charactersDatas;

    private void Awake() {
        DevicesManager.instance.OnPlayerJoinedGame += PlayerRegister;
        DevicesManager.instance.OnPlayerLeftGame += PlayerUnregister;
    }
    private void PlayerRegister(PlayerInput playerInput) {
        PlayerSlotUI newSlotUI = Instantiate(slotTemplate, this.transform);
        newSlotUI.playerInput = playerInput;
        newSlotUI.playerManager = playerInput.GetComponent<PlayerManager>();
        newSlotUI.playerLobbyUI = this;

        newSlotUI.playerManager.characterPrefab = charactersDatas.PlayerCharacter(0);
        newSlotUI.characterDisplay.sprite = charactersDatas.Sprite(0);
        slotList.Add(newSlotUI);

    }

    private void PlayerUnregister(PlayerInput playerInput) {

        for (int i = 0; i < slotList.Count; i++) {
            if (slotList[i].playerInput == playerInput)
            {
                slotList[i].DeleteSlot();
                slotList.RemoveAt(i);
            }
        }

        for (int i = 0; i < slotList.Count; i++) { 
            slotList[i].ResetNumber(i+1); 
        }

    }

    public void isReadyCheck()
    {
        foreach (PlayerSlotUI slotUI in slotList)
        {
            if (!slotUI.isReady)
                return;
        }

        Debug.Log("CARICAMENTO SCENA");
        SceneManager.LoadScene("TemplateArena");
    }
}
