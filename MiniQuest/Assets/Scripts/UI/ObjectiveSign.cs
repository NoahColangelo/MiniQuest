using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveSign : MonoBehaviour
{
    [SerializeField]
    private GameObject objectiveBlip;
    [SerializeField]
    private GameObject questionMark;

    private PlayerControls player;

    private bool playerNear = false;
    private bool objectiveDoneBeingRead = false;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerControls>();

        objectiveBlip.SetActive(true);
        questionMark.SetActive(false);

        Time.timeScale = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //this if allows the objective blip to show up when the game first starts and
        //then can be deactivated by pressing the interact sign
        if(!objectiveDoneBeingRead && player.GetReadSign())
        {
            objectiveDoneBeingRead = true;
            objectiveBlip.SetActive(false);
            Time.timeScale = 1.0f;
        }
            
        //this is for when the player is near the sign and wants to read the objective statement again
        if(playerNear)
        {
            questionMark.SetActive(true);

            if(player.GetReadSign())
            {
                objectiveBlip.SetActive(true);
            }
            else if (!player.GetReadSign())
            {
                objectiveBlip.SetActive(false);
            }
        }
        else
            questionMark.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerNear = true;
            if (player.GetReadSign())//sets it to false so the blip will not come up immediately when player goes near it without intent on looking
                player.setReadSign(false);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //when the player leaves the trigger box for the sign it will deactivate the blip
        //and stop the player from reading the sign
        if (collision.CompareTag("Player"))
        {
            playerNear = false;
            objectiveBlip.SetActive(false);
            player.setReadSign(false);
        }
    }
}
