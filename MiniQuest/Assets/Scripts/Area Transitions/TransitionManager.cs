using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    private Transform playerPos;//holds players position
    
    //the gameobjects that hold the transform the camera goes to for each transition
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

    private Transform newCameraPos;//holds the new position the camera will go to

    [SerializeField]
    private float interpolateDuration = 1.5f;//time it takes for the camera to move between positions

    private float tValue = 0.0f;

    private bool isTransitioning = false;// for when the camera is moving to its next position

    private char playerCurrentArea = 'M';

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        currentCameraPos = middleCameraPos;

        playerPos = FindObjectOfType<PlayerControls>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTransitioning)
            newCameraPos = CheckTransitionPoints();
          
        CameraPosChange(newCameraPos, Time.unscaledDeltaTime);            
    }

    Transform CheckTransitionPoints()
    {
        Vector3 viewPos = camera.WorldToViewportPoint(playerPos.position);//gets the players viewport position to check when they go off screen to transition camera positions

        if (viewPos.y > 1.0f)// for going to the top area
        {
            if (playerCurrentArea == 'M')
            {
                playerCurrentArea = 'U';
                return upCameraPos;
            }
            else//going back to mid from top
            {
                playerCurrentArea = 'M';
                return middleCameraPos;
            }
        }
        else if (viewPos.x < -0.0f)//for going to the left area
        {
            if (playerCurrentArea == 'M')
            {
                playerCurrentArea = 'L';
                return leftCameraPos;
            }
            else//going back to mid from left
            {
                playerCurrentArea = 'M';
                return middleCameraPos;
            }
        }
        else if (viewPos.y < -0.0f)//for going to the bottom area
        {
            if (playerCurrentArea == 'M')
            {
                playerCurrentArea = 'B';
                return downCameraPos;
            }
            else//going back to mid from bottom
            {
                playerCurrentArea = 'M';
                return middleCameraPos;
            }
        }
        else if (viewPos.x > 1.0f)// for going to the right area
        {
            if (playerCurrentArea == 'M')
            {
                playerCurrentArea = 'R';
                return rightCameraPos;
            }
            else//going back to mid from right
            {
                playerCurrentArea = 'M';
                return middleCameraPos;
            }
        }
        else
        {
            return currentCameraPos;
        }
    }

    void CameraPosChange(Transform newPos, float dt)//interpolates the camera to its different positions
    {
        //checks if the transition has ended and if the camera is already in the interpolated position

        if (tValue < interpolateDuration && currentCameraPos.position != newPos.position)//during interpolation
        {
            isTransitioning = true;

            camera.transform.position = Vector3.Lerp(currentCameraPos.position, newPos.position, tValue / interpolateDuration);//the interpolation equation

            tValue += dt;//increase t value with deltatime
            Time.timeScale = 0;//time scale will freeze animations and movement of player and enemies
        }
        else if (tValue >= interpolateDuration)//when done interpolating
        {
            tValue = 0.0f;//resets the t value

            camera.transform.position = newPos.position;//snaps camera to position cause lerp wont be exact
            currentCameraPos = newPos;//sets the current camera pos to the new one it is on

            isTransitioning = false;
            Time.timeScale = 1;
        }
    }
    public bool GetIsTransitioning()
    {
        return isTransitioning;
    }

    public char GetPlayerCurrentArea()
    {
        return playerCurrentArea;
    }
}
