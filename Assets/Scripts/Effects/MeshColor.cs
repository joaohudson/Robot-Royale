using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class MeshColor : MonoBehaviour
{
    [SerializeField]
    private Color color;

    private MaterialPropertyBlock propertie;
    private MeshRenderer meshRenderer;

    private void OnValidate()
    {
        Start();
        Update();
    }

    void Start()
    {
        propertie = new MaterialPropertyBlock();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        propertie.SetColor("_Color", color);
        meshRenderer.SetPropertyBlock(propertie);
    }

    /// <summary>
    /// Cor desta malha.
    /// </summary>
    public Color Color
    {
        get => color;
        set => color = value;
    }
}
