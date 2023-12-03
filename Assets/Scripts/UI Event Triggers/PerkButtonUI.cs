using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkButtonUI : MonoBehaviour
{
    private Vector3 scale;
    void Start() {
        // get gameObject vector3 scale
        scale = new Vector3(.9f, .9f, .9f);
    }
    public void hoverPerkButton()
    {
        // Debug.Log("Hovering over perk button");
        // get this gameobject's rectransform scale x and y, and then leantween increment tween it with + .1
        LeanTween.scale(gameObject, new Vector3(scale.x + .1f, scale.y + .1f, scale.z), .1f).setEaseOutQuad();
    }

    public void exitsPerkButton()
    {
        // Debug.Log("Exiting perk button");
        // revert the scale back to normal
        LeanTween.scale(gameObject, scale, .1f).setEaseOutQuad();
    }
}
