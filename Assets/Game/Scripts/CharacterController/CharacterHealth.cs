using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterManager))]
public class CharacterHealth : MonoBehaviour, IDamageable
{
    [SerializeField]private float hp;
    public UnityEvent<float> HPChanged = new UnityEvent<float>();
    [SerializeField] public float maxHP { get; private set; }
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

    public void InitHp(int maxHP) {
        this.maxHP = maxHP;
        hp = maxHP;
    }
}
