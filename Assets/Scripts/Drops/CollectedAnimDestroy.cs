using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedAnimDestroy : MonoBehaviour
{
    void Destroy()
    {
        Destroy(gameObject);
    }
}
