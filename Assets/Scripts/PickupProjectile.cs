using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupProjectile : MonoBehaviour
{
    [SerializeField]
    private ProjectileInfo projectile;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<WeaponController>().Projectile = projectile;
            Destroy(gameObject);
        }
    }
}
