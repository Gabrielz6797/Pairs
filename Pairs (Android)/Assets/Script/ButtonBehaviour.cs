using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBehaviour : MonoBehaviour
{
    void Start()
    {
        Config.createScoreFile();
    }

    public void loadScene(string scene_name)
    {
        SceneManager.LoadScene(scene_name);
    }

    public void resetGameSettings()
    {
        GameSettings.Instance.ResetGameSettings();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
