using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarManager : MonoBehaviour
{
    [SerializeField]
    private GemPlacement[] gemPlacements = new GemPlacement[3];//holds the gemPlacements
    [SerializeField]
    private string colour;//holds the colour of the gem the altar manager is managing

    private bool altarComplete = false;

    private int gemPlacedCount = 0;

    // Update is called once per frame
    void Update()
    {
        if (!altarComplete)
        {
            for (int i = 0; i < gemPlacements.Length; i++)//a for loop to check if all gemPlacements have a gem
            {
                if (gemPlacements[i].getGemPlaced())
                    gemPlacedCount++;//counts when a gemPlacement has a gem
            }

            if (gemPlacedCount == 3)//once all gemPlacements are complete the altar will be complete
            {
                altarComplete = true;
            }
            else//else resets gemPlacedCount
                gemPlacedCount = 0;
        }
    }

    public string getColour()
    {
        return colour;
    }
}
