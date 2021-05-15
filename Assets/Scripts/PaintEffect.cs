using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintEffect : MonoBehaviour
{
    /// <summary>
    /// Valor a ser somado como vetor normal para evitar sobreposição.
    /// </summary>
    private const float NORMAL_OFFSET = .01f;

    [SerializeField]
    private float duration = 4f;
    [Header("Dispose Effect")]
    [SerializeField]
    private float disposeDuration = 1f;
    [SerializeField]
    private MeshRenderer meshRenderer;
    [SerializeField]
    private MeshColor meshColor;

    /// <summary>
    /// Chamado para contruir o efeito.
    /// </summary>
    /// <param name="target">O alvo a ser pintado.</param>
    public void Build(RaycastHit target)
    {
        var targetTransform = target.transform;
        transform.position += target.normal * NORMAL_OFFSET;
        transform.forward = target.normal;
        
        if(disposeDuration > 0f)
        {
            StartCoroutine(Dispose());
        }
        else
        {
            Destroy(gameObject, duration);
        }
    }

    IEnumerator Dispose()
    {
        float delay = disposeDuration / 20f;
        WaitForSeconds wait = new WaitForSeconds(delay);

        yield return new WaitForSeconds(duration);

        for(int i = 0; i < 20; i++)
        {
            yield return wait;
            var color = meshColor.Color;
            color.a = 1f - i / 20f;
            meshColor.Color = color;
        }

        Destroy(gameObject);
    }
}
