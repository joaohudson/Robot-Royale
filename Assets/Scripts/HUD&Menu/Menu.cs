using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    #region Singleton
    public static Menu Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    [SerializeField]
    private GameObject loseMenu;
    [SerializeField]
    private GameObject winMenu;
    [SerializeField]
    private GameObject aim;

    public bool Paused { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Play();
        var player = PlayerController.Instance.GetComponent<CharacterState>();
        player.OnDeath += OnDeath;

        var enemieManager = EnemieManager.Instance;
        enemieManager.OnClearEnemies += OnClearEnemies;
    }

    private void OnClearEnemies()
    {
        Pause();
        winMenu.SetActive(true);
    }

    private void OnDeath()
    {
        Pause();
        loseMenu.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (Paused)
            {
                Play();
            }
            else
            {
                Pause();
            }
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Play()
    {
        Time.timeScale = 1f;
        aim.SetActive(true);
        LockMouse(true);
        Paused = false;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        aim.SetActive(false);
        LockMouse(false);
        Paused = true;
    }

    private void LockMouse(bool lockMouse)
    {
        if (lockMouse)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
