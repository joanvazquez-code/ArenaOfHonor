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
    public float maxVida = 100f;
    public float vidaActual;
    public bool estaMuerto = false;

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation = 0f;
    private int jumpCount = 0;
    private int maxJumps = 2;
    private bool isGrounded;
    private float groundCheckDistance = 0.3f;

    private BarraDeVida barraDeVida;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        vidaActual = maxVida;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        barraDeVida = FindFirstObjectByType<BarraDeVida>();

        if (barraDeVida != null)
            barraDeVida.ActualizarBarra();
    }

    void Update()
    {
        if (estaMuerto) return;
        HandleMovement();
        HandleMouseLook();
    }

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

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -85f, 85f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    // Daño gradual
    public void RecibirDañoGradual(float cantidad)
    {
        if (estaMuerto) return;

        vidaActual -= cantidad;
        vidaActual = Mathf.Max(vidaActual, 0);

        if (barraDeVida != null)
            barraDeVida.ActualizarBarra();

        if (vidaActual <= 0)
            Morir();
    }

    public void RecibirDaño(int cantidad)
    {
        if (estaMuerto) return;

        vidaActual -= cantidad;
        vidaActual = Mathf.Max(vidaActual, 0);

        if (barraDeVida != null)
            barraDeVida.ActualizarBarra();

        if (vidaActual <= 0)
            Morir();
    }
    public bool tieneArma = false;

    public void PickupWeapon()
    {
        tieneArma = true;
        Debug.Log("¡Arma recogida!");
        // Aquí puedes mostrar el arma en pantalla, habilitar su modelo, activar UI, etc.
    }

    void Morir()
    {
        estaMuerto = true;
        vidaActual = 0;
        Debug.Log("El jugador ha muerto.");
        if (barraDeVida != null)
            barraDeVida.ActualizarBarra();
    }
}
