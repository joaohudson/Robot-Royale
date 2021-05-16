using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    /// <summary>
    /// Chamado quando a arma começa a ser
    /// recarregada.
    /// </summary>
    public event System.Action OnReload;

    /// <summary>
    /// Chamado quando a munição mudar.
    /// </summary>
    public event System.Action OnChangeAmmo;

    /// <summary>
    /// Quantidade de munição atual.
    /// </summary>
    public int Ammo
    {
        get => ammo;
        private set
        {
            ammo = value;
            OnChangeAmmo?.Invoke();
        }
    }

    [SerializeField]
    private ProjectileInfo projectile;
    [SerializeField]
    private Transform firePoint;
    [SerializeField]
    private string target;
    [SerializeField]
    private AudioSource fireSound;
    [SerializeField]
    private AudioSource reloadSound;

    private float interval = 0f;
    private float reloadTime = 0f;
    private int ammo;

    private void Start()
    {
        InitProjectile();
    }

    private void Update()
    {
        if(interval > 0f)
        {
            interval -= Time.deltaTime;
        }

        if(reloadTime > 0f)
        {
            reloadTime -= Time.deltaTime;
            if(reloadTime <= 0f)
            {
                Ammo = projectile.count;
            }
        }
    }

    /// <summary>
    /// Inicializa o projétil.
    /// </summary>
    private void InitProjectile()
    {
        Ammo = projectile.count;
        if (fireSound != null)
        {
            fireSound.clip = projectile.sound;
        }
    }

    /// <summary>
    /// Ponto de onde o disparo é efetuado.
    /// </summary>
    public Transform FirePoint
    {
        get => firePoint;
    }

    /// <summary>
    /// O projétil desta arma.
    /// </summary>
    public ProjectileInfo Projectile
    {
        get => projectile;
        set {
            projectile = value;
            InitProjectile();
        }
    }

    /// <summary>
    /// Se a arma está sendo recarregada.
    /// </summary>
    public bool Reloading
    {
        get => reloadTime > 0f;
    }

    /// <summary>
    /// Recarrega a arma.
    /// </summary>
    public void Reload()
    {
        DoReload();
    }

    /// <summary>
    /// Dispara um projétil.
    /// </summary>
    /// <param name="direction">A direção do projétil.</param>
    public void Fire(Vector3 direction)
    {
        if (interval > 0f || reloadTime > 0f)
            return;

        Ammo--;
        if(Ammo == 0)
        {
            DoReload();
        }

        interval = 1f / projectile.fireRate;
        Instantiate(projectile.effectPrefab, firePoint.position, Quaternion.identity, transform);

        if(fireSound != null)
        {
            fireSound.Play();
        }

        if (projectile.isPhysicProjectile)
        {
            Instantiate(projectile.projectilePrefab, firePoint.position, Quaternion.LookRotation(direction)).GetComponent<Projectile>().Build(target);
        }
        else
        {
            FireRayCast(direction);
        }
    }

    private void DoReload()
    {
        //Arma já está recarregada ou já está recarregando
        if (ammo == projectile.count || Reloading)
            return;

        reloadSound?.Play();
        reloadTime = ProjectileInfo.RELOAD_TIME;
        OnReload?.Invoke();
    }

    private void FireRayCast(Vector3 direction)
    {
        RaycastHit hit;
        Quaternion rot = Quaternion.identity;

        for(int i = 0; i < projectile.spreadAmount; i++)
        {
            if (Physics.Raycast(firePoint.position, rot * direction, out hit, projectile.range))
            {
                if (hit.collider.CompareTag(target))
                {
                    var state = hit.collider.GetComponent<CharacterState>();
                    state.TakeDamage(projectile.damage, projectile.criticalChance);
                }
                Instantiate(projectile.impactEffectPrefab, hit.point, Quaternion.identity, hit.collider.transform);
                
                if(projectile.impactPaintEffect != null && hit.collider.CompareTag("Map"))
                {
                    Instantiate(projectile.impactPaintEffect, hit.point, Quaternion.identity).GetComponent<PaintEffect>().Build(hit);
                }
            }

            float sa = projectile.spreadAngle;
            rot = Quaternion.AngleAxis(Random.Range(-sa, sa), Vector3.up) *
                  Quaternion.AngleAxis(Random.Range(-sa, sa), Vector3.right) *
                  Quaternion.AngleAxis(Random.Range(-sa, sa), Vector3.forward);
        }
    }
}
