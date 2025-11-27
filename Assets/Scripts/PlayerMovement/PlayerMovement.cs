using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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
    public bool tieneArma = false;
    public GameObject espada;
    public Transform puntoSujecionArma; // Crea un GameObject hijo vacío para esto

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

        // REINICIAR ESTADO AL CARGAR LA ESCENA
        ReiniciarEstado();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        barraDeVida = FindFirstObjectByType<BarraDeVida>();

        if (barraDeVida != null)
            barraDeVida.ActualizarBarra();
        
        espada.SetActive(false);
    }

    // MÉTODO PARA REINICIAR EL ESTADO DEL JUGADOR
    public void ReiniciarEstado()
    {
        vidaActual = maxVida;
        estaMuerto = false;
        tieneArma = false; // Reiniciar también el estado del arma
        velocity = Vector3.zero;
        jumpCount = 0;

        Debug.Log("✅ Estado del jugador reiniciado. Vida: " + vidaActual);
    }

    void Update()
    {
        if (estaMuerto)
        {
            if (Time.frameCount % 60 == 0)
                Debug.Log("Personaje MUERTO esperando cambio de escena...");
            return;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("🔥 TEST: Forzando muerte inmediata");
            vidaActual = 0;
            Morir();
        }

        HandleMovement();
        HandleMouseLook();
        if(tieneArma)
            espada.SetActive(true);
        
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

    public void PickupWeapon()
    {
        tieneArma = true;
        Debug.Log("¡Arma recogida!");
    }

    public void RecogerArma(GameObject arma)
    {
        //if (tieneArma) return;

        /*Debug.Log("🎯 Posicionando arma...");

        tieneArma = true;

        // 1. ELIMINAR FÍSICA inmediatamente
        Rigidbody rb = arma.GetComponent<Rigidbody>();
        if (rb != null)
        {
            DestroyImmediate(rb);
            Debug.Log("✅ Rigidbody eliminado");
        }

        Collider collider = arma.GetComponent<Collider>();
        if (collider != null)
        {
            DestroyImmediate(collider);
            Debug.Log("✅ Collider eliminado");
        }

        // 2. HACER HIJO de la mano
        arma.transform.SetParent(puntoSujecionArma);

        // 3. POSICIONAR EXACTAMENTE
        arma.transform.localPosition = Vector3.zero;
        arma.transform.localRotation = Quaternion.identity;
        arma.transform.localScale = Vector3.one;

        // 4. DEBUG para verificar
        Debug.Log($"📍 ¿Arma activa? {arma.activeInHierarchy}");
        Debug.Log($"📍 ¿Tiene parent? {arma.transform.parent != null}");
        Debug.Log($"📍 Parent: {arma.transform.parent?.name}");
        Debug.Log("✅ Arma debería estar en la mano");*/
    }

    void Morir()
    {
        if (estaMuerto) return;

        estaMuerto = true;
        vidaActual = 0;
        Debug.Log("🔥 El jugador ha muerto - Iniciando cambio de escena");

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        StartCoroutine(CambiarEscenaConDelay());
    }

    System.Collections.IEnumerator CambiarEscenaConDelay()
    {
        Debug.Log("⏳ Esperando 1.5 segundos antes de cambiar escena...");
        yield return new WaitForSeconds(1.5f);
        Debug.Log("✅ Delay completado, cambiando a menú");
        CambiarAMenu();
    }

    void CambiarAMenu()
    {
        // Cargar el menú principal
        SceneManager.LoadScene("Menu");
    }

    // Método público para reiniciar desde el menú
    public void IniciarPartida()
    {
        ReiniciarEstado();
        SceneManager.LoadScene("Partida");
    }
}