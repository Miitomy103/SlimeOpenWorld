using UnityEngine;

public class CameraPivotRotator : MonoBehaviour, IRotatePivot
{
    float yaw;
    float pitch;

    public void RotatePivot(Vector2 lookInput, float sensitivity)
    {
        yaw += lookInput.x * sensitivity * Time.deltaTime;
        pitch -= lookInput.y * sensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -89f, 89f);
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}
