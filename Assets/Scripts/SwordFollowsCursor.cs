using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SwordFollowsCursor : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private GameObject cursorGraph;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private SpriteRenderer hairSprite;
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        AimMouse();
    }

    private void AimMouse() {
        // make this object rotate towards the cursor's position, we are working with 2d
        // so we only need to worry about the z axis
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorGraph.transform.position = cursorPos;

        // flip the player sprite based on the cursor position
        if (cursorPos.x < transform.position.x) {
            playerSprite.flipX = true;
            hairSprite.flipX = true;
        } else {
            playerSprite.flipX = false;
            hairSprite.flipX = false;
        }
    }
}
