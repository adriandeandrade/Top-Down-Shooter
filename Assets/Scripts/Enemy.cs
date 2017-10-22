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

    bool detected;

    float myCollisionRadius;
    float targetCollisionRadius;
    [SerializeField]
    float maxDetectionRange;
    [SerializeField]
    float loseSightDistance;

    NavMeshAgent pathFinder;

    [SerializeField]
    GameObject target;

    void Awake()
    {
        pathFinder = GetComponent<NavMeshAgent>();
        pathFinder.speed = 3f;

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            target = GameObject.FindGameObjectWithTag("Player").gameObject;
            StartCoroutine(UpdatePath());
        }

        myCollisionRadius = GetComponent<CapsuleCollider>().radius;
        targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;
        maxDetectionRange = 30f;

    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(UpdatePath());
    }

    void Update()
    {
        if (!detected && IsWithinDetectionRange())
        {
            StartCoroutine(UpdatePath());
        }
    }

    bool IsWithinDetectionRange()
    {
        Vector3 directionToTarget = target.transform.position - transform.position;

        if (directionToTarget.sqrMagnitude <= maxDetectionRange)
        {
            detected = true;
            currentState = State.CHASING;

            return true;
        }
        else if (directionToTarget.sqrMagnitude < loseSightDistance)
        {
            detected = false;
            currentState = State.IDLE;
        }

        return false;
    }

    public void SetCharacteristics(float moveSpeed)
    {
        pathFinder.speed = moveSpeed;
    }

    IEnumerator UpdatePath()
    {
        float refreshRate = .25f;
        while (detected)
        {
            if (currentState == State.CHASING)
            {
                Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
                Vector3 targetPosition = target.transform.position - directionToTarget * (myCollisionRadius + targetCollisionRadius);

                if (!dead)
                {
                    pathFinder.SetDestination(targetPosition);
                }
            }
            else
            {
                currentState = State.IDLE;
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
