using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public float startingHealth;
    public float health { get; protected set; }
    protected bool dead;

    protected virtual void Start()
    {
        health = startingHealth;
    }
}
