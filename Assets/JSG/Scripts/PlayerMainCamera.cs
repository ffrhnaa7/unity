using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class PlayerMainCamera : MonoBehaviour
{
    public GameObject Pivot;
   
    public float RotationSensitive = 3.0f;
    public float RotationMin = -75;
    public float RotationMax = 75;
    float mouseAxisX;
    float mouseAxisY;
    float smoothTime = 0.12f;
    public float CameraSpeed = 10.0f;
    public float cameraDist = 10;
    Transform pivotTransform;

    Vector3 targetRotation;
    Vector3 currentVel;
    Vector3 offset = new Vector3(0, 2f, 0f);
    private void Awake()
    {
        Pivot = GameObject.FindWithTag("Player");
        Debug.Log($"Camera Target 설정 완료. {Pivot}");
        pivotTransform = Pivot.transform;
    }
    private void Update()
    {
        targetRotation = Vector3.SmoothDamp(targetRotation, targetRotation + new Vector3(-mouseAxisY, mouseAxisX), ref currentVel, smoothTime);
        targetRotation.x = Mathf.Clamp(targetRotation.x, RotationMin, RotationMax); ;
        transform.eulerAngles = targetRotation;

    }
    private void FixedUpdate()
    {
        
        //transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * CameraSpeed);
    }

    private void LateUpdate()
    {
        RaycastHit hit;
        float dist;

        if (Physics.Raycast(transform.position, transform.forward, out hit, cameraDist)) 
        {
            dist = Vector3.Distance(hit.point, pivotTransform.position) * .95f;
        }
        else
        {
            dist = cameraDist;
        }
        Vector3 targetPos = pivotTransform.position + offset - dist * transform.forward;
        transform.position = targetPos;
        //transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref currentVel, smoothTime);
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 inputValue = context.ReadValue<Vector2>();
        Debug.Log($"{inputValue}");
        mouseAxisX = inputValue.x * RotationSensitive;
        mouseAxisY = inputValue.y * RotationSensitive;
        mouseAxisX = Mathf.Clamp(mouseAxisX, RotationMin, RotationMax);
        
        //transform.position -= transform.forward * offset.magnitude;
    }
}
