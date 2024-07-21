using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAllMobs : MonoBehaviour
{
    [SerializeField] GameObject mobLayer;

    public void KillAll()
    {
        foreach (Transform child in mobLayer.transform)
        {
            child.GetComponent<Behavior>().MobDeath();
        }
    }
}
