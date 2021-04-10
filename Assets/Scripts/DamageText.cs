using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    [SerializeField]
    private Text text;

    // Start is called before the first frame update
    public void Build(int damage, Color color)
    {
        Destroy(gameObject, .5f);
        text.text = damage.ToString();
        text.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * (Time.deltaTime * 5f);
    }
}
