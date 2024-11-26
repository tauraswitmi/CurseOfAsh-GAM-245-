using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaser : EnemyScript
{
    private Rigidbody2D myRigidbody;
    private Animator animator;

    public Transform target;

    public float chaseRadius;

    private Vector2 homePosition;


    // Start is called before the first frame update
    void Start()
    {
        currentState = EnemyState.idle;

        homePosition = new Vector2(transform.position.x, transform.position.y);

        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();

        target = GameObject.FindWithTag("Player").transform;
        hasCoroutineActive = false;

    }


    void Update()
    {
        // Checks if the enemy is dead
        if (currentState != EnemyState.dead)
        {
            // Checks if the enemy is tagged
            if (currentState != EnemyState.tagged)
            {
                // Checks if the player is in range
                if (Vector2.Distance(target.position, transform.position) <= chaseRadius)
                {
                    currentState = EnemyState.chase;
                }
                // Checks if the player is out of range and if the enemy is not at their home position
                else if (Vector2.Distance(homePosition, transform.position) != 0)
                {
                    currentState = EnemyState.back;

                }
            }
        }

    }

    void FixedUpdate()
    {
        if (currentState == EnemyState.dead)
        {
            transform.position = homePosition;
            animator.SetBool("tagged", false);
            animator.SetBool("moving", false);
            currentState = EnemyState.idle;

            this.gameObject.SetActive(false);
        }

        else if (currentState == EnemyState.tagged)
        {
            animator.SetBool("tagged", true);
        }
        
        else if (currentState == EnemyState.chase)
        {
            animator.SetBool("moving", true);
            MoveAnimate(target.position);
            Move(target.position);
        }

        else if (currentState == EnemyState.back)
        {
            animator.SetBool("moving", true);
            MoveAnimate(homePosition);
            Move(homePosition);

            if (Vector2.Distance(homePosition, transform.position) == 0)
            {
                currentState = EnemyState.idle;
                animator.SetBool("moving", false);
            }
        }
        
    }

    void Move(Vector2 moveTo)
    {
        Vector2 temp = Vector2.MoveTowards(transform.position, moveTo, speed * Time.deltaTime);
        myRigidbody.MovePosition(temp);
    }


    void MoveAnimate(Vector2 moveTo) // Gets the direction of the enemy while chasing the player
    {
        Vector2 current = transform.position;
        Vector2 temp = moveTo - current; 

        if (Mathf.Abs(temp.x) > Mathf.Abs(temp.y))
        {
            animator.SetFloat("moveY", 0);

            if (temp.x > 0)
            {
                animator.SetFloat("moveX", 1);
            } else
            {
                animator.SetFloat("moveX", -1);
            }
        } else
        {
            animator.SetFloat("moveX", 0);
            
            if (temp.y > 0)
            {
                animator.SetFloat("moveY", 1);
            } else
            {
                animator.SetFloat("moveY", -1);
            }
        }
    }

}
