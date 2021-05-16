using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Projectile")]
public class ProjectileInfo : ScriptableObject
{
    /// <summary>
    /// Tempo para recarregar a arma.
    /// </summary>
    public const float RELOAD_TIME = 1f;

    /// <summary>
    /// Dano do projétil.
    /// </summary>
    public int damage = 10;
    /// <summary>
    /// Chance de crítico do projétil.
    /// </summary>
    [Range(0f, 1f)]
    public float criticalChance = 0.1f;
    /// <summary>
    /// Quantos projéteis são disparados por segundo.
    /// </summary>
    public float fireRate = 1f;
    /// <summary>
    /// Quantos projéteis são disparados de uma vez.
    /// </summary>
    public int spreadAmount = 1;
    /// <summary>
    /// Ângulo de espalhamento dos projéteis.
    /// </summary>
    public float spreadAngle = 0f;
    /// <summary>
    /// Munição máxima do projétil.
    /// </summary>
    public int count = 30;
    /// <summary>
    /// Alcance do projétil.
    /// </summary>
    public float range = 100f;
    /// <summary>
    /// Efeito de disparo do projétil.
    /// </summary>
    public GameObject effectPrefab;
    /// <summary>
    /// Prefab do projétil caso ele seja um objeto
    /// ao invés de raycast.
    /// </summary>
    public GameObject projectilePrefab;
    /// <summary>
    /// Efeito de impacto do projétil.
    /// </summary>
    public GameObject impactEffectPrefab;
    /// <summary>
    /// Marca que o projétil deixa.
    /// </summary>
    public GameObject impactPaintEffect;
    /// <summary>
    /// Som do projétil.
    /// </summary>
    public AudioClip sound;
    /// <summary>
    /// Projétil não é simulado por um objeto,
    /// mas sim por um cálculo de ray cast imediato.
    /// </summary>
    public bool UseRayCast { get => projectilePrefab == null; }
}
