using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class OpeningScene : MonoBehaviour
{
    private InputManager inputManager;

    [SerializeField]
    private GameObject howToPlayBlip;
    [SerializeField]
    private GameObject optionsBlip;

    [SerializeField]
    private GameObject startButton;
    [SerializeField]
    private GameObject howToPlayButton;
    [SerializeField]
    private GameObject optionsButton;
    [SerializeField]
    private GameObject quitButton;

    [SerializeField]
    private Slider bgmSlider;
    [SerializeField]
    private Button leaveHTP;

    private void Awake()
    {
        howToPlayBlip.SetActive(false);
        optionsBlip.SetActive(false);

        inputManager = new InputManager();

        inputManager.UI.Enable();//enables the UI actionmap for the inputmanager
    }

    public void OnClickStart()
    {
        inputManager.UI.Disable();
        SceneManager.LoadScene(sceneName:"GameScene");//sends player to GameScene where the main game is
    }

    public void OnClickHowToPlay()//when the player clicks the how to play button
    {
        if (!howToPlayBlip.activeInHierarchy)
        {
            howToPlayBlip.SetActive(true);

            startButton.SetActive(false);
            quitButton.SetActive(false);
            optionsButton.SetActive(false);
            howToPlayButton.SetActive(false);

            leaveHTP.Select();//sets the leave how to play button to select for non mouse users
        }
        else
        {
            howToPlayBlip.SetActive(false);

            startButton.SetActive(true);
            quitButton.SetActive(true);
            optionsButton.SetActive(true);
            howToPlayButton.SetActive(true);

            howToPlayButton.GetComponent<Button>().Select();//sets the how to play button to select for non mouse users
        }
    }

    public void OnClickOptions()
    {
        if (!optionsBlip.activeInHierarchy)
        {
            optionsBlip.SetActive(true);

            startButton.SetActive(false);
            quitButton.SetActive(false);
            optionsButton.SetActive(false);
            howToPlayButton.SetActive(false);

            bgmSlider.Select();
        }
        else
        {
            optionsBlip.SetActive(false);

            startButton.SetActive(true);
            quitButton.SetActive(true);
            optionsButton.SetActive(true);
            howToPlayButton.SetActive(true);

            optionsButton.GetComponent<Button>().Select();
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
