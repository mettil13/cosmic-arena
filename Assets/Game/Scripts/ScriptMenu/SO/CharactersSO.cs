using UnityEngine;

public class CharacterSO : ScriptableObject
{
    public string characterName;
    public int health;
    public float movementForce;
    public float cooldownMovement;
    public int baseAttackDamage;
    public float baseAttackCooldown;
    public float stunTime;
}
