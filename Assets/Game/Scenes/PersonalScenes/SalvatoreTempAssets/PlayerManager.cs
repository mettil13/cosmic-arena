using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public CharacterInputAdapter characterPrefab;
    public CharacterInputAdapter playerCharacter;
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

    public void PossessCharacter(CharacterInputAdapter character)
    {
        if (character == null) return;
        character.manager = this;
        playerNumber = character.playerNumber;

        playerCharacter = character;
        SetPlayerInputEvent();
        
    }

    public void SetPlayerInputEvent() {
        //playerInput.actions["Direction"].performed += 
    }
    public void SpawnCharacter() {
        //da capire la transform

        //istanzia il prefab e assegna a playerCharacter
        //ricorda di aggiungerlo alla lista in devicemanager
    }

}
