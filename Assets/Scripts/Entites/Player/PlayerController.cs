using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(GunController))]
public class PlayerController : Entity
{
    PlayerMotor playerMotor;
    Camera viewCamera;
    GunController gunController;

    public float moveSpeed = 5;

    void Awake()
    {
        playerMotor = GetComponent<PlayerMotor>();
        gunController = GetComponent<GunController>();
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
        playerMotor.Move(moveVelocity);

        /* LOOK INPUT */
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        Plane floor = new Plane(Vector3.up, Vector3.up * 3);
        float rayDistance;

        if (floor.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            Debug.DrawLine(ray.origin, point, Color.red);
            playerMotor.LookAt(point);
        }

        //WEAPON INPUT
        if (Input.GetMouseButton(0))
        {
            gunController.OnTriggerHold();
        }

        if (Input.GetMouseButtonUp(0))
        {
            gunController.OnTriggerRelease();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            gunController.Reload();
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            gunController.SelectPistol();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            gunController.SelectShotgun();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            gunController.SelectRifle();
        }
    }
}
