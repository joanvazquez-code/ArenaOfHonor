using UnityEngine;

public class Controller : MonoBehaviour
{
    public bool canMove = true;

    [Header("Movimiento")]
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float runSpeed = 8f;
    [SerializeField] float changeSpeed = 5f;

    [Header("Agacharse")]
    [SerializeField] Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    Vector3 originalScale;
    [SerializeField] float crouchSpeed = 2f;
    [SerializeField] float crouchTransitionSpeed = 5f;

    [Header("Salto")]
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] int maxJumps = 1;

    [Header("Raton camara")]
    [Range(0.1f, 100f)]
    [SerializeField] float mouseSensitivity = 100f;
    [SerializeField] float horizontalSpeed = 1f;
    [SerializeField] float verticalSpeed = 1f;
    [SerializeField] float xMinLimit = -80f;
    [SerializeField] float xMaxLimit = 80f;

    [Header("Fisicas")]
    [SerializeField] float gravity = -9.81f;
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform roofCheck;
    [SerializeField] float checkerRadius = 0.3f;
    [SerializeField] LayerMask groundMask;

    float moveSpeed;
    float xRot;
    Camera cam;
    CharacterController cc;
    Vector3 velocidad;
    bool isGrounded;
    bool isCrouching;
    bool isUnderSmth;
    bool isRunning;
    int jumpCount = 0;

    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        originalScale = transform.localScale;
        moveSpeed = walkSpeed;
        cc = GetComponent<CharacterController>();
        ChangeCursorVisibility(true);
    }

    void Update()
    {
        if (canMove)
        {
            MouseLooking();
            PlayerMovement();
        }
        PlayerPhysics();
    }

    private void PlayerMovement()
    {
        InputKeysLogic();
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movimiento = transform.right * x + transform.forward * z;
        cc.Move(movimiento * moveSpeed * Time.deltaTime);
    }

    private void PlayerPhysics()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, checkerRadius, groundMask);
        isUnderSmth = Physics.CheckSphere(roofCheck.position, checkerRadius, groundMask);

        if (isGrounded && velocidad.y < 0)
        {
            velocidad.y = -2f;
            cc.slopeLimit = 45f;
            jumpCount = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps)
        {
            jumpCount++;
            cc.slopeLimit = 90f;
            velocidad.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocidad.y += gravity * Time.deltaTime;
        cc.Move(velocidad * Time.deltaTime);
    }

    private void MouseLooking()
    {
        float h = horizontalSpeed * Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float v = verticalSpeed * Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * h);
        xRot -= v;
        xRot = Mathf.Clamp(xRot, xMinLimit, xMaxLimit);
        cam.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
    }

    private void InputKeysLogic()
    {
        // Correr
        isRunning = Input.GetKey(KeyCode.LeftShift);
        if (isRunning && !isCrouching)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, runSpeed, Time.deltaTime * changeSpeed);
        }
        else if (!isCrouching)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, Time.deltaTime * changeSpeed);
        }

        // Agacharse
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            StartCrouch();
        }

        if (isCrouching)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, crouchScale, Time.deltaTime * crouchTransitionSpeed);
        }
        else if (!isCrouching && transform.localScale != originalScale)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * crouchTransitionSpeed);
        }

        if (Input.GetKeyUp(KeyCode.LeftControl) && !isUnderSmth)
        {
            StopCrouch();
        }
    }

    private void StartCrouch()
    {
        isCrouching = true;
        moveSpeed = crouchSpeed;
    }

    private void StopCrouch()
    {
        isCrouching = false;
        moveSpeed = walkSpeed;
    }

    private void ChangeCursorVisibility(bool active)
    {
        Cursor.lockState = active ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !active;
    }

    private void OnDrawGizmos()
    {
        if (groundCheck == null || roofCheck == null) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheck.position, checkerRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(roofCheck.position, checkerRadius);
    }
}
