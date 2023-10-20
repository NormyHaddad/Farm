using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropBehaviour : MonoBehaviour
{
    public GameObject gameManager;
    public float growTime;
    public bool isWatered;

    // Materials
    public Material messyMaterial;
    public Material readyMaterial;
    public Material wetMaterial;
    
    public Vector3 cropLocation = new Vector3(0, 0.05f, 0);
    
    // Plant stages
    public GameObject plantSeeds;
    public GameObject youngPlant;
    public GameObject maturePlant;
    public CropClass plantedCrop;

    public GameObject cropToPlant;
    public GameObject soil;

    public ParticleSystem waterSpray;


    enum states
    {
        MESSY = 0,
        READY = 1,
        PLANTED = 2,
        GROWING = 3,
        HARVESTABLE = 4
    };
    states plotState = states.READY;
    float timer;
    bool clicked = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (plotState == states.PLANTED)//if plant is freshly planted
        {
            timer += Time.deltaTime;
            if (timer * 2 >= growTime)//if its partly grown
            {
                Destroy(cropToPlant);
                cropToPlant = Instantiate(youngPlant, transform.position + cropLocation, Quaternion.identity);
                plotState = states.GROWING;
            }
        }
        if (plotState == states.GROWING)
        {
            timer += Time.deltaTime;

            if (isWatered) // If the plot is watered, the plant grows faster
                timer += Time.deltaTime / 2;

            if (timer >= growTime)//if its fully grown
            {
                Destroy(cropToPlant);
                cropToPlant = Instantiate(maturePlant, transform.position + cropLocation, Quaternion.identity);
                cropToPlant.transform.SetParent(gameObject.transform);
                plotState = states.HARVESTABLE;
            }
        }
    }

    private void OnMouseDown()
    {
        Debug.Log(gameManager.GetComponent<GameManager>().placeCropMode);

        if (cropToPlant != null)
            print(cropToPlant.name);

        if (plotState == states.READY && gameManager.GetComponent<GameManager>().currentCrop != null)
        {
            plantedCrop = gameManager.GetComponent<GameManager>().currentCrop;
            gameManager.GetComponent<GameManager>().UpdateCoins(plantedCrop.GetComponent<CropClass>().cost * -1);

            plantSeeds = plantedCrop.stage0;
            youngPlant = plantedCrop.stage1;
            youngPlant = plantedCrop.stage1;
            maturePlant = plantedCrop.stage2;
            growTime = plantedCrop.timeToGrow;
            cropToPlant = Instantiate(plantSeeds, transform.position + cropLocation, Quaternion.identity);
            cropToPlant.transform.SetParent(gameObject.transform);
            plotState = states.PLANTED;
        }

        // If the plot is either ready, has seeds planted, or has plants growing, and if it is not already watered
        if ((plotState == states.READY || plotState == states.PLANTED || plotState == states.GROWING)
            && gameManager.GetComponent<GameManager>().placeCropMode == false && !isWatered)
        {
            isWatered = true;
            soil.GetComponent<MeshRenderer>().material = wetMaterial;
            waterSpray.Play();
        }

        if (plotState == states.MESSY)
        {
            plotState = states.READY;
            isWatered = false;
            soil.GetComponent<MeshRenderer>().material = readyMaterial;
        }

        if (plotState == states.HARVESTABLE)
        {
            plotState = states.MESSY;
            timer = 0;
            soil.GetComponent<MeshRenderer>().material = messyMaterial;
            gameManager.GetComponent<GameManager>().UpdateCoins(plantedCrop.GetComponent<CropClass>().value);
            Destroy(cropToPlant);
        }
    }
}
