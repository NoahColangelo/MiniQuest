using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    [SerializeField]
    private string colour;//a string to hold the colour type that the gem is

    private bool isInteractable = true;//will make the gem uninteractable when needed

    public bool getIsInteractable()
    {
        return isInteractable;
    }
    public void setIsInteractable(bool interactable)
    {
        isInteractable = interactable;
    }

    public string getColour()
    {
        return colour;
    }
}
