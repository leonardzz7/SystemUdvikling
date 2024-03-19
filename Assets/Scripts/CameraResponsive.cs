using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResponsive : MonoBehaviour
{
    public bool maintainWidth;
    [Range(-1 , 10)]
    public int adaptPosition;
    public float defaultWidth;
    public float defaulthieght;

    Vector3 CameraPos; 

    void Start()
    {
        CameraPos = Camera.main.transform.position;

        //defaultWidth = Camera.main.orthographicSize * Camera.main.aspect;
        //Debug.Log(defaultWidth);
        //defaulthieght = Camera.main.orthographicSize;
        //Debug.Log(defaulthieght);
    }

    // Update is called once per frame
    void Update()
    {
        if (maintainWidth)
        {
            Camera.main.orthographicSize = defaultWidth / Camera.main.aspect;
            Camera.main.transform.position = new Vector3(CameraPos.x, -1 * (defaulthieght - Camera.main.orthographicSize), CameraPos.z);
            
        }
    }
}
