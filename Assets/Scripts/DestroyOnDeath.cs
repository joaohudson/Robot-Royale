using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterState))]
public class DestroyOnDeath : MonoBehaviour
{
    [SerializeField]
    private GameObject destroyedVersion;

    private CharacterState state;
    // Start is called before the first frame update
    void Start()
    {
        state = GetComponent<CharacterState>();

        state.OnChangeHealth += OnChangeHealth;
    }

    private void OnChangeHealth()
    {
        if(state.Health == 0)
        {
            if(destroyedVersion != null)
                Instantiate(destroyedVersion, transform.position, transform.rotation);
            
            Destroy(gameObject);
        }
    }
}
