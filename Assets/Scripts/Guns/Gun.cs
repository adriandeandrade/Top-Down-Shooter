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
    public int damage;
    public enum FireMode { Auto, Single };
    public FireMode fireMode;

    [Space]

    [Header("Other")]
    public Transform[] muzzles;
    public Projectile projectile;

    float nextShotTime;
    [SerializeField]
    int bulletsLeftInMagazine;
    bool isReloading;

    bool triggerReleasedSinceLastShot;

    private void LateUpdate()
    {
        if (!isReloading && bulletsLeftInMagazine == 0)
        {
            Reload();
        }
    }

    public void Shoot()
    {
        if (!isReloading && Time.time > nextShotTime && bulletsLeftInMagazine > 0)
        {
            if (fireMode == FireMode.Single)
            {
                if (!triggerReleasedSinceLastShot)
                {
                    return;
                }
            }

            for (int i = 0; i < muzzles.Length; i++)
            {
                if (bulletsLeftInMagazine == 0)
                {
                    break;
                }

                bulletsLeftInMagazine--;
                nextShotTime = Time.time + rateOfFire / 1000;
                Projectile newProjectile = Instantiate(projectile, muzzles[i].position, muzzles[i].rotation) as Projectile;
                newProjectile.SetProjectileSpeed(muzzleVelocity);
                newProjectile.SetProjectileDamage(damage);
                Destroy(newProjectile.gameObject, 5f);
            }
        }
    }

    public void Reload()
    {
        if (!isReloading && bulletsLeftInMagazine != bulletsPerMagazine)
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
        while (percent < 1)
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
