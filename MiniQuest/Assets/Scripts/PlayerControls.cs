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
    private TransitionManager transitionManager;
    [SerializeField]
    private PauseMenu pauseMenu;

    private Rigidbody2D rb;
    private Animator playerAnimator;

    private Vector2 movementVector;//vector that stores the vec2 from the input action movement
    private Vector2 prevMovementVector;//vector that stores the above vectors last value that was a non zero value

    [SerializeField]
    private float movementSpeed = 10.0f;// the added speed for the player movement

    //rotations for animations, projectile placements, and interactable object movement
    [SerializeField]
    private GameObject shootUp;//0
    [SerializeField]
    private GameObject shootRight;//90
    [SerializeField]
    private GameObject shootDown;//180
    [SerializeField]
    private GameObject shootLeft;//270

    [SerializeField]
    private float attackAnimDuration = 0.5f;//the animation duration
    private float timer = 0.0f;
    private bool isAttacking = false;

    private bool releaseProjectile = false; //used to let the attack manager know it can fire the projectile
    private float rotation = 0.0f; //rotation is for the projectile to face the correct way when fired

    private bool nearInteractableObject = false;//bool that triggers when player is near interactable object
    private bool isInteracting = false;//triggers when player presses the interact button with all conditions met
    private GameObject interactingObject = null;//object to hold the interacting objects info while the player has it


    //FUNCTIONS START HERE//
    private void Awake()
    {
        inputManager = new InputManager();

        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }//awake Function
    private void OnEnable()//function calls when object it is attatched to is enabled
    {
        movement = inputManager.PlayerController.Movement;//adds the movement action to its own variable for ease of use
        movement.Enable();

        inputManager.PlayerController.Attack.performed += Attack;//subscribes the attack binding to an event
        inputManager.PlayerController.Attack.Enable();

        inputManager.PlayerController.Interact.performed += Interact;//subscribes the Interact binding to an event
        inputManager.PlayerController.Interact.Enable();

        inputManager.PlayerController.OpenMenu.performed += OpenMenu;//subscribes the Interact binding to an event
        inputManager.PlayerController.OpenMenu.Enable();

        inputManager.UI.LeaveMenu.performed += LeaveMenu;
        inputManager.UI.LeaveMenu.Enable();
    }
    private void OnDisable()//function calls when object it is attatched to is disabled
    {
        movement.Disable();

        inputManager.PlayerController.Attack.performed -= Attack;//unsubscribes the attack action
        inputManager.PlayerController.Attack.Disable();

        inputManager.PlayerController.Interact.performed -= Interact;//unsubscribes the Interact action
        inputManager.PlayerController.Interact.Disable();

        inputManager.PlayerController.OpenMenu.performed -= OpenMenu;//unsubscribes the OpenMenu action
        inputManager.PlayerController.OpenMenu.Disable();

        inputManager.UI.LeaveMenu.performed -= LeaveMenu;//unsubscribes the CloseMenu action
        inputManager.UI.LeaveMenu.Disable();
    }

    private void Attack(InputAction.CallbackContext obj)//the attacking function
    {
        if (!isAttacking && !isInteracting)//check to see if the attack is already in progress
        {
            isAttacking = true;
            playerAnimator.SetBool("Attack", true);//sets the attack param in animation to true
        }
    }
    private void Interact(InputAction.CallbackContext obj)//the interacting function
    {
        if (nearInteractableObject && !isInteracting && !isAttacking)//checks if the player is in the trigger box of an object
            isInteracting = true;
        else if (isInteracting)//checks if player is already interacting to drop the object
        {
            isInteracting = false;
            interactingObject.transform.position = this.transform.position;//places object at players position so it doesnt get placed on an inaccessible place
            interactingObject.transform.SetParent(null);//sets the objects parent to null(no longer the player)
            interactingObject = null;
        }
    }
    private void OpenMenu(InputAction.CallbackContext obj)//the menu function to open the pause menu in game
    {
        pauseMenu.pauseGame();
    }

    private void LeaveMenu(InputAction.CallbackContext obj)//the menu function to open the pause menu in game
    {
        pauseMenu.pauseGame();        
    }

    private void Update()
    {
        movementVector = movement.ReadValue<Vector2>();//puts the movement vector from the input manager into a vec2

        if (movementVector != Vector2.zero && !isAttacking)//gets the last non zero vector from the movement vector for shooting a projectile
            prevMovementVector = movementVector;

        idleAnim();
        attackAnimDelay(Time.deltaTime);

        if (isInteracting && interactingObject != null)
            interactingObject.transform.position = checkRotation().transform.position;//moves the interacted object to the proper position around the character

        if(transitionManager.getIsTransitioning())//check to see if there is a transistion occurring
            movementVector = Vector2.zero;//makes it so the player cannot move while transitioning

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Gem")//player is interacting with a gem object
            nearInteractableObject = true;
            
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isInteracting && collision.tag == "Gem" && interactingObject == null)//checks if the interact button has been pressed and that hte player is still touching the gems trigger box
        {
            collision.gameObject.transform.SetParent(this.transform);//sets the gems parent to the player
            interactingObject = collision.gameObject;//holds the information of the gem
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Gem")//player is no longer interacting with a gem object
            nearInteractableObject = false;
    }

    private void idleAnim()//basically a switch statement for the idle anims to function properly with the way the player was last moving
    {
        if (movementVector.y == -1)//player facing down
        {
            playerAnimator.SetBool("Down_Idle", true);//
            playerAnimator.SetBool("Up_Idle", false);
            playerAnimator.SetBool("Left_Idle", false);
            playerAnimator.SetBool("Right_Idle", false);

            if(!isAttacking)
                rotation = 180;
        }
        else if (movementVector.y == 1)//player faicng up
        {
            playerAnimator.SetBool("Down_Idle", false);
            playerAnimator.SetBool("Up_Idle", true);//
            playerAnimator.SetBool("Left_Idle", false);
            playerAnimator.SetBool("Right_Idle", false);

            if (!isAttacking)
                rotation = 0;
        }
        else if (movementVector.x == -1)//player facing left
        {
            playerAnimator.SetBool("Down_Idle", false);
            playerAnimator.SetBool("Up_Idle", false);
            playerAnimator.SetBool("Left_Idle", true);//
            playerAnimator.SetBool("Right_Idle", false);

            if (!isAttacking)
                rotation = 270;
        }
        else if (movementVector.x == 1)//player facing right
        {
            playerAnimator.SetBool("Down_Idle", false);
            playerAnimator.SetBool("Up_Idle", false);
            playerAnimator.SetBool("Left_Idle", false);
            playerAnimator.SetBool("Right_Idle", true);//

            if (!isAttacking)
                rotation = 90;
        }
    }
    private void attackAnimDelay(float dt)//stops the player from moving during the attack animation
    {
        if (isAttacking)
        {
            timer += dt;//timer for the attack anim to play out

            if (timer >= attackAnimDuration)
            {
                timer = 0.0f;//timer reset

                //player is no longer attacking
                isAttacking = false;
                playerAnimator.SetBool("Attack", false);//sets the attack param in animations to false so the animation only plays once

                releaseProjectile = true;
            }
            else
            {
                movementVector = Vector2.zero;//makes it so the player cannot move while attacking
            }
        }
    }

    public InputManager getInputManager()
    {
        return inputManager;
    }
    public GameObject checkRotation()//checks which marker will be used for other objects based off the different idle anims of the character
    {
        switch (rotation)
        {
            case 0:
                return shootUp;
            case 90:
                return shootRight;
            case 180:
                return shootDown;
            case 270:
                return shootLeft;
        }
        return null;
    }
    public Vector2 GetPrevMovementVector()
    {
        return prevMovementVector;
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
