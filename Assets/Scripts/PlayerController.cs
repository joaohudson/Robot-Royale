using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponController))]
[RequireComponent(typeof(LauncherController))]
[RequireComponent(typeof(JumpController))]
public class PlayerController : MonoBehaviour
{
    #region Singleton
    public static PlayerController Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    [Header("Camera Settings")]
    [SerializeField]
    private float cameraDistance = 5f;
    [SerializeField]
    private float cameraInclination = 30f;

    [Header("Gameplay Settings")]
    [SerializeField]
    private float moveSpeed = 10f;
    [SerializeField]
    private float maxUp = 30f;
    [SerializeField]
    private float visionSensibility;
    
    [Header("Player General")]
    [SerializeField]
    private Transform weaponArm;

    private Camera mainCamera;
    private float angleX, angleY;
    private WeaponController weapon;
    private LauncherController launcher;
    private JumpController jump;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        weapon = GetComponent<WeaponController>();
        launcher = GetComponent<LauncherController>();
        jump = GetComponent<JumpController>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        //Atira caso solicitado
        if(Input.GetButton("Fire1"))
        {
            weapon.Fire();
        }//Se não estiver atirando e nem recarregando, lança granada caso solicitado
        else if (Input.GetKeyDown(KeyCode.Q) && !weapon.Reloading)
        {
            launcher.Launch();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump.Jump();
        }

        //Recarrega caso solicitado
        if (Input.GetKeyDown(KeyCode.R))
        {
            weapon.Reload();
        }

        angleX += mx * Time.deltaTime * visionSensibility;
        angleY += my * Time.deltaTime * visionSensibility;
        angleY = Mathf.Clamp(angleY, -maxUp, maxUp);

        transform.Translate(x * moveSpeed * Time.deltaTime, 0f, z * moveSpeed * Time.deltaTime, Space.Self);
        transform.rotation = Quaternion.AngleAxis(angleX, Vector3.up);
        weaponArm.localRotation = Quaternion.AngleAxis(angleY, Vector3.left);
    }

    private void LateUpdate()
    {
        mainCamera.transform.position = transform.position;
        mainCamera.transform.rotation = transform.rotation * weaponArm.localRotation * Quaternion.AngleAxis(cameraInclination, Vector3.right);

        mainCamera.transform.position -= mainCamera.transform.forward * cameraDistance;
    }
}
