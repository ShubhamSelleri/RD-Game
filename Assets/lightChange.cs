using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightChange : MonoBehaviour
{
    private LightAdjustment Lights;
    public float WorldLightIntensity;
    public float FlashLightIntensity;
    // Start is called before the first frame update
    void Start()
    {
         Lights = transform.parent.GetComponent<LightAdjustment>();
    }

    void OnTriggerEnter(Collider col)
{
    if (col.CompareTag("Player"))
    {
        Lights.WorldLight.intensity=WorldLightIntensity;
        Lights.FlashLight.intensity=FlashLightIntensity;
    }
}
}
