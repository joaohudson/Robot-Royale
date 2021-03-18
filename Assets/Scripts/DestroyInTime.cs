using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInTime : MonoBehaviour
{
    [SerializeField]
    private float time;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, time);    
    }
}
