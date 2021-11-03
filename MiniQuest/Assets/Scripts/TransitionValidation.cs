using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionValidation : MonoBehaviour
{
    private bool transitionStarted = false;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player") && !transitionStarted)
        {
            transitionStarted = true;
        }
    }

    public bool GetTransitionStarted()
    {
        return transitionStarted;
    }

    public void setTransitionStarted( bool set)
    {
        transitionStarted = set;
    }
}
