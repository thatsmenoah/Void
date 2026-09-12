using UnityEngine;

// Эта строчка гарантирует, что Unity автоматически добавит нужный компонент на объект
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float speed = 5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jump = 1.5f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float bhopSpeedBoost = 1.5f;

    [SerializeField] private float gravityMultiplier = 2f; 
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float jumpCd = 0.5f;

    [SerializeField]private float mouseSensitivity = 300f;
    [SerializeField]private Transform playerCamera;
    [SerializeField]private Transform groundCheck;
    [SerializeField]private float groundDistants = 0.4f;
    [SerializeField]private LayerMask groundMask;
    [SerializeField]private bool isGrounded;
    [SerializeField]private float carentSpeed;

    private CharacterController controller;
    private float xRotation = 0f;
    private Vector3 velocity;
    private bool cooldown = true; 

    void Start()
    {

        controller = GetComponent<CharacterController>();
        
        Cursor.lockState = CursorLockMode.Locked;
        carentSpeed = speed;
    }

    void Update()
    {
        // МЫШЬ
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // ВРАЩЕНИЕ КАМЕРЫ
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // ВРАЩЕНИЕ ТЕЛА
        transform.Rotate(Vector3.up * mouseX);

        //Улучшеная проверка земли
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistants, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // WASD
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // КАМЕРА + НАПРАВЛЕНИЕ
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        Debug.Log($"На земле:{isGrounded} Готов прыгать:{cooldown}");

        // ГРАВИТАЦИЯ
        if(Input.GetButtonDown("Jump") && isGrounded && cooldown){
            cooldown = false;
            velocity.y = Mathf.Sqrt(jump * -2f * gravity);
            if(move.magnitude>0.1f){
                // speed += bhopSpeedBoost;
                // speed = Mathf.Clamp(carentSpeed, speed, maxSpeed);
            }
            Invoke(nameof(ResetJump), jumpCd);
        }
        if(velocity.y < 0 && !isGrounded){
            velocity.y += gravity * gravityMultiplier * fallMultiplier * Time.deltaTime;
        }
        else{
            velocity.y += gravity * gravityMultiplier * Time.deltaTime;
        }
        controller.Move(velocity * Time.deltaTime);
    }

    void ResetJump()
    {
        cooldown = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistants);
    }
}