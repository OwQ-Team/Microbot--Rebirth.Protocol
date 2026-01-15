using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 6f;
    public float gravity = 20.0f;

    [Header("Look Settings")]
    public float lookSensitivity = 2f;
    
    public Transform cameraTransform;

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private float xRotation = 0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (PauseMenu.GameIsPaused) return;
        
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limit look angle

        if (cameraTransform != null)
        {
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        transform.Rotate(Vector3.up * mouseX);


        if (characterController.isGrounded)
        {
            float x = Input.GetAxis("Horizontal"); // A - D
            float z = Input.GetAxis("Vertical");   // W - S

            moveDirection = transform.right * x + transform.forward * z;
            moveDirection *= moveSpeed;
        }

        moveDirection.y -= gravity * Time.deltaTime;

        characterController.Move(moveDirection * Time.deltaTime);
    }
}