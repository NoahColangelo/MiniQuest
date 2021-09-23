using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{

    private bool transitionTriggered = false;

    private bool startTransitionDelay = false;// only used for when the player is going back to the middle area

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
            //transitionTriggered = true;

            startTransitionDelay = true;
        }
        else if (collision.transform.tag == "Player" && transitionTriggered)
        {
            startTransitionDelay = true;
        }

    }

    private void delayTransitionToMid(float dt)//the timer that delays the mid transition for the camera
    {
        if (startTransitionDelay && timer < waitOnMidTrigger)
        {
            timer += dt;
        }
        else if (timer > waitOnMidTrigger)//once timer has finished
        {
            timer = 0.0f;
            startTransitionDelay = false;

            if (transitionTriggered)//allows for the delay to work going to and from the mid scene to help combat the camera messing up
                transitionTriggered = false;
            else
                transitionTriggered = true;
        }
    }

    public bool getTransitionTriggered()
    {
        return transitionTriggered;
    }
}