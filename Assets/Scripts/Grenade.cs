using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField]
    private float explosionTime = 3f;
    [SerializeField]
    private float explosionRadius = 10f;
    [SerializeField]
    private int damage = 80;
    [SerializeField]
    private GameObject effect;

    void Start()
    {
        StartCoroutine(Explode());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
    IEnumerator Explode()
    {
        yield return new WaitForSeconds(explosionTime);
        Instantiate(effect, transform.position, Quaternion.identity);
        var cds = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach(var cd in cds)
        {
            if (cd.CompareTag("Enemie"))
            {
                var state = cd.GetComponent<CharacterState>();
                var nv = cd.GetComponent<NavMeshAgent>();
                state.TakeDamage(damage, 0f);
                nv.velocity = (nv.transform.position - transform.position).normalized * 20f;
            }
        }

        Destroy(gameObject);
    }
}
