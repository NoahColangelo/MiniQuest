using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameOverScene : MonoBehaviour
{
    private InputManager inputManager;

    private void Awake()
    {
        inputManager = new InputManager();

        inputManager.UI.Enable();//enables the UI actionmap for the inputmanager
    }

    public void OnClickTryAgain()
    {
        inputManager.UI.Disable();
        SceneManager.LoadScene(sceneName: "GameScene");//sends the player back to the game scene if they lose
    }

    public void OnClickExit()
    {
        inputManager.UI.Disable();
        SceneManager.LoadScene(sceneName: "OpeningScene");//send the player back to the OpeningScene
    }
}
