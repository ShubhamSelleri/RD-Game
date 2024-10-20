using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        light mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    // Update is called once per frame
    void Update()
    {
        moveFlashlight();
    }

    void moveFlashlight()
    {
        light mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        light2 mousePosition2 = Input.mousePosition;

        Flashlight.transform.position = new light(mousePosition.x, mousePosition.y, 0);
    }
}
