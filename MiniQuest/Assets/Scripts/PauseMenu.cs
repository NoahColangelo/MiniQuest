using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private PlayerControls playerControls;

    private bool gamePaused = false;

    private bool delayComplete = false;
    private float timer = 0.0f;
    private float delayDuration = 0.1f;

    void Start()
    {
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if(gamePaused)
            actionMapDelay(Time.unscaledDeltaTime);
    }

    public void pauseGame()//pauses and unpauses the game
    {
        if (!gamePaused)
        {
            Time.timeScale = 0;//setting time scale to 0 stops most function in the game to act as pausing it
            gamePaused = true;
            pauseMenu.SetActive(true);//sets the pause menu to be active

            playerControls.getInputManager().PlayerController.Disable();//switches to the UI ActionMap when using the UI
            playerControls.getInputManager().UI.Enable();
        }
        else if (gamePaused && delayComplete)
        {
            Time.timeScale = 1;//unpauses the game when timescale set to 1
            gamePaused = false;
            pauseMenu.SetActive(false);//hides the pause menu
            delayComplete = false;

            playerControls.getInputManager().UI.Disable();//switches to the PlayerController ActionMap when leaving the UI
            playerControls.getInputManager().PlayerController.Enable();
        }
    }

    public void exitGame()
    {
        #if UNITY_EDITOR//stops the game from playing in the editor
            UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();//closes the game if it were in an appilcation form
    }

    private void actionMapDelay(float udt)
    {
        if(timer > delayDuration)
        {
            timer += udt;
        }
        else
        {
            delayComplete = true;
            timer = 0.0f;
        }
    }
}
