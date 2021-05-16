using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour
{
    [SerializeField]
    private RectTransform aimTransform;
    [SerializeField]
    private Transform firePoint;
    [SerializeField]
    private float range = 100f;

    private Camera mainCamera;
    private Vector3 direction = Vector3.forward;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        var ray = mainCamera.ScreenPointToRay(aimTransform.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            direction = hit.point - firePoint.position;
            direction.Normalize();
        }
        else
        {
            Vector3 targetPoint = ray.origin + ray.direction * range;
            direction = targetPoint - firePoint.position;
            direction.Normalize();
        }
    }

    /// <summary>
    /// Obtém a direção da mira.
    /// </summary>
    public Vector3 Direction => direction;
}
