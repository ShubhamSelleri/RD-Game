using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 lastPosition;
    private Vector3 velocity;
    void Start()
    {
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        velocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;
    }
    public Vector3 getVelocity()
    {
        return velocity;
    }
}
