using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameSettings : MonoBehaviour
{
    private readonly Dictionary<EPuzzleCategories, string> _puzzleCatDirectory =
        new Dictionary<EPuzzleCategories, string>();
    private int _settings;
    private const int _settingsNumber = 1;
    private bool _muteFxPermanently = false;

    public enum EPuzzleCategories
    {
        NotSet,
        Verbs,
        Fruits,
        Vegetables,
        Nature,
        Animals,
        Occupations,
        Empty
    }

    public struct Settings
    {
        public EPuzzleCategories PuzzleCategory;
    };

    private Settings _gameSettings;

    public static GameSettings Instance;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        SetPuzzleCatDirectory();
        _gameSettings = new Settings();
        ResetGameSettings();
    }

    private void SetPuzzleCatDirectory()
    {
        _puzzleCatDirectory.Add(EPuzzleCategories.Verbs, "Verbs");
        _puzzleCatDirectory.Add(EPuzzleCategories.Fruits, "Fruits");
        _puzzleCatDirectory.Add(EPuzzleCategories.Vegetables, "Vegetables");
        _puzzleCatDirectory.Add(EPuzzleCategories.Nature, "Nature");
        _puzzleCatDirectory.Add(EPuzzleCategories.Animals, "Animals");
        _puzzleCatDirectory.Add(EPuzzleCategories.Occupations, "Occupations");
        _puzzleCatDirectory.Add(EPuzzleCategories.Empty, "Empty");
    }

    public void setPuzzleCategories(EPuzzleCategories cat)
    {
        if (_gameSettings.PuzzleCategory == EPuzzleCategories.NotSet)
        {
            _settings++;
        }
        _gameSettings.PuzzleCategory = cat;
    }

    public EPuzzleCategories GetPuzzleCategory()
    {
        return _gameSettings.PuzzleCategory;
    }

    public void ResetGameSettings()
    {
        _settings = 0;
        _gameSettings.PuzzleCategory = EPuzzleCategories.NotSet;
    }

    public bool AllSettingsReady()
    {
        return _settings == _settingsNumber;
    }

    public string GetMaterialDirectoryName()
    {
        return "Materials/";
    }

    public string GetPuzzleCategoryTextureDirectoryName()
    {
        if (_puzzleCatDirectory.ContainsKey(_gameSettings.PuzzleCategory))
        {
            return "Graphics/PuzzleCat/" + _puzzleCatDirectory[_gameSettings.PuzzleCategory] + "/";
        }
        else
        {
            Debug.LogError("ERROR: CANNOT GET THE DIRECTORY NAME");
            return "";
        }
    }

    public void muteSoundEffectPermanently(bool mute)
    {
        _muteFxPermanently = mute;
    }

    public bool isSoundEffectMutedPermanently()
    {
        return _muteFxPermanently;
    }
}
