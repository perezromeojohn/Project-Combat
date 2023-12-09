using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public void enemyStop()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<EnemyBehaviour>().enemyStop = true;
            // print the name of the enemy
            // Debug.Log(enemy.name);
        }
    }

    public void enemyResume()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<EnemyBehaviour>().enemyStop = false;
        }
    }

    public void playerStop()
    {
        GameObject player = GameObject.FindWithTag("Player");
        player.GetComponent<CharacterMovement>().playerStop = true;
        GameObject weaponParent = player.transform.Find("WeaponParent").gameObject;
        weaponParent.GetComponent<SwordFollowsCursor>().playerStop = true;
    }

    public void playerResume()
    {
        GameObject player = GameObject.FindWithTag("Player");
        player.GetComponent<CharacterMovement>().playerStop = false;
        GameObject weaponParent = player.transform.Find("WeaponParent").gameObject;
        weaponParent.GetComponent<SwordFollowsCursor>().playerStop = false;
    }

    public void resumeAll()
    {
        playerResume();
        enemyResume();
    }

    public void Start(){
        resumeAll();
    }
}
