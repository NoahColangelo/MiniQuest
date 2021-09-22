using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{

    private bool transitionTriggered = false;

    private bool startMidTransition = false;// only used for when the player is going back to the middle area

    private float waitOnMidTrigger = 0.2f;//slight delay before transition so player is not cut off of screen
    private float timer = 0.0f;


    private void Update()
    {
        delayTransitionToMid(Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)//check if player has entered the triggerbox to transition the camera
    {

        if (collision.transform.tag == "Player" && !transitionTriggered) 
        {
            transitionTriggered = true;
        }
        else if (collision.transform.tag == "Player" && transitionTriggered)
        {
            startMidTransition = true;
        }

    }

    private void delayTransitionToMid(float dt)//the timer that delays the mid transition for the camera
    {
        if (startMidTransition && timer < waitOnMidTrigger)
        {
            timer += dt;
        }
        else if (timer > waitOnMidTrigger)
        {
            timer = 0.0f;
            startMidTransition = false;

            transitionTriggered = false;
        }
    }

    public bool getTransitionTriggered()
    {
        return transitionTriggered;
    }
}
