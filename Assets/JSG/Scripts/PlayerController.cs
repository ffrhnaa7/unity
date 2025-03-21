using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameObject Camera;
    public float RotationSpeed = 1.0f;
    [SerializeField]
    float speed;

    Rigidbody body;
    Animator anim;
    Vector3 inputVector;
    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Vector3 moveVec = new Vector3(Vector3.Dot(Camera.transform.forward, inputVector), 0, Vector3.Dot(Camera.transform.right, inputVector));
        body.linearVelocity += moveVec * speed * Time.deltaTime;

        if (moveVec != Vector3.zero)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveVec), RotationSpeed * Time.deltaTime);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
        Debug.Log($"{inputVector}");
    }
}
