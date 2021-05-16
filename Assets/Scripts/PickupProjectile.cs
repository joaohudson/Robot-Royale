using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupProjectile : MonoBehaviour
{
    [SerializeField]
    private ProjectileInfo projectile;
    [SerializeField]
    private AudioClip pickupSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<WeaponController>().Projectile = projectile;
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            Destroy(gameObject);
        }
    }
}
