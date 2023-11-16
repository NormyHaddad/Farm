using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayNightCycle : MonoBehaviour
{
    // Start is called before the first frame update
    public int cycleDuration; // Length of the day-night cycle in seconds
    public float timeOfDay;
    public GameObject manager;

    public float sunVanish; // Rotation point where the sun is no longer visible
    public float moonVanish; // Rotation point where the moon is no longer visible

    float degPerSecond;
    void Start()
    {
        degPerSecond = 360 / cycleDuration;
        timeOfDay += 180;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(degPerSecond, 0, 0) * Time.deltaTime);
        timeOfDay += degPerSecond * Time.deltaTime;
        if (timeOfDay >= 360)
        {
            timeOfDay -= 360;
            ChangeSeason();
        }
    }

    void ChangeSeason()
    {
        string oldSeason = manager.GetComponent<GameManager>().season;
        string newSeason = "";
        if (oldSeason == "Summer")
            newSeason = "Autumn";
        if (oldSeason == "Autumn")
            newSeason = "Winter";
        if (oldSeason == "Winter")
            newSeason = "Spring";
        if (oldSeason == "Spring")
            newSeason = "Summer";

        manager.GetComponent<GameManager>().season = newSeason;
        manager.GetComponent<GameManager>().seasonDisplay.GetComponent<Text>().text = newSeason;
    }
}
