using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    [SerializeField] private GameObject blade;
    private Transform player;
    private float maxSpeedMultiplier = 3f;
    public float stayDuration = 1f;
    public float travelDuration = 1f;
    public float range = 3f;
    public float speedIncreaseRate = 0.4f;
    public float damage = 0f;
    public float bladeSize = 1f;
    private Vector3 targetPosition;
    public AudioSource bladeSound;

    private List<Collider2D> hitEnemies = new List<Collider2D>();

    void Start()
    {
        // find player
        player = GameObject.FindGameObjectWithTag("Player").transform;
        transform.localScale = Vector3.zero;
        var newBladeSize = new Vector3(bladeSize, bladeSize, bladeSize); // idk why I did this
        LeanTween.scale(gameObject, newBladeSize, 0.4f).setEase(LeanTweenType.easeOutBack);

        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        targetPosition = player.position + new Vector3(randomDirection.x, randomDirection.y, 0) * range;

        bladeSound.pitch = Random.Range(0.8f, 1.2f);
        bladeSound.Play();

        StartCoroutine(ThrowBoomerang());
        StartCoroutine(ResetHitEnemiesList());
    }

    void Update()
    {
        blade.transform.Rotate(0, 0, -760 * Time.deltaTime);
    }

    IEnumerator ThrowBoomerang()
    {
        Vector3 startPosition = transform.position;
        yield return MoveToPosition(startPosition, targetPosition, travelDuration);
        yield return new WaitForSeconds(stayDuration);
        yield return ReturnToPlayer(targetPosition);
        Destroy(gameObject);
    }

    IEnumerator MoveToPosition(Vector3 from, Vector3 to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            t = EaseInOutQuad(t);
            transform.position = Vector3.Lerp(from, to, t);
            yield return null;
        }
        transform.position = to;
    }

    IEnumerator ReturnToPlayer(Vector3 from)
    {
        float initialSpeed = Vector3.Distance(from, player.position) / travelDuration;
        float currentSpeed = initialSpeed;
        float elapsedTime = 0f;
        float maxSpeed = initialSpeed * maxSpeedMultiplier;

        while (Vector3.Distance(transform.position, player.position) > 0.1f)
        {
            elapsedTime += Time.deltaTime;
            
            if (elapsedTime >= 1f)
            {
                if (currentSpeed < maxSpeed)
                {
                    currentSpeed += speedIncreaseRate;
                }
                elapsedTime = 0f;
            }

            float step = currentSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, player.position, step);
            yield return null;
        }
        transform.position = player.position;
    }

    float EaseInOutQuad(float t)
    {
        return t < 0.5f ? 2f * t * t : 1f - Mathf.Pow(-2f * t + 2f, 2f) / 2f;
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

    IEnumerator ResetHitEnemiesList()
    {
        while (true)
        {
            yield return new WaitForSeconds(.3f);
            hitEnemies.Clear();
        }
    }
}