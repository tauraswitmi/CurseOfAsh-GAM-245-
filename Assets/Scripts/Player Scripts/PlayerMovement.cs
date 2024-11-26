using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    walk,
    sprint,
    dash,
    recover,
    hitstun,
    dead
}


public class PlayerMovement : MonoBehaviour
{
    public PlayerState currentState;
    
    public float speed;
    public int currentRoom;

    private Animator animator;
    private Rigidbody2D myRigidBody;

    private Vector2 moveDirection;

    private float dashX = 0;
    private float dashY = -1;

    private PlayerEnergy playerEnergy;
    private PlayerCorruption playerCorruption;
    public EnemyActivator enemyRoom;
    public RoomMove camer;

    public Transform[] spawns;
    public int spawnId = 0;


    // Start is called before the first frame update
    void Start()
    {
        currentState = PlayerState.walk;
        animator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
        playerEnergy = GetComponent<PlayerEnergy>();
        playerCorruption = GetComponent<PlayerCorruption>();
        enemyRoom = GetComponent<EnemyActivator>();
        camer = GetComponent<RoomMove>();


    }

    // Updates the states
    void Update()
    {

        // Checks if Directional Buttons are being pressed (UP DOWN LEFT RIGHT WASD)
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(moveX, moveY).normalized; // Gets movement direction

        // Gets the direction the player is facing when IDLE
        DashIdle(moveX, moveY);

        //Debug.Log(playerCorruption.corruption);
        Debug.Log(currentState);

        //=======================================================================================//

        // If the player's corruption level reaches 100%, switch to dead state
        if (playerCorruption.corruption == 100)
        {
            currentState = PlayerState.dead;
        }

        // If the player runs out of energy, switch to recovery state
        else if (playerEnergy.energy == 0) {
            currentState = PlayerState.recover;
        }


        // Checks if the player is dead.
        if (Input.GetButtonDown("reset"))
        {
            myRigidBody.MovePosition(spawns[spawnId].position);
            if (currentRoom == 8)
            {
                currentRoom = 6;
                enemyRoom.DeactivateEnemies(8);
                enemyRoom.ActivateEnemies(6);
                camer.RoomCameraChange(-27, 0, -27, 0);
            }
            else if (currentRoom == 7)
            {
                currentRoom = 3;
                camer.RoomCameraChange(-32, -27, -31, -22);
            }
            else
            {

                enemyRoom.DeactivateEnemies(currentRoom);
                enemyRoom.ActivateEnemies(currentRoom);
            }

            if (currentState == PlayerState.dead)
            {
                playerCorruption.corruption = 0;
                currentState = PlayerState.walk;
                animator.SetBool("dead", false);
            }
        }
        
        if (currentState != PlayerState.dead) 
        {
            if (currentState != PlayerState.hitstun)
            {
                
                // Checks if player is recovering. If it is, the player cannot sprint or dash
                if (currentState != PlayerState.recover)
                {
                    // If the Dash Button is pressed (X) //
                    if (Input.GetButtonDown("dash") && currentState != PlayerState.dash)
                    {
                        if (moveX == 0 && moveY == 0) { moveDirection = new Vector2(dashX, dashY); }
                        playerCorruption.CorruptionDrain();
                        Move(2); // DASHING
                        StartCoroutine(DashCoroutine());

                    }
                    else if (currentState == PlayerState.walk || currentState == PlayerState.sprint)
                    {
                        UpdateAnimationAndMove();


                        // If the Sprint Button is pressed (Z) //
                        if (Input.GetButton("sprint"))
                        {
                            currentState = PlayerState.sprint;
                            animator.SetBool("sprinting", true);

                        }
                        // If the player is just pressing the movement buttons (WASD, UP, DOWN, LEFT, RIGHT) //
                        else
                        {
                            currentState = PlayerState.walk;
                            animator.SetBool("sprinting", false);

                        }
                    }
                }
                else
                {
                    // Player is recovering from 0 to MAX ENERGY //

                    UpdateAnimationAndMove();
                    animator.SetBool("sprinting", false);

                    if (playerEnergy.energy == playerEnergy.energyMAX) { currentState = PlayerState.walk; }
                }
            }
            else
            {
                animator.SetBool("dead", true);
            }
        } 
        else
        {
            animator.SetBool("dead", true);
            Move(0);
        }
    }

    void FixedUpdate()
    {
        // Updates the player's movement and energy so that it goes at a consistant rate //

        if (currentState == PlayerState.recover)
        {
            Move(0.6f); // RECOVERING
            playerEnergy.EnergyRecoverEMPTY();
            playerCorruption.CorruptionDrain();

        } 
        else if (currentState == PlayerState.sprint)
        {
            Move(1.4f); // SPRINTING
            playerEnergy.EnergyDrainSprint();
            playerCorruption.CorruptionDrain();

        }
        else if (currentState == PlayerState.walk)
        {
            Move(1); // WALKING
            playerEnergy.EnergyRecover();
            playerCorruption.CorruptionDrain();

        }
    }

    private IEnumerator DashCoroutine() 
    {
        animator.SetBool("dashing", true);
        currentState = PlayerState.dash;
        yield return null;
        animator.SetBool("dashing", false);
        yield return new WaitForSeconds(.5f);
        playerEnergy.EnergyDrainDash();
        if (currentState != PlayerState.recover)
        {
            currentState = PlayerState.walk;
        }

    }


    void UpdateAnimationAndMove()
    {
        if (moveDirection != Vector2.zero)
        {
            animator.SetFloat("moveX", moveDirection.x);
            animator.SetFloat("moveY", moveDirection.y);
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
    }



    void Move(float extra)
    {
        myRigidBody.velocity = new Vector2(moveDirection.x * speed * extra, moveDirection.y * speed * extra);
    }

    void Stop()
    {
        myRigidBody.velocity = new Vector2(0, 0);
    }



    void DashIdle(float moveX, float moveY) // Records the direction the player was when previously moving
    {
        if (moveX == 1 || moveX == -1)
        {
            dashX = moveX;
            dashY = 0;
            return;
        }
        if (moveY == 1 || moveY == -1)
        {
            dashY = moveY;
            dashX = 0;
        }
    }

}
