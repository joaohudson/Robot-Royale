using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterState))]
public class DamageScreenEffect : MonoBehaviour
{
    [SerializeField]
    private Image damageScreen;

    private float duration;

    // Start is called before the first frame update
    void Start()
    {
        var cs = GetComponent<CharacterState>();
        cs.OnDamage += OnDamage;
        enabled = false;
    }

    private void OnDamage(DamageEvent damage)
    {
        enabled = true;
        duration = 1f;
        var color = damageScreen.color;
        color.a = 1f;
        damageScreen.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        duration -= Time.deltaTime;
        
        var color = damageScreen.color;
        color.a = duration * .4f;
        damageScreen.color = color;

        if(duration <= 0f)
        {
            enabled = false;
        }
    }
}
