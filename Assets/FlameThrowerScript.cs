using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowerScript : MonoBehaviour
{
    public GameObject flames;
    public int offTime;
    public int onTime;
    public int startDelay;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ToggleObject());
    }


    // Coroutine to toggle the GameObject on and off
    IEnumerator ToggleObject()
    {
        yield return new WaitForSeconds(startDelay);
        while (true)
        {
            // Enable the object
            flames.SetActive(true);
            
            // Wait for the "on time"
            yield return new WaitForSeconds(onTime);
            
            // Disable the object
            flames.SetActive(false);
            
            // Wait for the "off time"
            yield return new WaitForSeconds(offTime);
        }
    }
}
