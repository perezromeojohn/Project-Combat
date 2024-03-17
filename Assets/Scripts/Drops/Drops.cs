using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;

public class Drops : MonoBehaviour
{
    public BoxCollider2D boxCollider;
    public GameObject icon;
    public GameObject collected;
    public enum ResourceType { XP, Health, PowerUp, Magnet }
    public ResourceType resourceToHandle;
    
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        icon.transform.rotation = Quaternion.Euler(0, 0, -10);
        icon.transform.localScale = new Vector3(0, 0, 0);
        
        LeanTween.moveLocalY(icon, -.03f, 1f).setEaseInOutSine().setLoopPingPong();
        LeanTween.scale(icon, new Vector3(1, 1, 1), .5f).setEaseInOutSine();
        LeanTween.rotateZ(icon, 10, 1f).setEaseInOutSine().setLoopPingPong();
    }

    private void Heal(GameObject player)
    {
        player.GetComponent<PlayerHealth>().HealDamage(1);
    }

    private void Operation(GameObject player)
    {
        switch (resourceToHandle)
        {
            case ResourceType.XP:
                // Debug.Log("XP");
                break;
            case ResourceType.Health:
                Heal(player);
                break;
            case ResourceType.PowerUp:
                // Debug.Log("PowerUp");
                break;
            case ResourceType.Magnet:
                // Debug.Log("Magnet");
                break;
        }
    }

    // on collide debug the hit
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Operation(other.gameObject);
            Destroy(gameObject);
            GameObject collectedAnim = Instantiate(collected, transform.position, Quaternion.identity);
            GameObject debris = GameObject.Find("Debris");
            collectedAnim.transform.SetParent(debris.transform);
        }
    }
}

[CustomEditor(typeof(Dropdown))]
public class EnumExampleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var myScript = target as Drops;
        myScript.resourceToHandle = (Drops.ResourceType)EditorGUILayout.EnumPopup("Resource Type", myScript.resourceToHandle);
    }
}