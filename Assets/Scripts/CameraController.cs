using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target;
    public Vector3 offset;
    public float smoothTime = 0.125f;

    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        if(target != null)
        {
            target = FindObjectOfType<Player>().gameObject;
        }
    }

    void LateUpdate()
    {
        Vector3 desiredPos = target.transform.position + offset;
        Vector3 smoothedPos = Vector3.SmoothDamp(transform.position, desiredPos, ref velocity, smoothTime);
        transform.position = smoothedPos;
        transform.LookAt(target.transform);
    }

}
