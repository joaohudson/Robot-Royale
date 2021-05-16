using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponController))]
[RequireComponent(typeof(LauncherController))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AimController))]
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
    [SerializeField]
    private float cameraUpOffset = 1f;

    [Header("Gameplay Settings")]
    [SerializeField]
    private float moveSpeed = 10f;
    [SerializeField]
    private float jumpForce = 15f;
    [SerializeField]
    private float maxUp = 30f;
    [SerializeField]
    private float visionSensibility;
    
    [Header("Player General")]
    [SerializeField]
    private Transform weaponArm;

    private Camera mainCamera;
    private float angleX, angleY;
    private AimController aimController;
    private WeaponController weapon;
    private LauncherController launcher;
    private CharacterController controller;
    private Vector3 velocity = Vector3.zero;//recebe acelerações do jogador
    private bool isGrounded = false;//se está tocando o chão

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        weapon = GetComponent<WeaponController>();
        launcher = GetComponent<LauncherController>();
        controller = GetComponent<CharacterController>();
        aimController = GetComponent<AimController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Menu.Instance.Paused)//checa se o jogo está pausado
            return;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");
        Vector3 movement = new Vector3(x, 0f, z) * moveSpeed;//velocidade de movimento controlada pelo jogador

        //calcula a aceleração da gravidade
        velocity += Physics.gravity * Time.deltaTime;

        //Atira caso solicitado
        if (Input.GetButton("Fire1"))
        {
            weapon.Fire(aimController.Direction);
        }//Se não estiver atirando e nem recarregando, lança granada caso solicitado
        else if (Input.GetKeyDown(KeyCode.Q) && !weapon.Reloading)
        {
            launcher.Launch();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(isGrounded)
                Jump();
        }

        //Recarrega caso solicitado
        if (Input.GetKeyDown(KeyCode.R))
        {
            weapon.Reload();
        }

        angleX += mx * Time.deltaTime * visionSensibility;
        angleY += my * Time.deltaTime * visionSensibility;
        angleY = Mathf.Clamp(angleY, -maxUp, maxUp);

        transform.rotation = Quaternion.AngleAxis(angleX, Vector3.up);
        weaponArm.localRotation = Quaternion.AngleAxis(angleY, Vector3.left);
        controller.Move(transform.rotation * (velocity + movement) * Time.deltaTime);
        isGrounded = controller.isGrounded;//atualiza depois dos movimentos(necessário para evitar bugs)
        //rigidbody.velocity = transform.rotation * new Vector3(x * moveSpeed, rigidbody.velocity.y, z * moveSpeed);
    }

    private void LateUpdate()
    {
        mainCamera.transform.position = transform.position;
        mainCamera.transform.rotation = transform.rotation * weaponArm.localRotation * Quaternion.AngleAxis(cameraInclination, Vector3.right);

        mainCamera.transform.position -= mainCamera.transform.forward * cameraDistance;
        mainCamera.transform.position += mainCamera.transform.up * cameraUpOffset;
    }

    private void Jump()
    {
        velocity.y = jumpForce;
    }
}
