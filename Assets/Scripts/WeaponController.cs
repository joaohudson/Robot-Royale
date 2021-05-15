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
    private float disableSoundTime = 0f;

    private void Start()
    {
        Ammo = projectile.count;
        if(fireSound != null)
        {
            fireSound.clip = projectile.sound;
            fireSound.loop = projectile.soundLoop;
        }
    }

    private void Update()
    {
        if(interval > 0f)
        {
            interval -= Time.deltaTime;
        }

        //atualiza tempo para desligar o som do projétil.
        if(disableSoundTime > 0f)
        {
            disableSoundTime -= Time.deltaTime;
            if(disableSoundTime <= 0f)
            {
                fireSound.Stop();
            }
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
    public void Fire()
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
            if(!projectile.soundLoop || !fireSound.isPlaying)
            {
                fireSound.Play();
            }
            disableSoundTime = fireSound.clip.length;
        }

        if (projectile.UseRayCast)
        {
            FireRayCast();
        }
        else
        {
            Instantiate(projectile.projectilePrefab, firePoint.position, firePoint.rotation).GetComponent<Projectile>().Build(target);
        }
    }

    private void DoReload()
    {
        //Arma já está recarregada ou já está recarregando
        if (ammo == projectile.count || Reloading)
            return;

        reloadSound.Play();
        reloadTime = ProjectileInfo.RELOAD_TIME;
        OnReload?.Invoke();
    }

    private void FireRayCast()
    {
        RaycastHit hit;
        Quaternion rot = Quaternion.identity;

        for(int i = 0; i < projectile.spreadAmount; i++)
        {
            if (Physics.Raycast(firePoint.position, rot * firePoint.forward, out hit, projectile.range))
            {
                if (hit.collider.CompareTag(target))
                {
                    var state = hit.collider.GetComponent<CharacterState>();
                    state.TakeDamage(projectile.damage, projectile.criticalChance);
                }
                Instantiate(projectile.impactEffectPrefab, hit.point, Quaternion.identity, hit.collider.transform);
            }

            float sa = projectile.spreadAngle;
            rot = Quaternion.AngleAxis(Random.Range(-sa, sa), Vector3.up) *
                  Quaternion.AngleAxis(Random.Range(-sa, sa), Vector3.right) *
                  Quaternion.AngleAxis(Random.Range(-sa, sa), Vector3.forward);
        }
    }
}
