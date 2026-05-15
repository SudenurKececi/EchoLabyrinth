using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.15f;
    public Vector3 offset = new Vector3(0, 0, -10);

    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desired = target.position + offset;
        desired.z = offset.z; // kameranýn z'si sabit kalsýn

        transform.position = Vector3.SmoothDamp(transform.position, desired, ref velocity, smoothTime);
    }
}