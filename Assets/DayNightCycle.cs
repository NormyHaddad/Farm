using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    // Start is called before the first frame update
    public int cycleDuration; // Length of the day-night cycle in seconds

    public float sunVanish; // Rotation point where the sun is no longer visible
    public float moonVanish; // Rotation point where the moon is no longer visible

    float degPerSecond;
    void Start()
    {
        degPerSecond = 360 / cycleDuration;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(degPerSecond, 0, 0) * Time.deltaTime);
    }
}
