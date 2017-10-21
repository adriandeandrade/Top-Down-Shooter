using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : Entity
{
    PlayerController playerController;
    Camera viewCamera;

    public float moveSpeed = 5;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        viewCamera = Camera.main;

    }

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        /* MOVE INPUT */
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;
        playerController.Move(moveVelocity);

        /* LOOK INPUT */
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        Plane floor = new Plane(Vector3.up, Vector3.up * 3);
        float rayDistance;

        if(floor.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            Debug.DrawLine(ray.origin, point, Color.red);
            playerController.LookAt(point);
        }

    }
}
