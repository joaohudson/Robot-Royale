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

    private void Awake()
    {
        health = maxHealth;
    }
}
