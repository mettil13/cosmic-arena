using SalvatoreTempClasses;
using UnityEngine;

public class CharactersSO : ScriptableObject
{
    [SerializeField] private CharacterStruct[] characterArray;

    public PlayerCharacter PlayerCharacter(int index) {
        index = Mathf.Clamp(index, 0, characterArray.Length - 1);
        return characterArray[index].character;
    }

    public Sprite Sprite(int index) {
        index = Mathf.Clamp(index, 0, characterArray.Length - 1);
        return characterArray[index].sprite;
    }
}
