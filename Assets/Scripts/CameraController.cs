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
        
    }

    void LateUpdate()
    {
        if(target == null)
        {
            if (GameManager.instance.playerPrefabInstance != null) target = GameManager.instance.transform.gameObject;
            if(target == null)
            {
                return;
            }
        }

        Vector3 desiredPos = target.transform.position + offset;
        Vector3 smoothedPos = Vector3.SmoothDamp(transform.position, desiredPos, ref velocity, smoothTime);
        transform.position = smoothedPos;
        transform.LookAt(target.transform);
    }

}
