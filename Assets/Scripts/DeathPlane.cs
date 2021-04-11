using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var state = other.GetComponent<CharacterState>();

        if(state != null)
        {
            state.Kill();
        }
    }
}
