using System.Net.Mime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    public float RotationSpeed = 1.0f;
    [SerializeField]
    float defaultSpeed = 15;
    [SerializeField]
    float sprintSpeed = 20;

    Rigidbody body;
    Animator anim;
    Vector3 inputVector;
    Transform cameraTransform;
    float speed;
    private void Awake()
    {
        speed = defaultSpeed;
        body = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        cameraTransform = Camera.main.transform;
    }

    private void FixedUpdate()
    {
        Vector3 moveVec = new Vector3(Vector3.Dot(cameraTransform.forward, inputVector), 0, Vector3.Dot(cameraTransform.right, inputVector));
        if (inputVector != Vector3.zero)
            body.linearVelocity = moveVec * speed;

        if (moveVec != Vector3.zero)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveVec), RotationSpeed * Time.deltaTime);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        inputVector.x = context.ReadValue<Vector2>().y;
        inputVector.z = context.ReadValue<Vector2>().x;
        inputVector.Normalize();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            speed = sprintSpeed;
        }
        else if (context.canceled)
        {
            Debug.Log("Left shitft canceled");
            speed = defaultSpeed;
        }
    }
}
