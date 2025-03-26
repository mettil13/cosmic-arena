using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DevicesManager : MonoBehaviour
{
    public static DevicesManager instance = null;

    public PlayerInput[] playerArray = new PlayerInput[6];

    public List<CharacterManager> characterList = new List<CharacterManager>();

    [SerializeField] InputAction joinAction;
    [SerializeField] InputAction leaveAction;


    //EVENTS
    public event System.Action<PlayerInput> OnPlayerJoinedGame;
    public event System.Action<PlayerInput> OnPlayerLeftGame;

    private void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);


        joinAction.Enable();
        joinAction.performed += context => JoinAction(context);
        leaveAction.Enable();
        leaveAction.performed += context => LeaveAction(context);

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start() {
        PlayerInputManager.instance.JoinPlayer(0, -1, null);
    }

    void OnPlayerJoined(PlayerInput playerInput) {
        for (int i = 0; i < playerArray.Length; i++) {
            if (playerArray[i] == null) {
                playerArray[i] = playerInput;
                playerInput.transform.parent = this.transform;

                PlayerManager manager = playerInput.gameObject.GetComponent<PlayerManager>();
                manager.playerNumber = i;
                manager.PossessCharacter(CharacterWithoutManagerFound());

                break;
            }
        }
        Debug.Log("Player joined");
        if (OnPlayerJoinedGame != null) {
            OnPlayerJoinedGame(playerInput);
        }
    }

    void OnPlayerLeft(PlayerInput playerInput)
    {
        Debug.Log("Player left");
        if (OnPlayerLeftGame != null)
        {
            OnPlayerLeftGame(playerInput);
        }
    }

    void JoinAction(InputAction.CallbackContext context) {
        PlayerInputManager.instance.JoinPlayerFromActionIfNotAlreadyJoined(context);
    }

    void LeaveAction(InputAction.CallbackContext context) {
        if (ConnectedDevices() <= 1) return;

        for (int i = 0; i < playerArray.Length; i++) {
            if (playerArray[i] == null) continue;

            foreach (var device in playerArray[i].devices) {
                if (device != null && context.control.device == device) {
                    UnregisterPlayer(playerArray[i]);
                    return;
                }

            }

        }
    }

    void UnregisterPlayer(PlayerInput playerInput) {

        for (int i = 0; i < playerArray.Length; i++) {
            if (playerArray[i] == playerInput) {
                playerArray[i] = null;
                playerInput.GetComponent<PlayerManager>().OnControllerDisconnected();
                break;
            }
        }

        RearrangePlayers();

    }

    public void RearrangePlayers()
    {
        for (int i = 0; i < playerArray.Length; i++)
        {

            if (playerArray[i] == null)
            {
                for (int j = 1; i + j < playerArray.Length; j++)
                {
                    if (playerArray[i + j] != null)
                    {
                        playerArray[i] = playerArray[i + j];
                        //playerArray[i].GetComponent<PlayerManager>().playerNumber = i;
                        playerArray[i + j] = null;
                        break;
                    }
                }
            }
        }
    }

    public int ConnectedDevices() {
        int number = 0;
        for (int i = 0; i < playerArray.Length; i++) {
            if (playerArray[i] == null) continue;
            number++;
        }
        return number;
    }

    CharacterManager CharacterWithoutManagerFound() {
        foreach (var character in characterList) {
            if (character.manager == null)
                return character;
        }
        return null;
    }

    public void SpawnPlayers(Transform[] spawnPoints) {
        for (int i = 0;i < playerArray.Length;i++) {
            if (playerArray[i] == null) continue;
            PlayerManager player = playerArray[i].GetComponent<PlayerManager>();
            player.SpawnCharacter(spawnPoints[i]);

        }
    }

}