using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonSmog : MonoBehaviour
{
    [SerializeField] ParticleSystem poisonCloud;
    [SerializeField] ParticleSystem poisonSmog; // has the collisions
    [SerializeField] Transform playerTransform;
    [SerializeField] public float flameThrowerRange = 1f; // Maximum range of the flamethrower
    public float duration = 0f;
    public AudioSource flameThrowerSound;

    private Vector3 previousPosition;
    private bool isFlamethrowerActive = false;

    void Start()
    {
        // playerTransform = GameObject.Find("Player").transform;
        // find the player tag
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        previousPosition = playerTransform.position;

        poisonCloud.Stop();
        poisonSmog.Stop();
    }

    void Update()
    {
        AimAtCursor();
        AdjustParticleEmissionDirection();
        previousPosition = playerTransform.position;

        if (Input.GetKeyDown(KeyCode.L))
        {
            if (!isFlamethrowerActive)
            {
                StartCoroutine(ActivateFlamethrower());
            }
        }
    }

    public void FireFlameThrower()
    {
        if (!isFlamethrowerActive)
        {
            StartCoroutine(ActivateFlamethrower());
        }
    }

    void AimAtCursor()
    {
        // Get the world position of the mouse cursor
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPosition.z = 0; // Set z to 0 as we are in 2D

        // Calculate the direction from the game object to the cursor
        Vector3 direction = (cursorPosition - transform.position).normalized;

        // Rotate the game object to face the cursor
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Update the velocity over lifetime of the poisonSmog particle system
        var velocityOverLifetimeSmog = poisonSmog.velocityOverLifetime;
        velocityOverLifetimeSmog.enabled = true;
        velocityOverLifetimeSmog.x = new ParticleSystem.MinMaxCurve(direction.x * flameThrowerRange);
        velocityOverLifetimeSmog.y = new ParticleSystem.MinMaxCurve(direction.y * flameThrowerRange);

        // Update the velocity over lifetime of the poisonCloud particle system
        var velocityOverLifetimeCloud = poisonCloud.velocityOverLifetime;
        velocityOverLifetimeCloud.enabled = true;
        velocityOverLifetimeCloud.x = new ParticleSystem.MinMaxCurve(direction.x * flameThrowerRange);
        velocityOverLifetimeCloud.y = new ParticleSystem.MinMaxCurve(direction.y * flameThrowerRange);
    }

    void AdjustParticleEmissionDirection()
    {
        // Calculate the player's movement direction
        Vector3 playerMovementDirection = (playerTransform.position - previousPosition).normalized;

        // Update the velocity over lifetime of the poisonSmog particle system based on player movement
        var velocityOverLifetimeSmog = poisonSmog.velocityOverLifetime;
        velocityOverLifetimeSmog.enabled = true;
        velocityOverLifetimeSmog.x = new ParticleSystem.MinMaxCurve(velocityOverLifetimeSmog.x.constant + playerMovementDirection.x * flameThrowerRange);
        velocityOverLifetimeSmog.y = new ParticleSystem.MinMaxCurve(velocityOverLifetimeSmog.y.constant + playerMovementDirection.y * flameThrowerRange);

        // Update the velocity over lifetime of the poisonCloud particle system based on player movement
        var velocityOverLifetimeCloud = poisonCloud.velocityOverLifetime;
        velocityOverLifetimeCloud.enabled = true;
        velocityOverLifetimeCloud.x = new ParticleSystem.MinMaxCurve(velocityOverLifetimeCloud.x.constant + playerMovementDirection.x * flameThrowerRange);
        velocityOverLifetimeCloud.y = new ParticleSystem.MinMaxCurve(velocityOverLifetimeCloud.y.constant + playerMovementDirection.y * flameThrowerRange);
    }

    IEnumerator ActivateFlamethrower()
    {
        isFlamethrowerActive = true;
        poisonCloud.Play();
        poisonSmog.Play();
        flameThrowerSound.Play();

        yield return new WaitForSeconds(duration);

        poisonCloud.Stop();
        poisonSmog.Stop();
        flameThrowerSound.Stop();
        isFlamethrowerActive = false;
    }
}
