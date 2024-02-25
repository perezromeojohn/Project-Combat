using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartAnimation : MonoBehaviour
{
    void Start()
    {
        gameObject.transform.rotation = Quaternion.Euler(0, 0, -20);
        gameObject.transform.localScale = new Vector3(.9f, .9f, .9f);
        // set this object size to .8f.8f.8f and pingpong scale it to 1 1 1
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.3f).setEase(LeanTweenType.easeOutQuart).setLoopPingPong(-1); 
        // leantween rotate z axis to 20
        LeanTween.rotateZ(gameObject, 20, 1f).setEase(LeanTweenType.easeInOutQuad).setLoopPingPong(-1);
    }
}
