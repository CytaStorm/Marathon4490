using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField]
    public Transform target; // The target object for the camera to follow
    [SerializeField]
    public float smoothSpeed = 0.125f; // Smooth speed for following

    private void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

        }
    }
}
