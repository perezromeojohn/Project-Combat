using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hourglass : MonoBehaviour
{
    // Start is called before the first frame update
    void Update()
    {
        transform.Rotate(0, 0, 120 * Time.deltaTime);
    }
}
