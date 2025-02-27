using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public PlayerCharacterTemp character;
    public int playerNumber;

    public void OnControllerDisconnected() {
        if (character != null)
            character.manager = null;
        Destroy(this.gameObject); return;
    }

}
