using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerSlotUI : MonoBehaviour
{

    public PlayerInput playerInput;
    public PlayerManager playerManager;
    public PlayerLobbyUI playerLobbyUI;
    private RectTransform rectTransform;

    private int chosenCharacterIndex = 0;
    public bool isReady = false;

    public Image characterDisplay;
    [SerializeField] TextMeshProUGUI playerName;
    [SerializeField] GameObject isReadyUI;

    InputAction navigateAction;
    InputAction readyAction;

    public void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        ResetNumber();

        navigateAction = playerInput.actions["Navigate"];
        navigateAction.Enable();
        navigateAction.performed += OnNavigateAction;

        readyAction = playerInput.actions["Submit"];
        readyAction.Enable();
        readyAction.performed += OnReadyAction;
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
            playerManager.characterPrefab = playerLobbyUI.charactersDatas.PlayerCharacter(chosenCharacterIndex);
            characterDisplay.sprite = playerLobbyUI.charactersDatas.Sprite(chosenCharacterIndex);
            Debug.Log(chosenCharacterIndex);
        }
        if (xAxis < 0)
        {
            chosenCharacterIndex--;
            if (chosenCharacterIndex < 0)
                chosenCharacterIndex += characters;
            playerManager.characterPrefab = playerLobbyUI.charactersDatas.PlayerCharacter(chosenCharacterIndex);
            characterDisplay.sprite = playerLobbyUI.charactersDatas.Sprite(chosenCharacterIndex);
            Debug.Log(chosenCharacterIndex);
        }
    }

    void OnReadyAction(InputAction.CallbackContext context) {
        isReady = !isReady;
        isReadyUI.SetActive(isReady);
        if (isReady) {
            navigateAction?.Disable();
            playerLobbyUI.isReadyCheck();
        } else
        {
            navigateAction?.Enable();
        }

    }

    public void ResetNumber()
    {
        int index = rectTransform.GetSiblingIndex() + 1;
        playerName.text = "Player " + index.ToString();
        playerManager.playerNumber = index;
    }

    public void ResetNumber(int index)
    {
        playerName.text = "Player " + index.ToString();
        playerManager.playerNumber = index;
    }

    public void DeleteSlot()
    {
        navigateAction.performed -= OnNavigateAction;
        readyAction.performed -= OnReadyAction;
        navigateAction.Disable();
        readyAction.Disable();
        navigateAction = null;
        readyAction = null;
        Destroy(this.gameObject);
    }

    public void OnDestroy() {
        navigateAction.performed -= OnNavigateAction;
        readyAction.performed -= OnReadyAction;
    }
}
