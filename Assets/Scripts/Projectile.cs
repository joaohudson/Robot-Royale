using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private ProjectileInfo info;
    [SerializeField]
    private float moveSpeed = 60f;
    [SerializeField]
    private float size = 1f;
    [SerializeField]
    private float explosionRadius = 0f;

    private float distance = 0f;
    private string target;

    public void Build(string target)
    {
        this.target = target;
    }

    // Update is called once per frame
    void Update()
    {
        float d = Time.deltaTime * moveSpeed;
        transform.position += d * transform.forward;

        distance += d;

        if(distance >= info.range)
        {
            Die();
        }
    }

    private void FixedUpdate()
    {
        var cds = Physics.OverlapSphere(transform.position, size);

        foreach (var cd in cds)
        {
            if (cd.CompareTag(target))
            {
                if(explosionRadius == 0f)
                {
                    var state = cd.GetComponent<CharacterState>();
                    state.TakeDamage(info.damage, info.criticalChance);
                }
                break;
            }
            Die();
        }
    }

    private void Die()
    {
        if(explosionRadius == 0f)
        {
            Destroy(gameObject);
            Instantiate(info.impactEffectPrefab, transform.position, Quaternion.identity);
            return;
        }

        var cds = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach(var cd in cds)
        {
            if (cd.CompareTag(target))
            {
                var state = cd.GetComponent<CharacterState>();
                state.TakeDamage(info.damage, info.criticalChance);
            }
        }

        Instantiate(info.impactEffectPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
