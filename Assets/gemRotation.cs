using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gemRotation : MonoBehaviour
{
    public float rotationSpeed = 90f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Rot());
    }

    // Update is called once per frame
    private IEnumerator Rot()
    {
        while (true)  // Infinite loop to rotate forever
        {
            // Rotate the object around the specified axis at the specified speed
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            yield return null;  // Wait for the next frame
        }
    }
}
