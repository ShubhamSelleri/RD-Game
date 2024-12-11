using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blinkingLight : MonoBehaviour
{

    public Light lightToBlink;
    public float blinkSpeed;
    // Start is called before the first frame update
    public float baseVal;
    public float Amplitude;
    public float delay=3;

    void Start()
    {
        StartCoroutine(BlinkLight());
    }

    IEnumerator BlinkLight()
    {
        while (true)
        {
            lightToBlink.intensity=baseVal+Amplitude*Mathf.Sin(Time.time * blinkSpeed+delay);
            yield return null;
        }
    }
}
