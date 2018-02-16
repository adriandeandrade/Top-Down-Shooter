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
    bool hasTarget;

    [SerializeField]
    GameObject target;

    NavMeshAgent pathFinder;

    private void Awake()
    {
        pathFinder = GetComponent<NavMeshAgent>();
    }

    protected override void Start()
    {
        base.Start();
        SetEnemyCharacteristics();
        currentState = State.IDLE;
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

    private void Update()
    {
        if(HasTarget())
        {
            StartCoroutine(UpdatePath());
        }
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

    public void FindTarget()
    {
        if (!hasTarget)
        {
            target = FindObjectOfType<PlayerController>().gameObject;
        }
        else
        {
            target = null;
        }
    }

    IEnumerator UpdatePath()
    {
        float refreshRate = 1f;
        if(HasTarget())
        {
            pathFinder.SetDestination(target.transform.position);
        }
        yield return new WaitForSeconds(refreshRate);
    }
}
