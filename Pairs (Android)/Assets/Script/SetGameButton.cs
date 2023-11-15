using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetGameButton : MonoBehaviour
{
    public enum EButtonType
    {
        NotSet,
        PuzzleCategoryButton,
    };

    [SerializeField]
    public EButtonType ButtonType = EButtonType.NotSet;

    [HideInInspector]
    public GameSettings.EPuzzleCategories puzzleCategories = GameSettings.EPuzzleCategories.NotSet;

    void Start() { }

    public void SetGameOption(string GameSceneName)
    {
        var comp = gameObject.GetComponent<SetGameButton>();
        GameSettings.Instance.setPuzzleCategories(comp.puzzleCategories);
        if (GameSettings.Instance.AllSettingsReady())
        {
            SceneManager.LoadScene(GameSceneName);
        }
    }
}
