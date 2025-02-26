using UnityEngine;

public class CharactersSO : ScriptableObject
{
    [SerializeField] private CharacterStruct[] characterArray;

    public int CharactersNumber() { 
    return characterArray.Length;
    }
    public PlayerCharacterTemp PlayerCharacter(int index) {
        index = Mathf.Clamp(index, 0, characterArray.Length - 1);
        return characterArray[index].characterPrefab;
    }

    public Sprite Sprite(int index) {
        index = Mathf.Clamp(index, 0, characterArray.Length - 1);
        return characterArray[index].sprite;
    }
}
