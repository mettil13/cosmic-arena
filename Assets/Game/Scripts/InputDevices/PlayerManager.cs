using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public CharacterManager characterPrefab;
    public CharacterManager playerCharacter;
    public PlayerInput playerInput;
    public int playerNumber;

    private void Start() {
        playerInput = GetComponent<PlayerInput>();
    }

    public void OnControllerDisconnected() {
        if (characterPrefab != null)
            playerCharacter.manager = null;
        Destroy(this.gameObject); return;
    }

    public void PossessCharacter(CharacterManager character)
    {
        if (character == null) return;
        character.manager = this;
        playerNumber = character.playerNumber;

        playerCharacter = character;
        playerCharacter.characterInputAdapter = this.GetComponent<CharacterInputAdapter>();
        
    }

    public void SpawnCharacter(Transform spawnPoint) {

        Debug.Log("SpawnCharacter");
        playerCharacter = Instantiate(characterPrefab, spawnPoint);
        playerCharacter.stateMachine.states = characterPrefab.stateMachine.states;
        playerCharacter.Init();
        DevicesManager.instance.characterList.Add(playerCharacter);
        playerCharacter.manager = this;
        playerCharacter.characterInputAdapter = this.GetComponent<CharacterInputAdapter>();
        playerCharacter.playerNumber = playerNumber;
    }

}
