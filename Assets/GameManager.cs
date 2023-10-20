using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Camera cam;
    public int Coins;
    public int Cost;
    public string season;

    // Bools
    public bool placeObjMode;
    public bool placeCropMode;
    public bool placeTreeMode;
    bool objInstantiated;
    bool tooltipVisible;

    // Screens
    public GameObject cropSelectorScreen;
    public GameObject gameOverlayScreen;
    public GameObject placeObjScreen;
    public GameObject currentScreen;
    List<GameObject> previousScreens;

    public GameObject plotObj;

    // UI
    public GameObject coinDisplay;
    public GameObject seasonDisplay;
    public GameObject tooltip;
    public GameObject errorMsg;

    public CropClass currentCrop;
    public GameObject currentObj;
    TreeBehaviour currentTree;
    public GameObject objClone;
    public TreeBehaviour treeClone;
    public TreeBehaviour tree;
    public List<CropClass> cropList;
    public Material ghostGreen;

    // Start is called before the first frame update
    void Start()
    {
        currentObj = null;
        previousScreens = new List<GameObject> { };
        currentScreen = gameOverlayScreen;
        cropSelectorScreen.SetActive(false);
        placeObjScreen.SetActive(false);
        currentCrop = null;
        placeObjMode = false;
        objInstantiated = false;
        tooltipVisible = false;

        //seasonDisplay.GetComponent<TextMeshProUGUI>().text = season;
    }

    // Update is called once per frame
    void Update()
    {
        // If the tooltip is visible, make it follow the mouse
        if (tooltipVisible)
        {
            tooltip.transform.position = Input.mousePosition;
        }

        if (Input.GetKeyDown(KeyCode.X) && !gameOverlayScreen.activeSelf)
        {
            CloseScreen();
            currentCrop = null;
            placeObjMode = false;
            placeCropMode = false;
            //Destroy(objClone);
        }
        if (placeObjMode)
        {
            LayerMask mask = LayerMask.GetMask("Ground");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, 999.0f, mask);
            Vector3 pos2 = new Vector3((float)Math.Round(2 * hit.point.x) / 2, 0, (float)Math.Round(2 * hit.point.z) / 2);

            if (!objInstantiated)
            {
                objClone = Instantiate(currentObj, pos2, Quaternion.identity);
                objInstantiated = true;
            }

            objClone.transform.position = pos2;

            if (Input.GetMouseButtonDown(0) && objInstantiated)
            {
                Destroy(objClone);
                objClone = Instantiate(currentObj, pos2, Quaternion.identity);
                if (objClone.GetComponent<CropBehaviour>().gameManager == null) // temporary
                    { objClone.GetComponent<CropBehaviour>().gameManager = gameObject; }
                placeObjMode = false;
                objInstantiated = false;
            }
        }
    }

    public void ShowCropSelector()
    {
        ChangeToScreen(cropSelectorScreen);
    }

    public void ReturnCrop(CropClass crop)
    {
        currentCrop = crop;
        //cropSelectorScreen.SetActive(false);
        placeCropMode = true;
        ChangeToScreen(placeObjScreen);
    }
    
    public void PlaceObjModeNew(GameObject objToPlace)
    {
        if (Coins >= objToPlace.GetComponent<buildableObj>().cost)
        {
            currentCrop = null;
            currentObj = objToPlace;
            //CloseScreen();
            ChangeToScreen(placeObjScreen);
            placeObjMode = true;
        }
        else
            StartCoroutine(ShowErrorMsg("Not enough coins", 3));
    }

    public void CheckSeason(CropClass crop)
    {
        bool validSeason = false;
        List<string> seasonsList = crop.GetComponent<CropClass>().seasons;
        if (seasonsList.Contains("all"))
        {
            ReturnCrop(crop.GetComponent<CropClass>());
        }
        else
        {
            foreach (string str in seasonsList)
            {
                if (str == season)
                {
                    validSeason = true;
                    Debug.Log("correct season");
                }
            }
        }

        if (validSeason)
        {
            ReturnCrop(crop);
            Debug.Log(4);
        }
        else
        {
            Debug.Log("wrong season");
            StartCoroutine(ShowErrorMsg("Cannot be planted in this season"));
        }
    }

    public void UpdateCoins(int value, int yield = 1)
    {
        Coins += value * yield;
        coinDisplay.GetComponent<Text>().text = "Coins: " + Coins;
    }

    public void ChangeToScreen(GameObject screenToChangeTo)
    {
        //put current screen to list to backlog, close current screen, change to next screen, show new screen
        if (currentScreen != null && screenToChangeTo != null)
        {
            previousScreens.Add(currentScreen);
            currentScreen.SetActive(false);
            currentScreen = screenToChangeTo;
            currentScreen.SetActive(true);
        }
    }

    public void CloseScreen()
    {
        if (currentScreen != null)
        {
            currentScreen.SetActive(false);
            currentScreen = previousScreens[previousScreens.Count - 1];
            previousScreens.RemoveAt(previousScreens.Count - 1);
            currentScreen.SetActive(true);
        }
    }
    public IEnumerator ShowErrorMsg(string textToDisplay, float decayTime = 5f)
    {
        Debug.Log("error");
        errorMsg.SetActive(true);
        errorMsg.GetComponent<TextMeshProUGUI>().text = textToDisplay;
        yield return new WaitForSeconds(decayTime);
        errorMsg.SetActive(false);
        StopCoroutine(ShowErrorMsg(textToDisplay, decayTime));
    }

    public void ShowTooltip(string message)
    {
        tooltipVisible = true;
        tooltip.SetActive(true);
        tooltip.GetComponentInChildren<TextMeshProUGUI>().text = message;
    }

    public void HideTooltip()
    {
        tooltipVisible = false;
        tooltip.SetActive(false);
    }
}
