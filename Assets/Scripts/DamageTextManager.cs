using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageTextManager : MonoBehaviour
{

    #region Singleton
    public static DamageTextManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    [SerializeField]
    private DamageText damageText;
    [SerializeField]
    private Transform canvas;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    public void AddDamageText(Vector3 position, int value, bool critical)
    {
        var obj = Instantiate(damageText, position + Vector3.right * Random.Range(-.5f, .5f), Quaternion.identity, canvas);
        obj.GetComponent<DamageText>().Build(value, critical ? Color.yellow : Color.red);
    }
}
