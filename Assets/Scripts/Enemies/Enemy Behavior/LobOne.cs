using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobOne : MonoBehaviour
{
    public GameObject shadowPrefab;
    public AnimationCurve trajectoryCurve;
    public float maxHeight = 2f;
    public float duration = 1f;

    // target
    private GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        FireProjectile(player);
    }

    void FireProjectile(GameObject target)
    {
        GameObject shadow = Instantiate(shadowPrefab, target.transform.position, Quaternion.identity);
        // shadow.transform.parent = projectile.transform;
        // shadow.transform.localPosition = new Vector3(0, -.05f, 0);
        // set shadow transform localposition to projectile offset -.05f on y axis
        shadow.transform.position = new Vector3(transform.position.x, transform.position.y - .05f, transform.position.z);

        Vector3 targetPos = target.transform.position;
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
        // destroy projectile
        Destroy(gameObject);
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
