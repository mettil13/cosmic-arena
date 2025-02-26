using SalvatoreTempClasses;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSlotUI : MonoBehaviour
{
    public PlayerInput playerInput;
    private PlayerManager playerManager;

    //[SerializeField] int playerNumber;
    [SerializeField] GameObject panelON, panelOFF;

    public void PlayerON(PlayerInput playerInput) {
        this.playerInput = playerInput;
        playerManager = playerInput.GetComponent<PlayerManager>();
        panelON.SetActive(true);
        panelOFF.SetActive(false);
    }
    public void PlayerOFF() {
        playerInput = null;
        playerManager = null;
        ResetPanel();
        panelON.SetActive(false);
        panelOFF.SetActive(true);
    }

    public void ResetPanel() {

    }
}
