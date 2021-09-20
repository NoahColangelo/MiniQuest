using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    //VARIABLES START HERE//

    private InputManager inputManager;
    private InputAction movement;

    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private Animator playerAnimator;

    private Vector2 movementVector;//vector that stores the vec2 from the input action movement
    private Vector2 prevMovementVector;//vector that stores the above vectors last value that was a non zero value

    [SerializeField]
    private float movementSpeed = 10.0f;// the added speed for the player movement

    [SerializeField]
    private float attackAnimDuration = 0.5f;
    private float timer = 0.0f;
    private bool isAttacking = false; //attack timer and duration

    private bool releaseProjectile = false; //used to let the attack manager know it can fire the projectile
    private float rotation = 0.0f; //rotation is for the projectile to face the correct way when fired


    //FUNCTIONS START HERE//
    private void Awake()
    {
        inputManager = new InputManager();
    }//awake Function
    private void OnEnable()//function calls when object it is attatched to is enabled
    {
        movement = inputManager.PlayerController.Movement;//adds the movement action to its own variable for ease of use
        movement.Enable();

        inputManager.PlayerController.Attack.performed += Attack;//subscribes the attack binding to an event
        inputManager.PlayerController.Attack.Enable();
    }
    private void OnDisable()//function calls when object it is attatched to is disabled
    {
        movement.Disable();
        inputManager.PlayerController.Attack.Disable();
    }

    private void Attack(InputAction.CallbackContext obj)//the attacking function
    {
        if (!isAttacking)//check to see if the attack is already in progress
        {
            isAttacking = true;
            playerAnimator.SetBool("Attack", true);//sets the attack param in animation to true
        }
    }

    private void idleAnim()//basically a switch statement for the idle anims to function properly with the way thep layer was last moving
    {
        if (movementVector.y == -1)//player facing down
        {
            playerAnimator.SetBool("Down_Idle", true);//
            playerAnimator.SetBool("Up_Idle", false);
            playerAnimator.SetBool("Left_Idle", false);
            playerAnimator.SetBool("Right_Idle", false);

            rotation = 180;
        }
        else if (movementVector.y == 1)//player faicng up
        {
            playerAnimator.SetBool("Down_Idle", false);
            playerAnimator.SetBool("Up_Idle", true);//
            playerAnimator.SetBool("Left_Idle", false);
            playerAnimator.SetBool("Right_Idle", false);

            rotation = 0;
        }
        else if (movementVector.x == -1)//player facing left
        {
            playerAnimator.SetBool("Down_Idle", false);
            playerAnimator.SetBool("Up_Idle", false);
            playerAnimator.SetBool("Left_Idle", true);//
            playerAnimator.SetBool("Right_Idle", false);

            rotation = 90;
        }
        else if (movementVector.x == 1)//player facing right
        {
            playerAnimator.SetBool("Down_Idle", false);
            playerAnimator.SetBool("Up_Idle", false);
            playerAnimator.SetBool("Left_Idle", false);
            playerAnimator.SetBool("Right_Idle", true);//

            rotation = 270;
        }
    }

    private void Update()
    {
        movementVector = movement.ReadValue<Vector2>();//puts the movement vector from the input manager into a vec2

        if (movementVector != Vector2.zero)
            prevMovementVector = movementVector;

        idleAnim();

        if(isAttacking)
        {
            timer += Time.deltaTime;//timer for the attack anim to play out
            
            if(timer >= attackAnimDuration)
            {
                timer = 0.0f;//timer reset

                //player is no longer attacking
                isAttacking = false;
                playerAnimator.SetBool("Attack", false);

                releaseProjectile = true;
            }
            else
            {
                movementVector = Vector2.zero;//makes it so the player cannot move while attacking
            }
        }

        //sets the parameters for animation based on the movement vector from the input manager
        playerAnimator.SetFloat("HorizontalMovement", movementVector.x);
        playerAnimator.SetFloat("VerticalMovement", movementVector.y);
        playerAnimator.SetFloat("Speed", movementVector.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        //movement equation equivalent to P = p + v*t with added speed
        rb.MovePosition(rb.position + movementVector * movementSpeed * Time.fixedDeltaTime);
    }

    public Vector2 GetPrevMovementVector()
    {
        return prevMovementVector;
    }

    public float getRotation()
    {
        return rotation;
    }

    public bool getReleaseProjectile()
    {
        return releaseProjectile;
    }
    public void setReleaseProjectile(bool temp)
    {
        releaseProjectile = temp;
    }
}
