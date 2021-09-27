using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float lifetime = 3.0f;
    [SerializeField]
    private float projectileSpeed = 15.0f;

    private float timer = 0.0f;
    private bool isDead = false;

    [SerializeField]
    private Rigidbody2D rb;

    private Vector2 directionVector;//this vector takes the movement vector from playerControls to use for directing the projectile

    // Update is called once per frame
    private void Update()
    {
        //timer for the projectiles lifespan
        timer += Time.deltaTime;

        if (timer >= lifetime)
            isDead = true;
    }

    private void FixedUpdate()//fixed update for the movement of the projectile
    {
        rb.MovePosition(rb.position + directionVector * projectileSpeed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)//checks to see if the projectile hits an enemy or blocking tile
    {
        if(collision.transform.tag == "Blocking" || collision.transform.tag == "Enemy")
        {
            isDead = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)//this destorys the arrow if it crosses a tranistion triggerbox
    {
        if(collision.tag == "Transition")
            isDead = true;
    }
    public void setDirectionVector(Vector2 prevMovementVector)//sets the directionVector to the prevMovementVector
    {
        directionVector = prevMovementVector;
    }

    public bool getIsDead()//gets isDead
    {
        return isDead;
    }
    public void resetIsDead()//sets isDead to false and resets the life timer
    {
        isDead = false;
        timer = 0.0f;
    }
}
