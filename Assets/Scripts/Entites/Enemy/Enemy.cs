using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Entity
{
    [Header("Characteristics")]
    public int speed;
    public int damage;
    public float maxAttackRange;
    public enum State { IDLE, CHASING, ATTACKING };

    State currentState;
    float refreshRate = 0.25f;
    bool hasTarget;

    NavMeshAgent pathFinder;
    [SerializeField] GameObject target;

    protected override void Start()
    {
        base.Start();
        pathFinder = GetComponent<NavMeshAgent>();
        SetEnemyCharacteristics();

        if (target != null)
        {
            currentState = State.CHASING;
            StartCoroutine(UpdatePath());
            hasTarget = true;
        }
    }

    public void SetEnemyCharacteristics()
    {
        pathFinder.speed = speed;
    }

    public bool HasTarget()
    {
        return target != null;
    }

    public Vector3 GetTargetCurrentPosition()
    {
        if (hasTarget)
        {
            return target.transform.position;
        }

        return Vector3.zero;
    }

    float GetDistanceToPlayer()
    {
        return Vector3.Distance(transform.position, target.transform.position);
    }

    public bool IsWithinAttackRange()
    {
        if (!hasTarget)
        {
            return false;
        }

        if (GetDistanceToPlayer() >= maxAttackRange)
        {
            return true;
        }

        return false;
    }

    IEnumerator UpdatePath()
    {
        while (target != null)
        {
            if (currentState == State.CHASING)
            {
                Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
                Vector3 targetPosition = target.transform.position - directionToTarget;
                if (!dead)
                {
                    pathFinder.SetDestination(targetPosition);
                }
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
