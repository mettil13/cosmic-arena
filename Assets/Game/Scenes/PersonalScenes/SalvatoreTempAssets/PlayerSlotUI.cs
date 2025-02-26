using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerSlotUI : MonoBehaviour
{
    [SerializeField] PlayerLobbyUI playerLobbyUI;

    public PlayerInput playerInput;
    private PlayerManager playerManager;
    private int chosenCharacterIndex = 0;

    InputAction navigateAction;

    [SerializeField] GameObject panelON, panelOFF;

    [SerializeField] Image characterDisplay;

    public void Awake() {
        playerLobbyUI = GetComponentInParent<PlayerLobbyUI>();
        navigateAction.Enable();
    }
    public void PlayerON(PlayerInput playerInput) {
        this.playerInput = playerInput;
        playerManager = playerInput.GetComponent<PlayerManager>();

        navigateAction = playerInput.actions["Navigate"];
        navigateAction.performed += context => OnNavigateAction(context);

        characterDisplay.sprite = playerLobbyUI.charactersDatas.Sprite(chosenCharacterIndex);
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
        characterDisplay.sprite = playerLobbyUI.charactersDatas.Sprite(chosenCharacterIndex);
    }

    void OnNavigateAction(InputAction.CallbackContext context) {
        if (playerManager == null) return;
        Vector2 input = context.ReadValue<Vector2>();
        int xAxis = (int)input.x;
        if (xAxis == 0) return;
        int characters = playerLobbyUI.charactersDatas.CharactersNumber();

        if (xAxis > 0) {
            chosenCharacterIndex++;
            if (chosenCharacterIndex >= characters)
                chosenCharacterIndex -= characters;
            playerManager.chosenCharacterIndex = chosenCharacterIndex;
            characterDisplay.sprite = playerLobbyUI.charactersDatas.Sprite(chosenCharacterIndex);
        }
        if (xAxis < 0) {
            chosenCharacterIndex--;
            if (chosenCharacterIndex < 0)
                chosenCharacterIndex += characters;
            playerManager.chosenCharacterIndex = chosenCharacterIndex;
            characterDisplay.sprite = playerLobbyUI.charactersDatas.Sprite(chosenCharacterIndex);
        }
    }
}
