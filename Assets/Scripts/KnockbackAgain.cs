using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackAgain : MonoBehaviour
{
    public float thrust;
    public float knockTime;
    private Vector2 moveDirection;
    private float moveX;
    private float moveY;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) // If the ENEMY collides with the PLAYER
        {
            Rigidbody2D player = other.GetComponent<Rigidbody2D>(); // Gets the player's RIGIDBODY

            if (player != null)
            {
                if (player.GetComponent<PlayerMovement>().currentState != PlayerState.hitstun)
                {

                    // Changes the player state to hitstun
                    player.GetComponent<PlayerMovement>().currentState = PlayerState.hitstun;

                    // Gets the distance between the player and enemy
                    Vector2 difference = player.transform.position - this.transform.position;

                    if (Mathf.Abs(difference.x) > Mathf.Abs(difference.y))
                    {
                        moveY = 0;

                        if (difference.x > 0)
                        {
                            moveX = 1; // Hit by sword while facing to the right, knockback to the left
                        }
                        else
                        {
                            moveX = -1; // Hit by sword while facing to the left, knockback to the right
                        }
                    }
                    else
                    {
                        moveX = 0;

                        if (difference.y > 0)
                        {
                            moveY = 1; //  Hit by sword while facing up, knockback down
                        } 
                        else
                        {
                            moveY = -1; // Hit by sword while facing down, knockback up
                        }
                    }
                    moveDirection = new Vector2(moveX, moveY);

                    player.velocity = new Vector2(moveDirection.x * 2, moveDirection.y * 2);

                    StartCoroutine(Whatever(player));

                    player.GetComponent<Animator>().SetBool("dead", false);

                    player.GetComponent<PlayerCorruption>().CorruptionFill(this.transform.parent.GetComponent<EnemyScript>().corruptionDamage);
                    player.GetComponent<PlayerMovement>().currentState = PlayerState.walk;

                }
            }
        }
    }

    private IEnumerator Whatever(Rigidbody2D player) //Fuck this fucking function I hope whoever created it dies a horrible death
    {
        

        player.velocity = new Vector2(moveDirection.x * 2, moveDirection.y * 2);
        yield return new WaitForSeconds(2.0f);

        player.velocity = Vector2.zero;
    }

    /*private IEnumerator PleaseGodWork(Rigidbody2D player)
    {
        
        yield return null;
        player.GetComponent<Animator>().SetBool("dead", false);
        yield return new WaitForSeconds(1.0f);
        player.velocity = Vector2.zero;

        // Gets corruption damage from parent enemy and sends it to the player
        player.GetComponent<PlayerCorruption>().CorruptionFill(this.transform.parent.GetComponent<EnemyScript>().corruptionDamage);
        player.GetComponent<PlayerMovement>().currentState = PlayerState.walk;
    }*/
}
