using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private float maxDistance = .05f; // Max distance for the laser
    [SerializeField] GameObject turretFirePoint; // The point where the laser originates
    [SerializeField] GameObject targetedEnemy; // The enemy that the turret is targeting
    [SerializeField] float radius = 1f; // Radius for detecting nearby enemies
    [SerializeField] LineRenderer lineRenderer; // The line renderer for the laser

    void Start()
    {
    
    }

    void Update()
    {
        DetectNearbyEnemy();
        CheckTargetedEnemyDistance();
        DrawLaser();
    }

    void DetectNearbyEnemy()
    {
        // if targeted enemy is not null, return
        if (targetedEnemy != null)
        {
            return;
        }

        // Get all colliders within the radius
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(turretFirePoint.transform.position, radius);

        // Loop through the colliders and print their names
        foreach (Collider2D hitCollider in hitColliders)
        {
            Debug.Log("Detected object: " + hitCollider.name);
            // Set the detected enemy as the targeted enemy if not already targeted
            if (targetedEnemy == null)
            {
                targetedEnemy = hitCollider.gameObject;
            }
        }
    }

    void CheckTargetedEnemyDistance()
    {
        if (targetedEnemy != null)
        {
            float distance = Vector2.Distance(turretFirePoint.transform.position, targetedEnemy.transform.position);
            if (distance > radius)
            {
                Debug.Log("Targeted enemy out of range: " + targetedEnemy.name);
                targetedEnemy = null;
            }
        }
    }

    void DrawLaser()
    {
        Vector3 turretPosition = turretFirePoint.transform.position;
        Vector3 endPosition;

        if (targetedEnemy != null)
        {
            endPosition = targetedEnemy.transform.position;
        }
        else
        {
            endPosition = turretFirePoint.transform.position + turretFirePoint.transform.up * maxDistance;
        }

        // Ignore z positions by setting them to the same value as the turretFirePoint's z position
        turretPosition.z = turretFirePoint.transform.position.z;
        endPosition.z = turretFirePoint.transform.position.z;

        // Ensure correct order: first position is turret, second is end position
        lineRenderer.SetPosition(0, turretPosition);
        lineRenderer.SetPosition(1, endPosition);
    }


    void OnDrawGizmos()
    {
        if (turretFirePoint != null)
        {
            Gizmos.color = Color.green; // Set the color of the gizmo
            Gizmos.DrawWireSphere(turretFirePoint.transform.position, radius); // Draw a wireframe sphere
        }
    }
}
