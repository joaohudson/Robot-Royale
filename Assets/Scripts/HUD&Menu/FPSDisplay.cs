using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
    [SerializeField]
    private Text fpsText;

    private float wait = 0f;

    // Update is called once per frame
    void Update()
    {
        if(wait > 0f)
        {
            wait -= Time.deltaTime;
            return;
        }

        fpsText.text = $"FPS: {(int)(1f/Time.deltaTime)}";
        wait = .25f;
    }
}
