using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    private InputManager inputManager;
    private InputAction movement;

    public Rigidbody2D rb;

    [SerializeField]
    private float movementSpeed = 10.0f;

    private void Awake()
    {
        inputManager = new InputManager();
    }

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
        Debug.Log("Le Attack");
    }


    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.ReadValue<Vector2>() * movementSpeed * Time.fixedDeltaTime);
    }

}
