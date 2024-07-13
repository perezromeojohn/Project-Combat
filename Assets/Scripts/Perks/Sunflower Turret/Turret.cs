using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] GameObject turretFirePoint; // The point where the laser originates
    [SerializeField] GameObject sunflowerProjectile; // The projectile that the turret will shoot

    public int projectileCount = 3; // Number of projectiles to spawn
    public float projectileInterval = 3f; // Time between spawning projectiles
    public float turretLifeTime = 10f; // Life time before the turret gets destroyed
    public float damage = 0f; // Damage dealt by the projectile

    void Start()
    {
        // Set initial scale to 0,0,0
        transform.localScale = Vector3.zero;

        // Scale up to 1.5 in all axes over 0.2 seconds
        LeanTween.scale(gameObject, new Vector3(1.5f, 1.5f, 1.5f), 0.2f).setOnComplete(() =>
        {
            // Start the coroutine to spawn projectiles
            StartCoroutine(SpawnProjectiles());

            // Start the coroutine to destroy the turret
            StartCoroutine(DestroyTurret());
        });
    }

    IEnumerator SpawnProjectiles()
    {
        for (int i = 0; i < projectileCount; i++)
        {
            // Instantiate(sunflowerProjectile, turretFirePoint.transform.position, turretFirePoint.transform.rotation);
            GameObject projectile = Instantiate(sunflowerProjectile, turretFirePoint.transform.position, turretFirePoint.transform.rotation);
            projectile.GetComponent<SunflowerProjectile>().damage = damage;

            // Scale up to 1.8 and then back to 1.5 over 0.3 seconds
            LeanTween.scale(gameObject, new Vector3(1.8f, 1.8f, 1.8f), 0.15f).setOnComplete(() =>
            {
                LeanTween.scale(gameObject, new Vector3(1.5f, 1.5f, 1.5f), 0.15f);
            });

            // Wait for the specified interval before spawning the next projectile
            yield return new WaitForSeconds(projectileInterval);
        }
    }

    IEnumerator DestroyTurret()
    {
        // Wait for the specified life time before starting the destruction
        yield return new WaitForSeconds(turretLifeTime);

        // Scale down to 0 in all axes over 0.2 seconds
        LeanTween.scale(gameObject, Vector3.zero, 0.2f).setOnComplete(() =>
        {
            // Destroy the turret after scaling down
            Destroy(gameObject);
        });
    }

    void Update()
    {
        // No need to put anything here for the current requirements
    }
}
