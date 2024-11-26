using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEvader : EnemyScript
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
    public float sprintRadiusENTER;
    public float sprintRadiusLEAVE;

    private bool directionONE;
    private bool checks;
    private int direction;

    public GameObject gate;

    // Start is called before the first frame update
    void Start()
    {
        currentState = EnemyState.move;

        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();

        target = GameObject.FindWithTag("Player").transform;

        directionONE = true;
        checks = true;
        direction = 3;
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
                // Checks if the enemy is running away from player
                if (currentState == EnemyState.evade)
                {
                    if (Vector2.Distance(target.position, transform.position) > runRadiusLEAVE) // Exits running range
                    {
                        currentState = EnemyState.move;
                    }
                    else if (Vector2.Distance(target.position, transform.position) <= sprintRadiusENTER) // Enters sprinting range
                    {
                        currentState = EnemyState.evadeRun;
                    }
                }
                // Checks if the enemy is sprinting away from player
                else if (currentState == EnemyState.evadeRun)
                {
                    if (Vector2.Distance(target.position, transform.position) > sprintRadiusLEAVE) // Exits sprinting range
                    {
                        currentState = EnemyState.evade;
                    }
                } 
                // Checks if the enemy is patrolling
                else
                {
                    checks = true;

                    if (Vector2.Distance(target.position, transform.position) <= runRadiusENTER) // Enters running range
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
        }
    }

    void FixedUpdate()
    {

        if (currentState == EnemyState.dead)
        {
            transform.position = path[0].position;
            directionONE = true;
            currentPoint = 1;
            currentGoal = path[currentPoint];
            animator.SetBool("running", false);
            animator.SetBool("tagged", false);
            currentState = EnemyState.move;

            gate.GetComponent<GateThree>().tally--;
            this.gameObject.SetActive(false);
        }

        else if (currentState == EnemyState.tagged)
        {
            animator.SetBool("tagged", true);
        }

        else if (currentState == EnemyState.evadeRun)
        {
            animator.SetBool("running", true);
            Move(1.8f);
            MoveAnimate();
        }

        else if (currentState == EnemyState.evade)
        {
            animator.SetBool("running", true);
            Move(1.2f);
            MoveAnimate();
        }

        else if (currentState == EnemyState.move)
        {
            animator.SetBool("running", false);
            Move(0.7f);
            MoveAnimate();
        }
    }


    void Move(float spd)
    {
        if (Vector2.Distance(transform.position, path[currentPoint].position) > roundingDistance)
        {
            Vector2 temp = Vector2.MoveTowards(transform.position, path[currentPoint].position, speed * spd * Time.deltaTime);
            myRigidbody.MovePosition(temp);
        } else
        {
            ChangeGoal();
        }
    }


    void MoveAnimate() // Gets the direction of the enemy while chasing the player
    {
        Vector3 current = transform.position;
        Vector3 temp = path[currentPoint].position - current;

        if (Mathf.Abs(temp.x) > Mathf.Abs(temp.y))
        {
            animator.SetFloat("moveY", 0);
            

            if (temp.x > 0)
            {
                animator.SetFloat("moveX", 1);
                direction = 0;
            }
            else
            {
                animator.SetFloat("moveX", -1);
                direction = 2;
            }
        }
        else
        {
            animator.SetFloat("moveX", 0);

            if (temp.y > 0)
            {
                animator.SetFloat("moveY", 1);
                direction = 1;
            }
            else
            {
                animator.SetFloat("moveY", -1);
                direction = 3;
            }
        }
    }


    private void ChangeGoal()
    {
        if (directionONE)
        {
            if (currentPoint == path.Length - 1)
            {
                currentPoint = 0;
                currentGoal = path[0];
            } else
            {
                currentPoint++;
                currentGoal = path[currentPoint];
            }
        } else
        {
            if (currentPoint == 0)
            {
                currentPoint = path.Length - 1;
                currentGoal = path[currentPoint];
            } else
            {
                currentPoint--;
                currentGoal = path[currentPoint];
            }
        }
    }

    private void ChangeDirection()
    {
        Vector3 temp = target.position - transform.position;

        if (directionONE)
        {
            if (direction == 0 && temp.x >= 0)
            {
                directionONE = false;
                ChangeGoal();
            } 
            else if (direction == 1 && temp.y >= 0)
            {
                directionONE = false;
                ChangeGoal();
            }
            else if (direction == 2 && temp.x < 0)
            {
                directionONE = false;
                ChangeGoal();
            }
            else if (direction == 3 && temp.y < 0)
            {
                directionONE = false;
                ChangeGoal();
            }
        }
        else
        {
            if (direction == 0 && temp.x >= 0)
            {
                directionONE = true;
                ChangeGoal();
            }
            else if (direction == 1 && temp.y >= 0)
            {
                directionONE = true;
                ChangeGoal();
            }
            else if (direction == 2 && temp.x < 0)
            {
                directionONE = true;
                ChangeGoal();
            }
            else if (direction == 3 && temp.y < 0)
            {
                directionONE = true;
                ChangeGoal();
            }
        }
    }
}
