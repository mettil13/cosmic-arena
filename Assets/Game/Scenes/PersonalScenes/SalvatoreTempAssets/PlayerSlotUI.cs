using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerSlotUI : MonoBehaviour
{
    [SerializeField] PlayerLobbyUI playerLobbyUI;

    public PlayerInput playerInput;
    private PlayerManager playerManager;
    private int chosenCharacterIndex = 0;
    public bool isReady = false;
    [SerializeField] GameObject isReadyUI;

    InputAction navigateAction;
    InputAction readyAction;

    [SerializeField] GameObject panelON, panelOFF;

    [SerializeField] Image characterDisplay;

    public void Awake()
    {
        playerLobbyUI = GetComponentInParent<PlayerLobbyUI>();
    }
    public void PlayerON(PlayerInput playerInput)
    {
        this.playerInput = playerInput;
        playerManager = playerInput.GetComponent<PlayerManager>();
        playerManager.character = playerLobbyUI.charactersDatas.PlayerCharacter(chosenCharacterIndex);

        navigateAction = playerInput.actions["Navigate"];
        navigateAction.Enable();
        navigateAction.performed += context => OnNavigateAction(context);

        readyAction = playerInput.actions["Submit"];
        readyAction.Enable();
        readyAction.performed += context => OnReadyAction(context);

        characterDisplay.sprite = playerLobbyUI.charactersDatas.Sprite(chosenCharacterIndex);
        panelON.SetActive(true);
        panelOFF.SetActive(false);
    }
    public void PlayerOFF()
    {
        playerInput = null;
        playerManager = null;

        //navigateAction.performed -= context => OnNavigateAction(context);
        navigateAction = null;
        readyAction = null;
        ResetPanel();

        panelON.SetActive(false);
        panelOFF.SetActive(true);
    }

    public void ResetPanel()
    {
        chosenCharacterIndex = 0;
        characterDisplay.sprite = playerLobbyUI.charactersDatas.Sprite(chosenCharacterIndex);
    }

    void OnNavigateAction(InputAction.CallbackContext context)
    {
        if (playerManager == null) return;
        Vector2 input = context.ReadValue<Vector2>();
        int xAxis = (int)input.x;
        if (xAxis == 0) return;
        int characters = playerLobbyUI.charactersDatas.CharactersNumber();

        if (xAxis > 0)
        {
            chosenCharacterIndex++;
            if (chosenCharacterIndex >= characters)
                chosenCharacterIndex -= characters;
            playerManager.character = playerLobbyUI.charactersDatas.PlayerCharacter(chosenCharacterIndex);
            characterDisplay.sprite = playerLobbyUI.charactersDatas.Sprite(chosenCharacterIndex);
        }
        if (xAxis < 0)
        {
            chosenCharacterIndex--;
            if (chosenCharacterIndex < 0)
                chosenCharacterIndex += characters;
            playerManager.character = playerLobbyUI.charactersDatas.PlayerCharacter(chosenCharacterIndex);
            characterDisplay.sprite = playerLobbyUI.charactersDatas.Sprite(chosenCharacterIndex);
        }
    }

    void OnReadyAction(InputAction.CallbackContext context){
        isReady = !isReady;
        isReadyUI.SetActive(isReady);
        if (isReady) {
            navigateAction.Disable();
            playerLobbyUI.isReadyCheck();
        } else
        {
            navigateAction.Enable();
        }
        
    }
}
