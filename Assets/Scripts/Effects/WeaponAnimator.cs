using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponController))]
[RequireComponent(typeof(Animator))]
public class WeaponAnimator : MonoBehaviour
{
    private WeaponController weapon;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        weapon = GetComponent<WeaponController>();
        animator = GetComponent<Animator>();
        weapon.OnReload += OnReload;
    }

    private void OnReload()
    {
        animator.Play("ReloadWeapon", 0, 0f);
    }
}
