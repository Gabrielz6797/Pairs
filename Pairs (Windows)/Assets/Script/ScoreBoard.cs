using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class ScoreBoard : MonoBehaviour
{
    public Text[] scoreText_verbs;
    public Text[] dateText_verbs;
    public Text[] scoreText_fruits;
    public Text[] dateText_fruits;
    public Text[] scoreText_vegetables;
    public Text[] dateText_vegetables;
    public Text[] scoreText_nature;
    public Text[] dateText_nature;
    public Text[] scoreText_animals;
    public Text[] dateText_animals;
    public Text[] scoreText_occupations;
    public Text[] dateText_occupations;
    public Text[] scoreText_vehicles;
    public Text[] dateText_vehicles;
    public Text[] scoreText_body;
    public Text[] dateText_body;

    // Start is called before the first frame update
    void Start()
    {
        updateScoreBoard();
    }

    public void updateScoreBoard()
    {
        Config.updateScoreList();
        displayCategoryScoreData(
            Config.scoreTimeListVerbs,
            Config.verbTimeList,
            scoreText_verbs,
            dateText_verbs
        );
        displayCategoryScoreData(
            Config.scoreTimeListFruits,
            Config.fruitsTimeList,
            scoreText_fruits,
            dateText_fruits
        );
        displayCategoryScoreData(
            Config.scoreTimeListVegetables,
            Config.vegetablesTimeList,
            scoreText_vegetables,
            dateText_vegetables
        );
        displayCategoryScoreData(
            Config.scoreTimeListNature,
            Config.natureTimeList,
            scoreText_nature,
            dateText_nature
        );
        displayCategoryScoreData(
            Config.scoreTimeListAnimals,
            Config.animalsTimeList,
            scoreText_animals,
            dateText_animals
        );
        displayCategoryScoreData(
            Config.scoreTimeListOccupations,
            Config.occupationsTimeList,
            scoreText_occupations,
            dateText_occupations
        );
        displayCategoryScoreData(
            Config.scoreTimeListVehicles,
            Config.vehiclesTimeList,
            scoreText_vehicles,
            dateText_vehicles
        );
        displayCategoryScoreData(
            Config.scoreTimeListBody,
            Config.bodyTimeList,
            scoreText_body,
            dateText_body
        );
    }

    private void displayCategoryScoreData(
        float[] scoreTimelist,
        string[] categoryTimeList,
        Text[] scoreText,
        Text[] dataText
    )
    {
        for (var index = 0; index < 3; index++)
        {
            if (scoreTimelist[index] > 0)
            {
                var dataTime = Regex.Split(categoryTimeList[index], "T");
                var minutes = Mathf.Floor(scoreTimelist[index] / 60);
                float seconds = Mathf.RoundToInt(scoreTimelist[index] % 60);
                scoreText[index].text = minutes.ToString("00") + ":" + seconds.ToString("00");
                dataText[index].text = dataTime[0] + " " + dataTime[1];
            }
            else
            {
                scoreText[index].text = " ";
                dataText[index].text = " ";
            }
        }
    }
}
