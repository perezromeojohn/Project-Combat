using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drops : MonoBehaviour
{
    public BoxCollider2D boxCollider;
    public GameObject icon;
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        LeanTween.moveLocalY(icon, -.03f, 1f).setEaseInOutSine().setLoopPingPong();
    }

    // on collide debug the hit
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit the drop");
            Destroy(gameObject);
        }
    }
}
