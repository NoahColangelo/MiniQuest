using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class OpeningScene : MonoBehaviour
{
    private InputManager inputManager;

    private void Awake()
    {
        inputManager = new InputManager();

        inputManager.UI.Enable();//enables the UI actionmap for the inputmanager
    }

    public void OnClickStart()
    {
        inputManager.UI.Disable();
        SceneManager.LoadScene(sceneName:"GameScene");//sends player to GameScene where the main game is
    }

    public void OnClickOptions()
    {
        Debug.Log("options clicked");
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
