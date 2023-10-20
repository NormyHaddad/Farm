using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MillController : MonoBehaviour
{
    public GameObject gameManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        gameManager.GetComponent<GameManager>().ShowTooltip("In: 10x Wheat\nOut: 30x Flour");
    }

    private void OnMouseExit()
    {
        gameManager.GetComponent<GameManager>().HideTooltip();
    }
}
