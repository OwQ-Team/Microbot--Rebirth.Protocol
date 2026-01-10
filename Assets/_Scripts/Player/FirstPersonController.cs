using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 6f;
    public float gravity = 20.0f;

    [Header("Look Settings")]
    public float lookSensitivity = 2f;
    
    [Tooltip("Drag your Main Camera here")]
    public Transform cameraTransform;

    // Private variables
    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private float xRotation = 0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // --- 1. Mouse Look Logic ---
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

        // Handle Up/Down look (Pitch)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limit look angle

        if (cameraTransform != null)
        {
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        // Handle Left/Right look (Yaw) - Rotates the entire player body
        transform.Rotate(Vector3.up * mouseX);


        // --- 2. Movement Logic ---
        if (characterController.isGrounded)
        {
            float x = Input.GetAxis("Horizontal"); // A - D
            float z = Input.GetAxis("Vertical");   // W - S

            // Calculate movement direction relative to where we are looking
            moveDirection = transform.right * x + transform.forward * z;
            moveDirection *= moveSpeed;
        }

        // Apply Gravity
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
    }
}