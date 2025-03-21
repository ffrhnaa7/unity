using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class PlayerMainCamera : MonoBehaviour
{
    public GameObject Target;

    public float RotationSensitive = 3.0f;

    float mouseAxisX;
    float mouseAxisY;
    float smoothTime = 0.12f;
    public float CameraSpeed = 10.0f;
    Vector3 targetPos;
    Vector3 targetRotation;
    Vector3 currentVel;
    Vector3 offset = new Vector3(0, 2.35f, -3.75f);
    private void FixedUpdate()
    {
        Vector3 targetPosition = Target.transform.position;
        //targetPos = new Vector3(
        //    targetPosition.x + offsetX,
        //    targetPosition.y + offsetY,
        //    targetPosition.z + offsetZ
        //    );
        targetPos = targetPosition + offset;
        //transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * CameraSpeed);
    }

    private void OnLook(InputValue value)
    {
        Vector2 inputValue = value.Get<Vector2>();
        mouseAxisX = inputValue.x * RotationSensitive;
        mouseAxisY = inputValue.y * RotationSensitive;
        //mouseAxisX = Mathf.Clamp(mouseAxisX, RotationMin, RotationMax);
        targetRotation = Vector3.SmoothDamp(targetRotation, new Vector3(mouseAxisX, mouseAxisY), ref currentVel, smoothTime);
        //transform.position -= transform.forward * offset.magnitude;
    }
}
