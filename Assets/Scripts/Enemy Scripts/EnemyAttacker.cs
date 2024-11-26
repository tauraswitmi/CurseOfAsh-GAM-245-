using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacker : EnemyScript
{
    private Rigidbody2D myRigidbody;
    private Animator animator;

    public Transform target;
    public Transform[] path;
    public Transform currentGoal;
    public int currentPoint;

    public float roundingDistance;
    public float runRadiusENTER;
    public float runRadiusLEAVE;
    public float attackRadiusENTER;

    private bool stopfucker; // Stops all the fucking states that keep the attack from working
    private bool directionONE;
    private bool checks;
    private int direction;

    private float activate;

    public GameObject gate;

    // Start is called before the first frame update
    void Start()
    {
        currentState = EnemyState.move;

        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();

        target = GameObject.FindWithTag("Player").transform;

        stopfucker = false;
        directionONE = true;
        checks = true;
        direction = 3;
        activate = 105f;

        hasCoroutineActive = false;
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log(currentState);

        // Checks if the enemy is NOT DEAD
        if (currentState != EnemyState.dead)
        {
            // Checks if the enemy is NOT TAGGED
            if (currentState != EnemyState.tagged)
            {
                // Checks if the enemy is NOT ATTACKING
                if (currentState != EnemyState.attack)
                {
                    // Checks if the enemy is EVADING
                    if (currentState == EnemyState.evade)
                    {
                        // Checks if the enemy is no longer in range of the player
                        if (Vector2.Distance(target.position, transform.position) > runRadiusLEAVE && !stopfucker) // Exits running range
                        {
                            currentState = EnemyState.move;
                        }

                        // Checks if the player is in range for the enemy to attack the player
                        else if (Vector2.Distance(target.position, transform.position) <= attackRadiusENTER && activate >= 100) // Enters attack range
                        {
                            currentState = EnemyState.attack;
                        }
                    }

                    // If the player is NOT EVADING or NOT ATTACKING
                    else
                    {
                        checks = true;

                        // If the player enters RUNNING range
                        if (Vector2.Distance(target.position, transform.position) <= runRadiusENTER && !stopfucker) // Enters running range
                        {
                            if (checks)
                            {
                                ChangeDirection();
                                checks = false;
                            }

                            currentState = EnemyState.evade;
                        }
                    }
                }

                // If the player is ATTACKING
                else
                {
                    hasCoroutineActive = true;
                    //stopfucker = true;
                    animator.SetBool("attacking", true);
                    StartCoroutine(AttackCoroutine());
                    activate = 0;
                }
              
            }
        }
    }

    void FixedUpdate()
    {
        // If the enemy is DEAD
        if (currentState == EnemyState.dead)
        {
            transform.position = path[0].position;
            directionONE = true;
            currentPoint = 1;
            currentGoal = path[currentPoint];
            animator.SetBool("running", false);
            animator.SetBool("tagged", false);
            animator.SetBool("attacking", false);
            currentState = EnemyState.move;
            gate.GetComponent<GateFour>().tally--;

            this.gameObject.SetActive(false);
        }

        // If the enemy is TAGGED
        else if (currentState == EnemyState.tagged)
        {
            animator.SetBool("attacking", false);
            animator.SetBool("tagged", true);
        }

        // If the enemy is RUNNING
        else if (currentState == EnemyState.evade && !stopfucker)
        {
            animator.SetBool("attacking", false);
            animator.SetBool("running", true);
            Move(1.2f);
            MoveAnimate();
        }

        // If the enemy is ONLY MOVING
        else if (currentState == EnemyState.move && !stopfucker)
        {
            animator.SetBool("attacking", false);
            animator.SetBool("running", false);
            Move(0.7f);
            MoveAnimate();
        }

        if (activate < 100)
        {
            activate += 0.5f;
        }
    }



    public IEnumerator AttackCoroutine()
    {
        currentState = EnemyState.attack;

        animator.SetBool("attacking", true);
        yield return null;

        
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("attacking", false);
        currentState = EnemyState.move;
        //stopfucker = false;
        hasCoroutineActive = false;
    }



    void Move(float spd)
    {
        // If the player is not within the rounding distance, it will move towards the next POSITION[]
        if (Vector2.Distance(transform.position, path[currentPoint].position) > roundingDistance)
        {
            Vector2 temp = Vector2.MoveTowards(transform.position, path[currentPoint].position, speed * spd * Time.deltaTime);
            myRigidbody.MovePosition(temp);
        }
        else
        {
            ChangeGoal();
        }
    }



    void MoveAnimate() // Gets the direction of the enemy while chasing the player
    {
        Vector3 current = transform.position;
        Vector3 temp = path[currentPoint].position - current;

        
        // If the (ENEMY) is closer -HORIZONTALLY- to the POSITION[]
        if (Mathf.Abs(temp.x) > Mathf.Abs(temp.y))
        {
            animator.SetFloat("moveY", 0);

            // If the POSITION[] is to the RIGHT of the (ENEMY)
            if (temp.x > 0)
            {
                animator.SetFloat("moveX", 1);
                direction = 0;
            }

            // If the POSITION[] is to the LEFT of the (ENEMY)
            else
            {
                animator.SetFloat("moveX", -1);
                direction = 2;
            }
        }

        // If the (ENEMY) is closer -VERTICALLY- to the (POSITION[])
        else
        {
            animator.SetFloat("moveX", 0);

            // If the POSITION[] is ABOVE the (ENEMY)
            if (temp.y > 0)
            {
                animator.SetFloat("moveY", 1);
                direction = 1;
            }

            // If the POSITION[] is BELOW the (ENEMY)
            else
            {
                animator.SetFloat("moveY", -1);
                direction = 3;
            }
        }
    }


    private void ChangeGoal()
    {
        // If going COUNTER-CLOCKWISE, change path to CLOCKWISE
        if (directionONE)
        {
            if (currentPoint == path.Length - 1)
            {
                currentPoint = 0;
                currentGoal = path[0];
            }
            else
            {
                currentPoint++;
                currentGoal = path[currentPoint];
            }
        }

        // If going CLOCKWISE, change path to COUNTER-CLOCKWISE
        else
        {
            if (currentPoint == 0)
            {
                currentPoint = path.Length - 1;
                currentGoal = path[currentPoint];
            }
            else
            {
                currentPoint--;
                currentGoal = path[currentPoint];
            }
        }
    }

    private void ChangeDirection()
    {
        Vector3 temp = target.position - transform.position; // Gets the player's position relative to the enemy's

        // If going COUNTER-CLOCKWISE
        if (directionONE)
        {
            // If facing RIGHT and the (PLAYER'S POSITION) is to the RIGHT of the (ENEMY)
            if (direction == 0 && temp.x >= 0)
            {
                directionONE = false;
                ChangeGoal();
            }
            
            // If facing UP and the (PLAYER'S POSITION) is ABOVE the (ENEMY)
            else if (direction == 1 && temp.y >= 0)
            {
                directionONE = false;
                ChangeGoal();
            }

            // If facing LEFT and the (PLAYER'S POSITION) is to the LEFT of the (ENEMY)
            else if (direction == 2 && temp.x < 0)
            {
                directionONE = false;
                ChangeGoal();
            }

            // If facing DOWN and the (PLAYER'S POSITION) is BELOW the (ENEMY)
            else if (direction == 3 && temp.y < 0)
            {
                directionONE = false;
                ChangeGoal();
            }
        }
        
        // If going CLOCKWISE
        else
        {
            // If facing RIGHT and the (PLAYER'S POSITION) is to the RIGHT of the (ENEMY)
            if (direction == 0 && temp.x >= 0)
            {
                directionONE = true;
                ChangeGoal();
            }

            // If facing UP and the (PLAYER'S POSITION) is ABOVE the (ENEMY)
            else if (direction == 1 && temp.y >= 0)
            {
                directionONE = true;
                ChangeGoal();
            }

            // If facing LEFT and the (PLAYER'S POSITION) is to the LEFT of the (ENEMY)
            else if (direction == 2 && temp.x < 0)
            {
                directionONE = true;
                ChangeGoal();
            }

            // If facing DOWN and the (PLAYER'S POSITION) is BELOW the (ENEMY)
            else if (direction == 3 && temp.y < 0)
            {
                directionONE = true;
                ChangeGoal();
            }
        }
    }
}
