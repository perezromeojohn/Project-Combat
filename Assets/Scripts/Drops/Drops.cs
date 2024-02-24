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
        icon.transform.rotation = Quaternion.Euler(0, 0, -10);
        icon.transform.localScale = new Vector3(0, 0, 0);
        
        LeanTween.moveLocalY(icon, -.03f, 1f).setEaseInOutSine().setLoopPingPong();
        LeanTween.scale(icon, new Vector3(1, 1, 1), .5f).setEaseInOutSine();
        LeanTween.rotateZ(icon, 10, 1f).setEaseInOutSine().setLoopPingPong();
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
