using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public LayerMask collisionMask;

    float speed = 10f;
    float damage = 1;


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position, .1f);
    }

    public void SetProjectileSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void SetProjectileDamage(float newDamage)
    {
        damage = newDamage;
    }

    private void Update()
    {
        float moveDistance = speed * Time.deltaTime;
        CheckCollisions(moveDistance);

        // Bullet movement
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    void CheckCollisions(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, moveDistance, collisionMask, QueryTriggerInteraction.Collide))
        {
            OnHitDamageableObject(hit);
        }
    }

    void OnHitDamageableObject(RaycastHit hit)
    {
        IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();
        if(damageableObject != null)
        {
            damageableObject.TakeHit(damage, hit);
        }
        GameObject.Destroy(gameObject);
    }
}
