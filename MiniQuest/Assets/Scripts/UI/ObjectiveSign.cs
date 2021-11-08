using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveSign : MonoBehaviour
{
    private bool playerNear = false;

    [SerializeField]
    private GameObject objectiveBlip;
    [SerializeField]
    private GameObject questionMark;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        objectiveBlip.SetActive(false);
        questionMark.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(playerNear)
        {
            questionMark.SetActive(true);

            if(player.GetComponent<PlayerControls>().GetReadSign())
            {
                objectiveBlip.SetActive(true);
            }
            else if (!player.GetComponent<PlayerControls>().GetReadSign())
            {
                objectiveBlip.SetActive(false);
            }
        }
        else
        {
            questionMark.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerNear = true;

            if (player == null)
                player = collision.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNear = false;
            objectiveBlip.SetActive(false);
            player.GetComponent<PlayerControls>().setReadSign(false);
        }
    }
}
