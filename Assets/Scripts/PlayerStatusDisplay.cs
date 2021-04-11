using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusDisplay : MonoBehaviour
{
    [SerializeField]
    private Image healththBar;
    [SerializeField]
    private Text healthText;
    [SerializeField]
    private Text ammoText;

    private CharacterState playerState;
    private WeaponController playerWeapon;

    // Start is called before the first frame update
    void Start()
    {
        playerState = PlayerController.Instance.GetComponent<CharacterState>();
        playerWeapon = PlayerController.Instance.GetComponent<WeaponController>();

        playerState.OnChangeHealth += OnChangeHealth;
        playerWeapon.OnChangeAmmo += OnChangeAmmo;
    }

    private void OnChangeHealth()
    {
        healththBar.fillAmount = playerState.HealthNormalized;
        healthText.text = $"{playerState.HealthNormalized * 100f}%";
    }

    private void OnChangeAmmo()
    {
        ammoText.text = $"Ammo: {playerWeapon.Ammo}/\u221e";
    }
}
