using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour
{
    public float maxPickupDistance;
    public float speed;

    GameObject player;
    Rigidbody rBody;

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rBody = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        MoveTowardsPlayer();
    }

    void MoveTowardsPlayer()
    {
        float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
        Vector3 velocity = Vector3.zero;
        if (distanceFromPlayer <= maxPickupDistance)
        {
            transform.position = Vector3.SmoothDamp(transform.position, player.transform.position, ref velocity, speed);
            //transform.Translate(-directionToPlayer * speed, Space.World);
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
