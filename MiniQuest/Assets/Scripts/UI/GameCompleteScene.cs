using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameCompleteScene : MonoBehaviour
{
    private InputManager inputManager;

    private void Awake()
    {
        inputManager = new InputManager();

        inputManager.UI.Enable();//enables the UI actionmap for the inputmanager
    }

    public void OnClickBackToMain()
    {
        inputManager.UI.Disable();
        SceneManager.LoadScene(sceneName: "OpeningScene");//sends player to GameScene where the main game is
    }
}
