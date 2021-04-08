using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageTextManager : MonoBehaviour
{
    private class DamageText
    {
        public float lifeTime;
        public Text text;
    }

    #region Singleton
    public static DamageTextManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    [SerializeField]
    private Text textPrefab;
    [SerializeField]
    private Transform canvas;

    private List<DamageText> texts = new List<DamageText>();
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < texts.Count; i++)
        {
            var txt = texts[i];
            txt.lifeTime -= Time.deltaTime;
            txt.text.transform.position += Vector3.up * Time.deltaTime * 5f;

            if(txt.lifeTime <= 0f)
            {
                texts.RemoveAt(i);
                Destroy(txt.text.gameObject);
                i--;
            }
        }
    }

    public void AddDamageText(Vector3 position, int value, bool critical)
    {
        DamageText damageText = new DamageText();
        var text = Instantiate(textPrefab, position, Quaternion.identity, canvas).GetComponent<Text>();
        text.color = critical ? Color.yellow : Color.red;
        text.text = value.ToString();

        damageText.text = text;
        damageText.lifeTime = .5f;
        damageText.text.transform.position += Vector3.right * Random.Range(-.5f, .5f);

        texts.Add(damageText);
    }
}
