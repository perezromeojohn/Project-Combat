using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggLob : MonoBehaviour
{
    public GameObject shadowPrefab;
    public GameObject explode;
    public SpriteRenderer eggIcon;
    public AnimationCurve trajectoryCurve;
    public float maxHeight = 2f;
    public float duration = 1f;
    public ParticleSystem trailEffect;

    public float searchRadius = 1f;
    public float defaultRandomRange = 0.5f;

    void Start()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, searchRadius);

        Vector3 targetPosition;
        Collider2D targetedEnemyCollider = null;

        foreach (Collider2D collider in hitColliders)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                targetedEnemyCollider = collider;
                break;
            }
        }

        if (targetedEnemyCollider != null)
        {
            targetPosition = targetedEnemyCollider.transform.position;
            // Debug.Log("Targeting: " + targetedEnemyCollider.gameObject.name);
        }
        else
        {
            targetPosition = new Vector3(transform.position.x + Random.Range(-defaultRandomRange, defaultRandomRange),
                                         transform.position.y + Random.Range(-defaultRandomRange, defaultRandomRange),
                                         transform.position.z);
            // Debug.Log("No enemies found, using default random position.");
        }

        FireProjectile(targetPosition);
    }

    void FireProjectile(Vector3 target)
    {
        GameObject shadow = Instantiate(shadowPrefab, transform.position, Quaternion.identity);
        shadow.transform.position = new Vector3(transform.position.x, transform.position.y - .05f, transform.position.z);

        Vector3 targetPos = target;
        Vector3 shadowStartPos = shadow.transform.position;

        StartCoroutine(ProjectileCurveRoutine(transform.position, targetPos));
        StartCoroutine(ShadowCurveRoutine(shadow, shadowStartPos, targetPos));
    }

    private IEnumerator ProjectileCurveRoutine(Vector3 startPos, Vector3 endPos)
    {
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float linearT = time / duration;
            float heightT = trajectoryCurve.Evaluate(linearT);
            float height = Mathf.Lerp(0, maxHeight, heightT);
            
            transform.position = Vector2.Lerp(startPos, endPos, linearT) + new Vector2(0f, height);
            // rotate z axis
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2((endPos.y - transform.position.y), (endPos.x - transform.position.x)) * Mathf.Rad2Deg);

            yield return null;
        }
        
        eggIcon.enabled = false;
        explode.SetActive(true);
        trailEffect.Stop();
    }

    private IEnumerator ShadowCurveRoutine(GameObject shadow, Vector3 startPos, Vector3 endPos)
    {
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float linearT = time / duration;
            shadow.transform.position = Vector2.Lerp(startPos, endPos, linearT);
            yield return null;
        }
        Destroy(shadow);
    }
}
