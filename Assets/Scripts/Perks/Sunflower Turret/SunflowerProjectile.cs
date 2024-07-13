using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunflowerProjectile : MonoBehaviour
{
    public float spiralSpeed = 5f; // Speed of the spiral movement
    public float spiralRadiusSpeed = 0.1f; // Speed at which the spiral radius increases
    public float lifetime = 10f; // Time in seconds before the projectile is destroyed
    public float damage = 0f; // Damage dealt by the projectile

    private List<Collider2D> hitEnemies = new List<Collider2D>();

    private float elapsedTime = 0f;
    private Vector3 initialPosition;
    
    [SerializeField] GameObject sunflowerIcon;

    void Start()
    {
        // Store the initial position of the projectile
        initialPosition = transform.position;

        // Start the coroutine to destroy the projectile after a certain time
        StartCoroutine(DestroyAfterTime());
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        MoveInSpiral();

        sunflowerIcon.transform.Rotate(0, 0, 180 * Time.deltaTime);
    }

    void MoveInSpiral()
    {
        // Calculate the new position in a consistent spiral pattern
        float angle = spiralSpeed * elapsedTime;
        float radius = spiralRadiusSpeed * elapsedTime;
        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;

        // Update the position of the projectile relative to its initial position
        transform.position = initialPosition + new Vector3(x, y, 0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!hitEnemies.Contains(other))
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                hitEnemies.Add(other);
                other.GetComponent<Behavior>().isAttacked = true;
                other.GetComponent<Behavior>().damageTaken = damage;
            }
        }
    }

    IEnumerator DestroyAfterTime()
    {
        // Wait for the specified lifetime
        yield return new WaitForSeconds(lifetime);

        // Scale down to 0 over 0.4 seconds
        LeanTween.scale(gameObject, Vector3.zero, 0.2f).setOnComplete(() =>
        {
            // Destroy the projectile
            Destroy(gameObject);
        });
    }
}
