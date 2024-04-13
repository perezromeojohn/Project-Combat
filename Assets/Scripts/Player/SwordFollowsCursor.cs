using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordFollowsCursor : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject cursorGraph;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private SpriteRenderer hairSprite;
    [SerializeField] private SpriteRenderer handSprite;
    [SerializeField] private Animator swordAnimator;
    void Start()
    {
        // Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // if time scale is equal to 1 rotate the sword
        if (Time.timeScale != 0)
        {
            RotateAtCursor();
        }
    }

    private void RotateAtCursor() {
        // make this object rotate towards the cursor's position, we are working with 2d
        // so we only need to worry about the
        if (swordAnimator.GetBool("isAttacking") == false) {
            Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = rotation;
            Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // cursorGraph.transform.position = cursorPos;
            // add if time scale is == 0

            if (Time.timeScale != 0)
            {
                if (cursorPos.x < transform.position.x) {
                    playerSprite.flipX = true;
                    hairSprite.flipX = true;
                    handSprite.transform.localScale = new Vector3(-1f, 1f, 1f);
                } else {
                    playerSprite.flipX = false;
                    hairSprite.flipX = false;
                    handSprite.transform.localScale = new Vector3(1f, 1f, 1f);
                }
            }
            
            if (transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180) {
                handSprite.sortingOrder = 9;
            } else {
                handSprite.sortingOrder = 13;
            }
        }
    }
}
