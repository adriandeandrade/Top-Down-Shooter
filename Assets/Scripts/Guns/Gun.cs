using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Gun Attributes")]
    [Tooltip("Miliseconds between shots")]
    public float rateOfFire = 100f;
    public float muzzleVelocity = 35f;
    public int bulletsPerMagazine;
    public float reloadTime;
    public enum FireMode { Auto, Single };
    public FireMode fireMode;

    [Space]

    [Header("Other")]
    public Transform muzzle;
    public Projectile projectile;
    
    float nextShotTime;
    [SerializeField]
    int bulletsLeftInMagazine;
    bool isReloading;

    bool triggerReleasedSinceLastShot;

    public float damage = 10f;

    private void LateUpdate()
    {
        if(!isReloading && bulletsLeftInMagazine == 0)
        {
            Reload();
        }
    }

    public void Shoot()
    {
        if(!isReloading && Time.time > nextShotTime && bulletsLeftInMagazine > 0)
        {
            if(fireMode == FireMode.Single)
            {
                if(!triggerReleasedSinceLastShot)
                {
                    return;
                }
            }

            bulletsLeftInMagazine--;
            nextShotTime = Time.time + rateOfFire / 1000;
            Projectile newProjectile = Instantiate(projectile, muzzle.position, muzzle.rotation) as Projectile;
            newProjectile.SetProjectileSpeed(muzzleVelocity);
            Destroy(newProjectile.gameObject, 5f);
        }
    }

    public void Reload()
    {
        if(!isReloading && bulletsLeftInMagazine != bulletsPerMagazine)
        {
            StartCoroutine(ReloadGun());
        }
    }

    IEnumerator ReloadGun()
    {
        isReloading = true;
        yield return new WaitForSeconds(.2f);

        float reloadSpeed = 1f / reloadTime;
        float percent = 0;
        while(percent < 1)
        {
            percent += Time.deltaTime * reloadSpeed;

            yield return null;
        }

        isReloading = false;
        bulletsLeftInMagazine = bulletsPerMagazine;

    }

    public void OnTriggerHold()
    {
        Shoot();
        triggerReleasedSinceLastShot = false;
    }

    public void OnTriggerRelease()
    {
        triggerReleasedSinceLastShot = true;
    }
}
