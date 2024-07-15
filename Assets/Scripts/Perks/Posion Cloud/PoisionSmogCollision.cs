using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisionSmogCollision : MonoBehaviour
{
    [SerializeField] ParticleSystem poisonSmog;
    private List<ParticleCollisionEvent> collisionEvents;
    public float damage = 3f;

    void Start()
    {
        poisonSmog = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = poisonSmog.GetCollisionEvents(other, collisionEvents);

        for (int i = 0; i < numCollisionEvents; i++)
        {
            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<Behavior>().isAttacked = true;
                other.GetComponent<Behavior>().damageTaken = damage;
            }
        }
    }
}
