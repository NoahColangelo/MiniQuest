using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    //START OF VARIABLES

    //movement variables
    [SerializeField]
    private float movementSpeed = 5.0f;

    private Transform target;
    private Rigidbody2D rb;
    private Vector2 direction;

    //a* pathfinding variables
    private Seeker seeker;
    private Path path;

    private int currentWayPoint = 0;
    private const float nextWaypointDist = 0.5f;

    //animation variables
    private Animator animator;
    private const float animDelayTimer = 0.3f;
    private float animTimer = 0.0f;

    //health variables
    private int health = 2;
    private bool isDead = false;
    private bool canHurt = true;
    private const float deathDelayTimer = 0.5f;
    private float deathTimer = 0.0f;

    //spawning variables
    private char enemyType;
    private char enemySpawnArea;

    //END OF VARIABLES
    void Start()
    {
        //assigns components
        target = FindObjectOfType<PlayerControls>().GetComponent<Transform>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();

        //this will cause the UpdatePath function to be called every 0.5 seconds
        InvokeRepeating(nameof(UpdatePath), 0.0f, 0.5f);
    }

    void UpdatePath()
    {
        if(seeker.IsDone())//checks of the seeker is complete its path if so then itll start a new one
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path newPath)
    {
        if(!newPath.error)//if the path is okay then the AI will be given a new path
        {
            path = newPath;
            currentWayPoint = 0;//reset the waypoints
        }
    }

    void Update()
    {
        if (animTimer >= animDelayTimer)//an anim delay to prevent the animation from flickering between states
        {
            animator.SetFloat("Horizontal_Movement", Mathf.Round(direction.x));
            animator.SetFloat("Vertical_Movement", Mathf.Round(direction.y));
            animator.SetFloat("Speed", direction.sqrMagnitude);

            animTimer = 0;
        }
        else
            animTimer++;

        if(health <= 0 && !isDead)//a death timer to allow for the death animation to play before the object pool reclaims the enemy
        {
            deathTimer += Time.deltaTime;

            if(deathTimer >= deathDelayTimer)
            {
                isDead = true;
                deathTimer = 0.0f;
                canHurt = false;
                animator.SetBool("isDead", false);
            }
        }
    }

    private void FixedUpdate()
    {
        if (path == null)//makes sure a path is currently being followed
            return;

        if (currentWayPoint >= path.vectorPath.Count)//makes sure the AI doesnt look for its next waypint when there are no more
            return;

        direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;//gets the direction to move the enemy

        if(health > 0)//wont move if enemy is dead
            rb.MovePosition(rb.position + direction * movementSpeed * Time.deltaTime);// standard movement equation used on player aswell

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);//checks the distance to the next waypoint

        if(distance < nextWaypointDist)//changes waypoint if enemy is at the current waypoint
        {
            currentWayPoint++;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Projectile"))//enemies lose health and die if hit by a projectile
        {
            health--;

            if(health <= 0)
            {
                animator.SetBool("isDead", true);
            }
        }
    }

    //GETTERS, SETTERS, AND RESETERS
    public bool GetIsDead()
    {
        return isDead;
    }
    public void SetIsDead(bool temp)
    {
        isDead = temp;
    }

    public bool GetCanHurt()
    {
        return isDead;
    }
    public void SetCanHurt(bool temp)
    {
        isDead = temp;
    }

    public void ResetHealth()
    {
        isDead = false;
        canHurt = true;
        health = 2;
    }

    public void SetEnemyType(char type)
    {
        enemyType = type;
    }

    public char GetEnemySpawnArea()
    {
        return enemySpawnArea;
    }
    public void SetEnemySpawnArea(char spawnArea)
    {
        enemySpawnArea = spawnArea;
    }
}
