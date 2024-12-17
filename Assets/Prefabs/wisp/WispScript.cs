using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WispScript : MonoBehaviour
{
    public float rotationSpeed = 50f;
    public float opacitySpeed = 1f;
    private Renderer wispRenderer;
    private float opacity = 1f;
    private bool fadingOut = true;

    void Start()
    {
        wispRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        if (fadingOut)
        {
            opacity -= opacitySpeed * Time.deltaTime;
            if (opacity <= 0f)
            {
                opacity = 0f;
                fadingOut = false;
            }
        }
        else
        {
            opacity += opacitySpeed * Time.deltaTime;
            if (opacity >= 1f)
            {
                opacity = 1f;
                fadingOut = true;
            }
        }

        Color color = wispRenderer.material.color;
        color.a = opacity;
        wispRenderer.material.color = color;
    }
}

