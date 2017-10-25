using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Entity
{
    public enum State { IDLE, CHASING, ATTACKING };
    State currentState;

    bool hasTarget;

    float myCollisionRadius;
    float targetCollisionRadius;

    NavMeshAgent pathFinder;

    [SerializeField]
    Transform target;

    void Awake()
    {
        if (GameObject.FindObjectOfType<PlayerMotor>() != null)
        {
            target = GameObject.FindObjectOfType<PlayerMotor>().transform;
            hasTarget = true;
            
        }

        pathFinder = GetComponent<NavMeshAgent>();
        pathFinder.speed = 3f;

        myCollisionRadius = GetComponent<CapsuleCollider>().radius;
        targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;
    }

    protected override void Start()
    {
        base.Start();

        if(hasTarget)
        {
            currentState = State.CHASING;
            StartCoroutine(UpdatePath());
        }
    }

    public void SetCharacteristics(float moveSpeed)
    {
        pathFinder.speed = moveSpeed;
    }

    IEnumerator UpdatePath()
    {
        print(currentState);

        float refreshRate = .25f;
        while (hasTarget)
        {
            if (currentState == State.CHASING)
            {
                Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
                Vector3 targetPosition = target.transform.position - directionToTarget * (myCollisionRadius + targetCollisionRadius);

                pathFinder.SetDestination(targetPosition);
            }

            yield return new WaitForSeconds(refreshRate);
        }
    }
}
