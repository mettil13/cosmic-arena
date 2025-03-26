using UnityEngine;

public class CharactersListSO : ScriptableObject
{
    [SerializeField] private CharacterStruct[] characterArray;

    public int CharactersNumber() { 
    return characterArray.Length;
    }
    public CharacterManager PlayerCharacter(int index) {
        index = Mathf.Clamp(index, 0, characterArray.Length - 1);
        return characterArray[index].characterPrefab;
    }

    public Sprite Sprite(int index) {
        index = Mathf.Clamp(index, 0, characterArray.Length - 1);
        return characterArray[index].sprite;
    }
}
