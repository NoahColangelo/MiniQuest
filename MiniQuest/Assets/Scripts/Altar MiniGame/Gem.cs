using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    [SerializeField]
    private string colour;//a string to hold the colour type that the gem is

    private bool isInteractable = true;//will make the gem uninteractable when needed

    public bool GetIsInteractable()
    {
        return isInteractable;
    }
    public void SetIsInteractable(bool interactable)
    {
        isInteractable = interactable;
    }

    public string GetColour()
    {
        return colour;
    }
}
