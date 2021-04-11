using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct DamageEvent
{
    /// <summary>
    /// Valor do dano causado.
    /// </summary>
    public int damage;
    /// <summary>
    /// Se foi crítico.
    /// </summary>
    public bool critical;
}

public class CharacterState : MonoBehaviour
{
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private bool showDamage;

    private int health;
    
    /// <summary>
    /// Chamado quando a vida deste personagem mudar.
    /// </summary>
    public event Action OnChangeHealth;

    /// <summary>
    /// Chamado quando este personagem receber dano.
    /// </summary>
    public event Action<DamageEvent> OnDamage;

    /// <summary>
    /// Chamado quando este personagem morrer.
    /// </summary>
    public event Action OnDeath;

    /// <summary>
    /// Vida atual do personagem.
    /// </summary>
    public int Health
    {
        get => health;
        private set
        {
            health = value >= 0 ? value : 0;
            OnChangeHealth?.Invoke();
        }
    }

    public float HealthNormalized
    {
        get => (float)health / (float)maxHealth;
    }

    public void Kill()
    {
        Health = 0;
        OnDamage?.Invoke(new DamageEvent { damage = Health, critical = false });
        OnDeath?.Invoke();
    }

    public void TakeDamage(int damage, float criticalChance)
    {
        bool critical = UnityEngine.Random.Range(0f, 1f) <= criticalChance;
        
        if (critical)
            damage *= 2;

        Health -= damage;

        OnDamage?.Invoke(new DamageEvent() { damage = damage, critical = critical});

        if (Health == 0)
            OnDeath?.Invoke();

        if(showDamage)
            DamageTextManager.Instance.AddDamageText(transform.position, damage, critical);
    }

    private void Awake()
    {
        health = maxHealth;
    }
}
