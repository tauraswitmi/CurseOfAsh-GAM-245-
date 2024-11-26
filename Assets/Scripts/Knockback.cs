using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{

    public float thrust;
    public float knockTime;


    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("enemy"))
        {
            Rigidbody2D enemy = other.GetComponent<Rigidbody2D>();
            if (enemy != null)
            {
                if (enemy.GetComponent<EnemyScript>().hasCoroutineActive == true)
                {
                    StopCoroutine(enemy.GetComponent<EnemyAttacker>().AttackCoroutine());
                }

                // Changes the enemy state to tagged
                enemy.GetComponent<EnemyScript>().currentState = EnemyState.tagged;

                // Adds the enemy's corruption damage to the player's corruption meter
                this.GetComponent<PlayerCorruption>().CorruptionFill(enemy.GetComponent<EnemyScript>().corruptionDamage);

                Vector2 difference = enemy.transform.position - this.transform.position;
                difference = difference.normalized * thrust;
                enemy.AddForce(difference, ForceMode2D.Impulse);
                StartCoroutine(KnockCoroutine(enemy));
            }
        }
    }



    private IEnumerator KnockCoroutine(Rigidbody2D enemy)
    {
        if (enemy != null)
        {
            yield return new WaitForSeconds(knockTime);
            enemy.velocity = Vector2.zero;

            
            // All enemies have one health point, change this if I even have more than one health point
            enemy.GetComponent<EnemyScript>().currentState = EnemyState.dead;
        }
    }

}
