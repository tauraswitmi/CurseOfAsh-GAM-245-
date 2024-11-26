using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActivator : MonoBehaviour
{

    private GameObject[] enemies;

    // Start is called before the first frame update
    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("enemy");
        foreach (GameObject enemy in enemies)
        {
            enemy.SetActive(false);
        }
        ActivateEnemies(3);
    }

    public void ActivateEnemies (int room)
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<EnemyScript>().room == room)
            {
                if (room == 6 || room == 8)
                {
                    enemy.GetComponent<EnemyScript>().currentState = EnemyState.idle;
                } else
                {
                    enemy.GetComponent<EnemyScript>().currentState = EnemyState.move;
                }
                
                enemy.SetActive(true);
            }
        }
    }

    public void DeactivateEnemies (int room)
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<EnemyScript>().room == room)
            {
                enemy.GetComponent<EnemyScript>().currentState = EnemyState.dead;
            }
        }
    }
}
