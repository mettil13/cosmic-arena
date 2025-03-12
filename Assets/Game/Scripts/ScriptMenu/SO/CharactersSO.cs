using UnityEngine;

public class CharacterSO : ScriptableObject
{
    public string characterName;
    public int health;
    public int movementForce;
    public int cooldownMovement;
    public int baseAttackDamage;
    public float baseAttackCooldown;
    public float stunTime;
}
