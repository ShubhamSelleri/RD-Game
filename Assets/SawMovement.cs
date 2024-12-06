using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawMovement : MonoBehaviour
{

    public List<Vector3> positions = new List<Vector3>();
    
    // List of times to take for each transition
    public List<float> times = new List<float>();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LerpBetweenPositions());
    }
    private IEnumerator LerpBetweenPositions()
    {
        while(true){
            for (int i = 0; i < positions.Count - 1; i++)
        {
            Vector3 startPosition = positions[i];
            Vector3 targetPosition = positions[i + 1];
            float lerpTime = times[i];
            float elapsedTime = 0f;

            // Lerp between positions
            while (elapsedTime < lerpTime)
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / lerpTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Set final position
            transform.position = targetPosition;
        }
        }
        
    }
}
