using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    //the transition gameobjects at each transition point
    [SerializeField]
    private Transition upTransition;
    [SerializeField]
    private Transition leftTransition;
    [SerializeField]
    private Transition downTransition;
    [SerializeField]
    private Transition rightTransition;

    //the gameobjects that hold the ransform the camera goes to for each transition
    [SerializeField]
    private Transform upCameraPos;
    [SerializeField]
    private Transform leftCameraPos;
    [SerializeField]
    private Transform downCameraPos;
    [SerializeField]
    private Transform rightCameraPos;
    [SerializeField]
    private Transform middleCameraPos;

    private Camera camera;
    private Transform currentCameraPos;//holds the current position of the camera

    [SerializeField]
    private float interpolateDuration = 1.5f;//time it takes for the camera to move between positions

    private float tValue = 0.0f;

    //bools to be used for later AI use
    private bool playerIsUp = false;
    private bool playerIsLeft = false;
    private bool playerIsDown = false;
    private bool playerIsRight = false;

    private bool isTransitioning = false;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        currentCameraPos = middleCameraPos;
    }

    // Update is called once per frame
    void Update()
    {
        cameraPosChange(checkTransitionTriggers(), Time.deltaTime);
    }

    Transform checkTransitionTriggers()//function checks if any of the transition triggers have been tripped and returns correct position for camera to lerp to
    {
        if(upTransition.getTransitionTriggered())
        {
            playerIsUp = true;
            return upCameraPos;
        }
        else if(leftTransition.getTransitionTriggered())
        {
            playerIsLeft = true;
            return leftCameraPos;
        }
        else if (downTransition.getTransitionTriggered())
        {
            playerIsDown = true;
            return downCameraPos;
        }
        else if (rightTransition.getTransitionTriggered())
        {
            playerIsRight = true;
            return rightCameraPos;
        }
        else//this would be for the middle camera position a no other bools are true
        {
            playerIsUp = false;
            playerIsLeft = false;
            playerIsDown = false;
            playerIsRight = false;
            return middleCameraPos;
        }
    }

    void cameraPosChange(Transform newPos, float dt)//interpolates the camera to its different positions
    {
        //checks if the transition has ended and if the camera is already in the interpolated position
        if (tValue < interpolateDuration && currentCameraPos.position != newPos.position)
        {
            isTransitioning = true;
            camera.transform.position = Vector3.Lerp(currentCameraPos.position, newPos.position, tValue / interpolateDuration);//the interpolation equation
            tValue += dt;//increase t value with deltatime
        }
        else if (tValue >= interpolateDuration)
        {
            tValue = 0.0f;//resets the t value
            camera.transform.position = newPos.position;//snaps camera to position cause lerp wont be exact
            currentCameraPos = newPos;//sets the current camera pos to the new one it is on
            isTransitioning = false;
        }
    }

    public bool getPlayerisUp()
    {
        return playerIsUp;
    }

    public bool getPlayerisLeft()
    {
        return playerIsLeft;
    }

    public bool getPlayerisDown()
    {
        return playerIsDown;
    }

    public bool getPlayerisRight()
    {
        return playerIsRight;
    }

    public bool getIsTransitioning()
    {
        return isTransitioning;
    }
}
