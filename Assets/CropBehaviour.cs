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
                cropToPlant = Instantiate(youngPlant, transform.position, Quaternion.identity);
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
                cropToPlant = Instantiate(maturePlant, transform.position, Quaternion.identity);
                cropToPlant.transform.SetParent(gameObject.transform);
                plotState = states.HARVESTABLE;
            }
        }
    }

    private void OnMouseDown()
    {
        if (cropToPlant != null)
            print(cropToPlant.name);

        print(plotState);
        if (plotState == states.READY && gameManager.GetComponent<GameManager>().currentCrop != null)
        {
            plantedCrop = gameManager.GetComponent<GameManager>().currentCrop;
            gameManager.GetComponent<GameManager>().UpdateCoins(plantedCrop.GetComponent<CropClass>().cost * -1);

            plantSeeds = plantedCrop.stage0;
            youngPlant = plantedCrop.stage1;
            maturePlant = plantedCrop.stage2;
            growTime = plantedCrop.timeToGrow;
            cropToPlant = Instantiate(plantSeeds, transform.position + cropLocation, Quaternion.identity);
            cropToPlant.transform.SetParent(gameObject.transform);
            plotState = states.PLANTED;
        }

        if (plotState == states.READY && gameManager.GetComponent<GameManager>().currentCrop == null)
        {
            isWatered = true;
            soil.GetComponent<MeshRenderer>().material = wetMaterial;
        }

        if (plotState == states.MESSY)
        {
            plotState = states.READY;
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
