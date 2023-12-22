using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public GameObject shieldRotate;
    public GameObject shadows;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // rotate the Z axis of the shield by 1 multiplied by Time.deltaTime
        transform.Rotate(0, 0, 240 * Time.deltaTime);

        // how can I rotate the shieldRotate
        shieldRotate.transform.Rotate(0, 0, 240 * Time.deltaTime);
    }
}
