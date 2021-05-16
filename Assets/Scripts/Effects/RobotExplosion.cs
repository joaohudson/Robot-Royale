using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotExplosion : MonoBehaviour
{
    [SerializeField]
    private float force = 10f;
    [SerializeField]
    private float upModifier = 20f;
    [SerializeField]
    private float radius = 4f;
    [SerializeField]
    private float duration = 10f;
    [SerializeField]
    private GameObject effect;
    [SerializeField]
    private Rigidbody[] chunks;

    // Start is called before the first frame update
    void Start()
    {
        if(effect != null)
        {
            Instantiate(effect, transform.position, Quaternion.identity);
        }

        transform.DetachChildren();

        foreach(var c in chunks)
        {
            c.AddExplosionForce(force, transform.position, radius, upModifier);
            Destroy(c.gameObject, duration);
        }

        Destroy(gameObject);
    }
}
