using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using MoreMountains.Feedbacks;

public class WheatProjectile : MonoBehaviour
{
    [SerializeField] GameObject wheatProjectile;
    [SerializeField] GameObject circle;
    [SerializeField] ParticleSystem wheatTrail;
    [SerializeField] TrailRenderer wheatTrailRenderer;
    [SerializeField] Light2D wheatLight;
    private MMF_Player envorimentalCameraShakeFeedback;

    public float damage = 0;

    public float explosionRadius = .3f;

    void Start()
    {
        // from the static class EnvironmentalCameraShakeFeedback, get the instance
        envorimentalCameraShakeFeedback = EnvironmentalCameraShakeFeedback.Instance;
        // Set the initial scale of the circle to 0
        circle.transform.localScale = Vector3.zero;
        wheatTrailRenderer.enabled = false;
        wheatProjectile.transform.localPosition = new Vector3(wheatProjectile.transform.localPosition.x, 2, wheatProjectile.transform.localPosition.z);

        // Tween the scale of the circle to 0.7 over 1 second
        LeanTween.scale(circle, new Vector3(0.7f, 0.7f, 0.7f), 1f).setOnComplete(() =>
        {
            // Start the sequence of actions with a 1-second delay before the explosion effect
            StartCoroutine(DelayedExplosionSequence());
        });
    }

    IEnumerator DelayedExplosionSequence()
    {
        // Enable the trail renderer 
        wheatTrailRenderer.enabled = true;

        // Move the wheat projectile to its final position with an ease-out bounce effect
        LeanTween.moveLocalY(wheatProjectile, 0, 0.5f).setEaseInQuart().setOnComplete(() =>
        {
            // Stop the wheat trail and play the explosion effect after a 1-second delay
            StartCoroutine(DelayBeforeExplosion());
            wheatTrail.Stop();
            envorimentalCameraShakeFeedback.PlayFeedbacks();
        });

        // Wait for 1 second before continuing the sequence
        yield return new WaitForSeconds(1);
    }

    IEnumerator DelayBeforeExplosion()
    {
        // Wait for 1 second
        yield return new WaitForSeconds(1);

        // based on the radius. use overlapcircle all and detect all colliders within the radius that are tagged as enemy
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(circle.transform.position, explosionRadius);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].CompareTag("Enemy"))
            {
                hitColliders[i].GetComponent<Behavior>().isAttacked = true;
                hitColliders[i].GetComponent<Behavior>().damageTaken = damage;
            }
        }
        wheatProjectile.GetComponent<SpriteRenderer>().enabled = false;
        circle.GetComponent<SpriteRenderer>().enabled = false;
        LeanTween.value(wheatLight.intensity, 0, 1).setOnUpdate((float val) =>
        {
            wheatLight.intensity = val;
        });

        // Start coroutine to stop the explosion effect after a delay
        StartCoroutine(StopExplosion());
    }

    IEnumerator StopExplosion()
    {
        // Wait for 1 second before stopping the explosion effect
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }


    // draw gizmos for the explosion radius, the gizmos should originate from the circle object
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(circle.transform.position, explosionRadius);
    }
}
