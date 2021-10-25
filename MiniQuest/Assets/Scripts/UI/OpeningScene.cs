using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class OpeningScene : MonoBehaviour
{
    private InputManager inputManager;

    [SerializeField]
    private GameObject HowToPlay;
    [SerializeField]
    private GameObject StartButton;
    [SerializeField]
    private GameObject QuitButton;

    private void Awake()
    {
        HowToPlay.SetActive(false);

        inputManager = new InputManager();

        inputManager.UI.Enable();//enables the UI actionmap for the inputmanager
    }

    public void OnClickStart()
    {
        inputManager.UI.Disable();
        SceneManager.LoadScene(sceneName:"GameScene");//sends player to GameScene where the main game is
    }

    public void OnClickHowToPlay()
    {
        if (!HowToPlay.activeInHierarchy)
        {
            HowToPlay.SetActive(true);
            StartButton.SetActive(false);
            QuitButton.SetActive(false);
        }
        else
        {
            HowToPlay.SetActive(false);
            StartButton.SetActive(true);
            QuitButton.SetActive(true);
        }
    }

    public void OnClickExit()
    {
        inputManager.UI.Disable();

        #if UNITY_EDITOR//stops the game from playing in the editor
            UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();//closes the game if it were in an appilcation form
    }
}
