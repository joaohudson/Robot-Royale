using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterState : MonoBehaviour
{
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private bool showDamage;

    private int health;
    
    public event Action OnChangeHealth;

    /// <summary>
    /// Vida atual do personagem.
    /// </summary>
    public int Health
    {
        get => health;
        set
        {
            health = value >= 0 ? value : 0;
            OnChangeHealth?.Invoke();
        }
    }

    public float HealthNormalized
    {
        get => (float)health / (float)maxHealth;
    }

    public void TakeDamage(int damage, float criticalChance)
    {
        bool critical = UnityEngine.Random.Range(0f, 1f) <= criticalChance;
        
        if (critical)
            damage *= 2;

        Health -= damage;

        if(showDamage)
            DamageTextManager.Instance.AddDamageText(transform.position, damage, critical);
    }

    private void Awake()
    {
        health = maxHealth;
    }
}
