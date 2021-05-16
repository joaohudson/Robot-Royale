using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemieManager : MonoBehaviour
{
    #region Singleton
    public static EnemieManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    [SerializeField]
    private int enemieCount = 10;

    /// <summary>
    /// Chamado quando todos os inimigos forem mortos.
    /// </summary>
    public Action OnClearEnemies;

    /// <summary>
    /// Chamado por um inimigo para registrar sua morte.
    /// </summary>
    public void RegisterDeath()
    {
        enemieCount--;
        if(enemieCount == 0)
            OnClearEnemies?.Invoke();
    }
}
