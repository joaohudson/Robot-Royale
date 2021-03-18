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

    public int damage = 10;
    [Range(0f, 1f)]
    public float criticalChance = 0.1f;
    public float fireRate = 1f;
    public int spreadAmount = 1;
    public float spreadAngle = 0f;
    public int count = 30;
    public float range = 100f;
    public GameObject effectPrefab;
    public GameObject projectilePrefab;
    public GameObject impactEffectPrefab;
    /// <summary>
    /// Projétil não é simulado por um objeto,
    /// mas sim por um cálculo de ray cast imediato.
    /// </summary>
    public bool UseRayCast { get => projectilePrefab == null; }
}
