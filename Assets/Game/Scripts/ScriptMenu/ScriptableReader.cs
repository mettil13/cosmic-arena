using System;
using System.IO;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

public class ScriptableReader : MonoBehaviour
{
    public static void ReadCharacterSO(string csvName)
    {
        string path = "/Resources/" + csvName + ".csv";
        if (File.Exists(Application.dataPath + path))
        {
            Debug.Log("Trovato!");
            
            string[] lines = File.ReadAllLines(Application.dataPath + path);
            int i = 0;
            foreach (string s in lines)
            {
                if(i != 0)
                {
                    CharacterSO newCharacter = ScriptableObject.CreateInstance<CharacterSO>();
                    string[] splitData = s.Split(",");
                    int j = 0;
                    foreach(string p in splitData)
                    {
                        switch (j)
                        {
                            case 0:
                                newCharacter.characterName = p;
                                break;

                            case 1:
                                newCharacter.health = Convert.ToInt32(p);
                                break;

                            case 2:
                                newCharacter.movementForce = Convert.ToInt32(p);
                                break;

                            case 3:
                                newCharacter.cooldownMovement = Convert.ToInt32(p);
                                break;

                            case 4:
                                newCharacter.baseAttackDamage = Convert.ToInt32(p);
                                break;

                            case 5:
                                string g = p.Replace(".", ",");
                                newCharacter.baseAttackCooldown = float.Parse(g);
                                break;

                            case 6:
                                string y = p.Replace(".", ",");
                                newCharacter.stunTime = float.Parse(y);
                                break;
                        }
                        j++;
                    }
                    newCharacter.name = "Character" + i;
                    Debug.Log(newCharacter.name);
                    AssetDatabase.CreateAsset(newCharacter, "Assets/ScriptMenu/SO/"+ newCharacter.name + ".asset");
                }
                i++;
            }
        }
        else
        {
            Debug.LogError("CSV File not found");
            Debug.Log(csvName);
        }
    }
    
}
