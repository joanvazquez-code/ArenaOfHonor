using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 6f;
    public float runSpeed = 10f;
    public float gravity = -35f;
    public float jumpHeight = 1.5f;

    [Header("Mouse Settings")]
    [Range(10f, 500f)]
    public float mouseSensitivity = 150f;
    public Transform playerCamera;

    [Header("Player Stats")]
    public int vida = 5;  // Número de vidas o puntos de vida del jugador
    public bool estaMuerto = false;

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation = 0f;
    private int jumpCount = 0;
    private int maxJumps = 2;
    private bool isGrounded;
    private float groundCheckDistance = 0.3f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (estaMuerto) return; // Si está muerto, no puede moverse ni girar

        HandleMovement();
        HandleMouseLook();
    }

    // --- MOVIMIENTO, CORRER Y SALTO ---
    void HandleMovement()
    {
        isGrounded = controller.isGrounded || Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            jumpCount = 0;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * currentSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpCount++;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // --- CÁMARA FPS ---
    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -85f, 85f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    // --- AJUSTE DE SENSIBILIDAD ---
    public void SetMouseSensitivity(float newSensitivity)
    {
        mouseSensitivity = newSensitivity;
    }

    // --- RECIBIR DAÑO ---
    public void RecibirDaño(int cantidad)
    {
        if (estaMuerto) return;

        vida -= cantidad;
        Debug.Log("Jugador recibió daño. Vida restante: " + vida);

        if (vida <= 0)
        {
            Morir();
        }
    }

    // --- MUERTE DEL JUGADOR ---
    void Morir()
    {
        estaMuerto = true;
        Debug.Log("El jugador ha muerto.");
        // Aquí puedes añadir animaciones, reinicio de escena, etc.
    }

    // --- EJEMPLO DE DETECCIÓN DE GOLPE ---
    // Esto se ejecuta cuando algo con collider y trigger toca al jugador.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemigo"))
        {
            RecibirDaño(1); // Le quita 1 de vida si toca un objeto con tag "Enemigo"
        }
    }
}
