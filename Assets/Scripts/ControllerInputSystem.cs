using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class ControllerInputSystem : MonoBehaviour
{
    [Header("Movimiento")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float changeSpeed = 5f;

    [Header("Cámara")]
    public float sensitivity = 1f;
    public float xMin = -80f;
    public float xMax = 80f;

    [Header("Salto")]
    public float jumpHeight = 3f;
    public float gravity = -9.81f;
    public int maxJumps = 1;

    [Header("Ground check")]
    public Transform groundCheck;
    public float checkRadius = 0.3f;
    public LayerMask groundMask;

    private CharacterController cc;
    private Camera cam;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;
    private float xRot;
    private int jumpCount;
    private bool isGrounded;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        cam = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        MovePlayer();
        ApplyGravity();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>() * sensitivity;
        transform.Rotate(Vector3.up * lookInput.x * Time.deltaTime);
        xRot -= lookInput.y * Time.deltaTime;
        xRot = Mathf.Clamp(xRot, xMin, xMax);
        cam.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && jumpCount < maxJumps)
        {
            jumpCount++;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void MovePlayer()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        cc.Move(move * walkSpeed * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, checkRadius, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            jumpCount = 0;
        }

        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);
    }
}
