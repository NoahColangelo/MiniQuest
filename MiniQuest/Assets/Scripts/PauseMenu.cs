using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;

    private bool gamePaused = false;

    void Start()
    {
        pauseMenu.SetActive(false);
    }

    public void pauseGame()//pauses and unpauses the game
    {
        if (!gamePaused)
        {
            Time.timeScale = 0;//setting time scale to 0 stops most function in the game to act as pausing it
            gamePaused = true;
            pauseMenu.SetActive(true);//sets the pause menu to be active
        }
        else
        {
            Time.timeScale = 1;//unpauses the game when timescale set to 1
            gamePaused = false;
            pauseMenu.SetActive(false);//hides the pause menu
        }
    }

    public void exitGame()
    {
        #if UNITY_EDITOR//stops the game from playing in the editor
            UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();//closes the game if it were in an appilcation form
    }
}
