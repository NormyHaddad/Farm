using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float movespeed;
    public float boundaryX;
    public float boundaryY;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") == -1 && transform.position.x >= -boundaryX)
            gameObject.transform.position += new Vector3(-movespeed * Time.deltaTime, 0, 0);
        if (Input.GetAxis("Horizontal") == 1 && transform.position.x <= boundaryX)
            gameObject.transform.position += new Vector3(movespeed * Time.deltaTime, 0, 0);
        if (Input.GetAxis("Vertical") == -1 && transform.position.z >= -boundaryY)
            gameObject.transform.position += new Vector3(0, 0, -movespeed * Time.deltaTime);
        if (Input.GetAxis("Vertical") == 1 && transform.position.z <= boundaryY)
            gameObject.transform.position += new Vector3(0, 0, movespeed * Time.deltaTime);
    }
}
