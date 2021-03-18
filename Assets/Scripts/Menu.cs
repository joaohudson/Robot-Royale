using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    #region Singleton
    public static Menu Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public bool Paused { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Play();
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

    public void Play()
    {
        Time.timeScale = 1f;
        LockMouse(true);
        Paused = false;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
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
