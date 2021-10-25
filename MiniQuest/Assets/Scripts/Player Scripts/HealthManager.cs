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

    private int hearts = 3;

    private bool dead = false;

    private bool gameFinish = false;

    [SerializeField]
    private GameObject[] health = new GameObject[3];//holds the hearts that are in the UI

    void Update()
    {
        if(hearts == 0 && !dead)//checks if the player is dead
        {
            dead = true;
            //player is dead, game over
            Debug.Log("you are dead");
            SceneManager.LoadScene(sceneName: "GameOverScene");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Heart") && hearts < 3)//checksi fthe player has collided with a heart
        {
            GainHeart();
            collision.GetComponent<Heart>().SetHeartUsed(true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Enemy") && !collision.gameObject.GetComponent<EnemyAI>().GetCanHurt())//checks is the player has collided with an enemy
        {
            LoseHeart();
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
}
