using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    [SerializeField]
    private AltarManager[] altars = new AltarManager[4];//holds the altars
    [SerializeField]
    private GameObject door;//holds the door tilemap to be disabled when all altars are complete

    [SerializeField]
    private GameObject redGem;
    [SerializeField]
    private GameObject pinkGem;
    [SerializeField]
    private GameObject greenGem;
    [SerializeField]
    private GameObject blueGem;

    private bool doorOpen = false;

    private int altarsComplete = 0;

    void Update()
    {
        if (!doorOpen)
        {
            for (int i = 0; i < altars.Length; i++)//a for loop to check if all altars have been complete
            {
                if (altars[i].GetAltarComplete())
                {
                    ActivateColour(altars[i].GetColour());//sets the colour of the gem to show altar is complete
                    altarsComplete++;//counts when an altar is complete
                }
            }

            if (altarsComplete == 3)//once all altars are complete the door will open
            {
                doorOpen = true;
                door.SetActive(false);
            }
            else//else resets altarsCompleteCount
                altarsComplete = 0;
        }
    }

    void ActivateColour(string colour)
    {
        if(colour == "Red")
        {
            redGem.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else if(colour == "Pink")
        {
            pinkGem.GetComponent<SpriteRenderer>().color = Color.white;//white because the gem is originally pink
        }
        else if (colour == "Green")
        {
            greenGem.GetComponent<SpriteRenderer>().color = Color.green;
        }
        else if (colour == "Blue")
        {
            blueGem.GetComponent<SpriteRenderer>().color = Color.blue;
        }
    }
}
