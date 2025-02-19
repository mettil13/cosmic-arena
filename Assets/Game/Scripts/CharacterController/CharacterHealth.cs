using System;
using UnityEngine;
[RequireComponent(typeof(CharacterManager))]
public class CharacterHealth : MonoBehaviour, IDamageable
{
    private float hp;
    public event Action<float> HPChanged;
    readonly float maxHP;
    CharacterManager characterManager;
    public float HP 
    {
        get 
        { 
            return hp;
        }
        set
        {
            hp = Mathf.Clamp(value, 0, maxHP);
            HPChanged.Invoke(HP);
        }
    }


    private void Awake()
    {
        characterManager = GetComponent<CharacterManager>();
    }

    public void TakeDamage(float damage, GameObject dealer)
    {
        HP -= damage;

        if (HP <= 0) 
        {
            characterManager.Die();
        }
    }
}
