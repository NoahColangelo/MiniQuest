using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemPlacement : MonoBehaviour
{
    private AltarManager altarManager;

    private bool gemPlaced = false;

    private GameObject gem;//holds the gems info when it is in the triggerbox of the gemPlacement
    private bool gemNear = false;

    private void Start()
    {
        altarManager = transform.GetComponentInParent<AltarManager>();//gets the altarmanager from the parent
    }

    private void Update()
    {
        if(gemNear && !gemPlaced && gem != null)
        {
            if (gem.transform.parent == false)//once gem is placed down from player the gem will be moved to the altar position and gemPlaced becomes true
            {
                gemPlaced = true;
                gem.transform.position = transform.position;
                gem.GetComponent<Gem>().setIsInteractable(false);// the gem is no longer interactable
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //checks to make sure the gem is the right colour and no other gem is placed
        if (collision.tag == "Gem" && !gemPlaced && collision.GetComponent<Gem>().getColour() == altarManager.getColour())
        {
            gemNear = true;
            gem = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //sets gem to null so it does not have an effect on the gem when moved away from the triggerbox
        if (collision.tag == "Gem" && !gemPlaced && collision.GetComponent<Gem>().getColour() == altarManager.getColour())
        {
            gemNear = false;
            gem = null;
        }
    }

    public bool getGemPlaced()
    {
        return gemPlaced;
    }
}
