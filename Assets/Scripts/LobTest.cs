using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobTest : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject shadowPrefab;
    public AnimationCurve trajectoryCurve;
    public float speed = 5f;
    public float maxHeight = 2f;
    public float duration = 1f;

    public GameObject target;

    void Update()
    {
        // if input is G, fire the projectile
        if (Input.GetKeyDown(KeyCode.G))
        {
            ThrowToTarget(target);
        }
    }

    // Public method to throw the projectile to a specific target
    public void ThrowToTarget(GameObject target)
    {
        if (target != null)
        {
            Vector3 initialPosition = transform.position;

            GameObject projectile = Instantiate(projectilePrefab, initialPosition, Quaternion.identity);
            GameObject shadow = Instantiate(shadowPrefab, initialPosition, Quaternion.identity);

            StartCoroutine(MoveProjectile(projectile.transform, shadow.transform, target.transform));
        }
        else
        {
            Debug.LogWarning("Target is null!");
        }
    }

    private IEnumerator MoveProjectile(Transform projectile, Transform shadow, Transform target)
    {
        float startTime = Time.time;
        float distance = Vector3.Distance(projectile.position, target.position);

        while (Time.time - startTime < duration)
        {
            float time = (Time.time - startTime) / duration;

            float height = trajectoryCurve.Evaluate(time) * maxHeight;

            Vector3 newPosition = Vector3.Lerp(projectile.position, target.position, time);
            newPosition.y += height;

            projectile.position = newPosition;
            shadow.position = newPosition; // Update shadow position

            yield return null;
        }

        Destroy(projectile.gameObject);
        Destroy(shadow.gameObject); // Destroy shadow after projectile
    }
}
