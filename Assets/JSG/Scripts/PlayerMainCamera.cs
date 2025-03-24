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
    public float DefaultCameraDist = 4;
    Transform pivotTransform;

    Vector3 targetRotation;
    Vector3 currentVel;
    Vector3 offset = new Vector3(0, 2f, 0f);
    float cameraDist = 4;
    private void Awake()
    {
        Pivot = GameObject.FindWithTag("Player");
        Debug.Log($"Camera Target 설정 완료. {Pivot}");
        pivotTransform = Pivot.transform;
    }
    private void Update()
    {
        targetRotation = Vector3.SmoothDamp(targetRotation, targetRotation + new Vector3(-mouseAxisY, mouseAxisX), ref currentVel, smoothTime);
        targetRotation.x = Mathf.Clamp(targetRotation.x, RotationMin, RotationMax);
        transform.eulerAngles = targetRotation;
    }
    private void FixedUpdate()
    {
        
    }

    private void LateUpdate()
    {
        SetDistSpringArm();
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 inputValue = context.ReadValue<Vector2>();
        mouseAxisX = inputValue.x * RotationSensitive;
        mouseAxisY = inputValue.y * RotationSensitive;
        mouseAxisX = Mathf.Clamp(mouseAxisX, RotationMin, RotationMax);
    }

    private void SetDistSpringArm()
    {
        RaycastHit hit;
        float dist;
        Vector3 rayOrigin = pivotTransform.position + offset;

        if (Physics.Raycast(rayOrigin, -transform.forward, out hit, DefaultCameraDist))
        {
            float targetDist = Vector3.Distance(hit.point, rayOrigin) * 0.8f;
            cameraDist = Mathf.Lerp(cameraDist, targetDist, Time.deltaTime * 10);
            Debug.DrawRay(rayOrigin, -transform.forward * cameraDist, Color.red);
        }
        else
        {
            cameraDist = Mathf.Lerp(cameraDist, DefaultCameraDist, Time.deltaTime * 10);
            Debug.DrawRay(rayOrigin, -transform.forward * DefaultCameraDist, Color.green);
        }

        Vector3 targetPos = pivotTransform.position + offset - cameraDist * transform.forward;
        transform.position = targetPos;
    }
}
