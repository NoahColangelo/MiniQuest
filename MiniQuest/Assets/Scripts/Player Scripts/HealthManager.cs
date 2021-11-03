using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    [SerializeField]
    private Sprite fullHeart;
    [SerializeField]
    private Sprite deadHeart;

    private int hearts = 3;//number of health

    private bool dead = false;//is the player dead?

    private bool gameFinish = false;//has the player entered the door

    //timer variables
    private float invulnerabilityTime = 1.0f;
    private float timer = 0.0f;
    private bool playerInvulnerable = false;

    private AudioSource audioSource;
    [SerializeField] AudioClip playerHit;

    [SerializeField]
    private GameObject[] health = new GameObject[3];//holds the hearts that are in the UI

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        invulnerableTimer(Time.deltaTime);

        if(hearts == 0 && !dead)//checks if the player is dead
        {
            dead = true;
            //player is dead, game over
            SceneManager.LoadScene(sceneName: "GameOverScene");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Heart") && hearts < 3)//checks if the player has collided with a heart
        {
            GainHeart();
            collision.GetComponent<Heart>().SetHeartUsed(true);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Enemy") && !playerInvulnerable)//checks if the player has collided with an enemy and vulnerable to damage
        {
            audioSource.PlayOneShot(playerHit);//plays player hit audio
            LoseHeart();
            playerInvulnerable = true;
        }
        
        if(collision.transform.CompareTag("DoorOpen") && !gameFinish)
        {
            gameFinish = true;
            SceneManager.LoadScene(sceneName: "GameCompleteScene");
        }
    }

    private void LoseHeart()
    {
        hearts--;//drops hearts by 1

        for(int i = 0; i < health.Length; i++)//checks from the front to the back of the array to change the hearts on the ui to dead ones
        {
            if(health[i].GetComponent<Image>().sprite == fullHeart)
            {
                health[i].GetComponent<Image>().sprite = deadHeart;
                break;
            }
        }
    }

    private void GainHeart()
    {
        hearts++;//increases hearts by 1

        for (int i = 2; i < health.Length; i--)//checks from the back to the front of the array to refill the hearts on the ui to full hearts
        {
            if (health[i].GetComponent<Image>().sprite == deadHeart)
            {
                health[i].GetComponent<Image>().sprite = fullHeart;
                break;
            }
        }
    }

    private void invulnerableTimer(float dt)//an invulnerability timer for when the player gets hit
    {
        if (playerInvulnerable && timer < invulnerabilityTime)
        {
            timer += dt;
        }
        else if (timer > invulnerabilityTime)//once timer has finished
        {
            timer = 0.0f;
            playerInvulnerable = false;
        }
    }
}
