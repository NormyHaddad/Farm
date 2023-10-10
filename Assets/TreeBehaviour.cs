using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBehaviour : MonoBehaviour
{
    //public GameManager gameManager;
    GameManager gameManager;
    public GameObject fruit;
    public GameObject ghost;
    GameObject fruitClone;
    public int value;
    public int yield;
    public float regrowTime;
    float counter = 0;
    enum states
    {
        READY,
        GROWING
    };
    states currentState = states.GROWING;

    // Start is called before the first frame update
    void Start()
    {
        fruit.SetActive(false);
        var all = FindObjectsOfType<GameManager>();
        foreach (var item in all)
        {
            if (item.name == "Game Manager")
                gameManager = item;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == states.GROWING)
        {
            counter += Time.deltaTime;
        }
        if (counter >= regrowTime)
        {
            currentState = states.READY;
            fruit.SetActive(true);
        }
    }

    private void OnMouseDown()
    {
        print("tree clicked");
        if (currentState == states.READY && gameManager.placeCropMode == false)
        {
            fruit.SetActive(false);
            currentState = states.GROWING;
            counter = 0;
            if (gameManager != null)
                gameManager.UpdateCoins(value, yield);
        }
    }
}
