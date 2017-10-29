using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToPlayer : MonoBehaviour
{
    public float teleportChance = 0f;
    public float teleportCooldown = 0.5f;

    float teleportTimeLeft = 0;

    Enemy enemy;

    void Start()
    {
        enemy = GetComponent<Enemy>();
        teleportCooldown *= Random.Range(1f, 1.5f);
        teleportTimeLeft = Random.Range(teleportCooldown / 2f, teleportCooldown);
    }

    void Update()
    {
        teleportTimeLeft -= Time.deltaTime;

        if(enemy.HasTarget())
        {
            if(enemy.IsWithinAttackRange())
            {
                if(teleportTimeLeft <= 0)
                {
                    teleportTimeLeft = teleportCooldown;

                    if(Random.Range(0f, 1f) < teleportChance)
                    {
                        Vector3 offset = Random.onUnitSphere;
                        offset.y = 0;
                        offset.Normalize();
                        Vector3 teleportPoint = enemy.GetTargetCurrentPosition() + offset * 2f;

                        if(Physics.CheckSphere(teleportPoint + Vector3.up * 1.1f, 1.0f) == false)
                        {
                            transform.position = teleportPoint;
                        }
                    }
                }
            }
        }
    }
}
