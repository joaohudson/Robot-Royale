using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterState : MonoBehaviour
{
    [SerializeField]
    private int maxHealth;

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
        if(UnityEngine.Random.Range(0f, 1f) <= criticalChance)
            damage *= 2;

        Health -= damage;
    }

    private void Awake()
    {
        health = maxHealth;
    }
}
