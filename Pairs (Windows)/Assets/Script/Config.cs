using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class Config
{
#if UNITY_EDITOR
    static readonly string Dir = Directory.GetCurrentDirectory();
#elif UNITY_ANDROID
    static readonly string Dir = Application.persistentDataPath;
#else
    static readonly string Dir = Application.persistentDataPath;
#endif
    static readonly string file = @"/PairMatching.ini";
    static readonly string path = Dir + file;
    private const int numberOfScoreRecords = 3;
    public static float[] scoreTimeListVerbs = new float[numberOfScoreRecords];
    public static string[] verbTimeList = new string[numberOfScoreRecords];
    public static float[] scoreTimeListFruits = new float[numberOfScoreRecords];
    public static string[] fruitsTimeList = new string[numberOfScoreRecords];
    public static float[] scoreTimeListVegetables = new float[numberOfScoreRecords];
    public static string[] vegetablesTimeList = new string[numberOfScoreRecords];
    public static float[] scoreTimeListNature = new float[numberOfScoreRecords];
    public static string[] natureTimeList = new string[numberOfScoreRecords];
    public static float[] scoreTimeListAnimals = new float[numberOfScoreRecords];
    public static string[] animalsTimeList = new string[numberOfScoreRecords];
    public static float[] scoreTimeListOccupations = new float[numberOfScoreRecords];
    public static string[] occupationsTimeList = new string[numberOfScoreRecords];
    public static float[] scoreTimeListVehicles = new float[numberOfScoreRecords];
    public static string[] vehiclesTimeList = new string[numberOfScoreRecords];
    public static float[] scoreTimeListBody = new float[numberOfScoreRecords];
    public static string[] bodyTimeList = new string[numberOfScoreRecords];
    private static bool _bestScore = false;

    public static void createScoreFile()
    {
        if (System.IO.File.Exists(path) == false)
        {
            createFile();
        }
        updateScoreList();
    }

    public static void updateScoreList()
    {
        var file = new StreamReader(path);
        updateScoreList(file, scoreTimeListVerbs, verbTimeList);
        updateScoreList(file, scoreTimeListFruits, fruitsTimeList);
        updateScoreList(file, scoreTimeListVegetables, vegetablesTimeList);
        updateScoreList(file, scoreTimeListNature, natureTimeList);
        updateScoreList(file, scoreTimeListAnimals, animalsTimeList);
        updateScoreList(file, scoreTimeListOccupations, occupationsTimeList);
        updateScoreList(file, scoreTimeListVehicles, vehiclesTimeList);
        updateScoreList(file, scoreTimeListBody, bodyTimeList);
        file.Close();
    }

    public static void updateScoreList(StreamReader file, float[] scoreTimeList, string[] timeList)
    {
        if (file == null)
        {
            return;
        }
        var line = file.ReadLine();
        while (line != null && line[0] == '(')
        {
            line = file.ReadLine();
        }
        for (int i = 1; i <= numberOfScoreRecords; i++)
        {
            var word = line.Split('#');
            if (word[0] == i.ToString())
            {
                string[] subString = Regex.Split(word[1], "D");
                if (float.TryParse(subString[0], out var scoreOnPosition))
                {
                    scoreTimeList[i - 1] = scoreOnPosition;
                    if (scoreTimeList[i - 1] > 0)
                    {
                        var dataTime = Regex.Split(subString[1], "T");
                        timeList[i - 1] = dataTime[0] + "T" + dataTime[1];
                    }
                    else
                    {
                        timeList[i - 1] = " ";
                    }
                }
                else
                {
                    scoreTimeList[i - 1] = 0;
                    timeList[i - 1] = " ";
                }
            }
            line = file.ReadLine();
        }
    }

    public static void playScoreOnBoard(float time)
    {
        updateScoreList();
        _bestScore = false;
        switch (GameSettings.Instance.GetPuzzleCategory())
        {
            case GameSettings.EPuzzleCategories.Verbs:
                playScoreOnBoard(time, scoreTimeListVerbs, verbTimeList);
                break;
            case GameSettings.EPuzzleCategories.Fruits:
                playScoreOnBoard(time, scoreTimeListFruits, fruitsTimeList);
                break;
            case GameSettings.EPuzzleCategories.Vegetables:
                playScoreOnBoard(time, scoreTimeListVegetables, vegetablesTimeList);
                break;
            case GameSettings.EPuzzleCategories.Nature:
                playScoreOnBoard(time, scoreTimeListNature, natureTimeList);
                break;
            case GameSettings.EPuzzleCategories.Animals:
                playScoreOnBoard(time, scoreTimeListAnimals, animalsTimeList);
                break;
            case GameSettings.EPuzzleCategories.Occupations:
                playScoreOnBoard(time, scoreTimeListOccupations, occupationsTimeList);
                break;
            case GameSettings.EPuzzleCategories.Vehicles:
                playScoreOnBoard(time, scoreTimeListVehicles, vehiclesTimeList);
                break;
            case GameSettings.EPuzzleCategories.Body:
                playScoreOnBoard(time, scoreTimeListBody, bodyTimeList);
                break;
        }
        saveScoreList();
    }

    private static void playScoreOnBoard(float time, float[] scoreTimeList, string[] timeList)
    {
        var nowTime = System.DateTime.Now.ToString("hh:mm");
        var dataTime = System.DateTime.Now.ToString("MM/dd/yyyy");
        var currentDate = dataTime + "T" + nowTime;
        for (int i = 0; i < numberOfScoreRecords; i++)
        {
            if (scoreTimeList[i] > time || scoreTimeList[i] == 0.0f)
            {
                if (i == 0)
                {
                    _bestScore = true;
                }
                for (var moveDownFrom = numberOfScoreRecords - 1; moveDownFrom > i; moveDownFrom--)
                {
                    scoreTimeList[moveDownFrom] = scoreTimeList[moveDownFrom - 1];
                    timeList[moveDownFrom] = timeList[moveDownFrom - 1];
                }
                scoreTimeList[i] = time;
                timeList[i] = currentDate;
                break;
            }
        }
    }

    public static bool isBestScore()
    {
        return _bestScore;
    }

    public static void createFile()
    {
        saveScoreList();
    }

    public static void saveScoreList()
    {
        System.IO.File.WriteAllText(path, string.Empty);
        var writer = new StreamWriter(path, false);

        writer.WriteLine("(Verbs)");
        for (var i = 1; i <= numberOfScoreRecords; i++)
        {
            var x = scoreTimeListVerbs[i - 1].ToString();
            writer.WriteLine(i.ToString() + "#" + x + "D" + verbTimeList[i - 1]);
        }

        writer.WriteLine("(Fruits)");
        for (var i = 1; i <= numberOfScoreRecords; i++)
        {
            var x = scoreTimeListFruits[i - 1].ToString();
            writer.WriteLine(i.ToString() + "#" + x + "D" + fruitsTimeList[i - 1]);
        }

        writer.WriteLine("(Vegetables)");
        for (var i = 1; i <= numberOfScoreRecords; i++)
        {
            var x = scoreTimeListVegetables[i - 1].ToString();
            writer.WriteLine(i.ToString() + "#" + x + "D" + vegetablesTimeList[i - 1]);
        }

        writer.WriteLine("(Nature)");
        for (var i = 1; i <= numberOfScoreRecords; i++)
        {
            var x = scoreTimeListNature[i - 1].ToString();
            writer.WriteLine(i.ToString() + "#" + x + "D" + natureTimeList[i - 1]);
        }

        writer.WriteLine("(Animals)");
        for (var i = 1; i <= numberOfScoreRecords; i++)
        {
            var x = scoreTimeListAnimals[i - 1].ToString();
            writer.WriteLine(i.ToString() + "#" + x + "D" + animalsTimeList[i - 1]);
        }

        writer.WriteLine("(Occupations)");
        for (var i = 1; i <= numberOfScoreRecords; i++)
        {
            var x = scoreTimeListOccupations[i - 1].ToString();
            writer.WriteLine(i.ToString() + "#" + x + "D" + occupationsTimeList[i - 1]);
        }

        writer.WriteLine("(Vehicles)");
        for (var i = 1; i <= numberOfScoreRecords; i++)
        {
            var x = scoreTimeListVehicles[i - 1].ToString();
            writer.WriteLine(i.ToString() + "#" + x + "D" + vehiclesTimeList[i - 1]);
        }

        writer.WriteLine("(Body)");
        for (var i = 1; i <= numberOfScoreRecords; i++)
        {
            var x = scoreTimeListBody[i - 1].ToString();
            writer.WriteLine(i.ToString() + "#" + x + "D" + bodyTimeList[i - 1]);
        }
        
        writer.Close();
    }
}
