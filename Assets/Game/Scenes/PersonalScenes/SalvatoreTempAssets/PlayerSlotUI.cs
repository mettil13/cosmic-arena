using SalvatoreTempClasses;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSlotUI : MonoBehaviour
{
    public PlayerInput playerInput;
    private PlayerManager playerManager;
    private int chosenCharacterIndex = 0;

    InputAction navigateAction;

    //[SerializeField] int playerNumber;
    [SerializeField] GameObject panelON, panelOFF;

    public void Awake() {
        navigateAction.Enable();
    }
    public void PlayerON(PlayerInput playerInput) {
        this.playerInput = playerInput;
        playerManager = playerInput.GetComponent<PlayerManager>();

        navigateAction = playerInput.actions["Navigate"];
        navigateAction.performed += context => OnNavigateAction(context);

        panelON.SetActive(true);
        panelOFF.SetActive(false);
    }
    public void PlayerOFF() {
        playerInput = null;
        playerManager = null;

        navigateAction.performed -= context => OnNavigateAction(context);
        navigateAction = null;
        ResetPanel();

        panelON.SetActive(false);
        panelOFF.SetActive(true);
    }

    public void ResetPanel() {
        chosenCharacterIndex = 0;
    }

    void OnNavigateAction(InputAction.CallbackContext context) {
        if (playerManager == null) return;
        Vector2 input = context.ReadValue<Vector2>();
        int xAxis = (int)input.x;
        if (xAxis > 0) {
            chosenCharacterIndex++;
            playerManager.chosenCharacterIndex++;
        }
        if (xAxis < 0) {
            chosenCharacterIndex--;
            playerManager.chosenCharacterIndex--;
        }
    }
}
