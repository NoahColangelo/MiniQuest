using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    [SerializeField]
    private TransitionValidation transitionValidationMid;
    [SerializeField]
    private TransitionValidation transitionValidationArea;

    private bool transitionTriggered = false;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player") && !transitionTriggered && transitionValidationMid.GetTransitionStarted())//going to another area from mid
        {
            transitionTriggered = true;
            transitionValidationMid.setTransitionStarted(false);
        }
        else if (collision.transform.CompareTag("Player") && transitionTriggered && transitionValidationArea.GetTransitionStarted())//coming back from the area to mid
        {
            transitionTriggered = false;
            transitionValidationArea.setTransitionStarted(false);
        }
    }

    public bool GetTransitionTriggered()
    {
        return transitionTriggered;
    }
}
