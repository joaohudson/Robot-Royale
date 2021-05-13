using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField]
    private TextMesh text;

    private float size;
    private float duration;
    private Vector3 baseScale;

    // Start is called before the first frame update
    public void Build(int damage, Color color)
    {
        Destroy(gameObject, .5f);
        text.text = damage.ToString();
        text.color = color;
        size = 1f;
        duration = 1f;
        baseScale = text.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * (Time.deltaTime * 5f);
        text.transform.localScale = baseScale * size;
        duration -= Time.deltaTime * 2f;
        size = Mathf.Sqrt(duration);
    }
}
