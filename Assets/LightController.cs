using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public GameObject dayNightCycle;
    DayNightCycle timeHolder;
    public GameObject light;
    public GameObject lightBulb;
    public Material onMaterial;
    public Material offMaterial;
    void Start()
    {
        //timeHolder = dayNightCycle.GetComponent<DayNightCycle>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(dayNightCycle.GetComponent<DayNightCycle>().timeOfDay);
        if (dayNightCycle.GetComponent<DayNightCycle>().timeOfDay >= 270 || dayNightCycle.GetComponent<DayNightCycle>().timeOfDay <= 90)
        {
            light.SetActive(true);
            lightBulb.GetComponent<MeshRenderer>().material = onMaterial;
        }
        if (dayNightCycle.GetComponent<DayNightCycle>().timeOfDay >= 90 && dayNightCycle.GetComponent<DayNightCycle>().timeOfDay <= 270)
        {
            light.SetActive(false);
            lightBulb.GetComponent<MeshRenderer>().material = offMaterial;
        }
    }
}
