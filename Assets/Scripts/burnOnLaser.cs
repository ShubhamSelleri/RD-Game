using UnityEngine;

public class ContinuousProximityEffect : MonoBehaviour
{
    public Transform target;           // The target object to check proximity
    public float checkDistance = 5f;   // Distance within which to check for the target
    private float proximityTime = 0f;   // Time spent within the proximity
    private bool isWithinProximity = false; // Is currently within proximity
    public GameObject flameEffects;     // Reference to the FlameEffects child
    public float burnDuration = 5f;    // Duration for which the flame effects are active
    private float burnTime = 0f;        // Timer for the burn duration
    private bool isBurning = false;     // Is currently burning

    void Update()
    {
        // Check the distance to the target
        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance <= checkDistance)
            {
                // We're within proximity
                if (!isWithinProximity)
                {
                    isWithinProximity = true; // Start the proximity timer
                    proximityTime = 0f;       // Reset the timer
                }

                proximityTime += Time.deltaTime; // Increase the timer

                // Check if proximity time exceeds 2 seconds
                if (proximityTime >= 1f && flameEffects != null && !isBurning)
                {
                    flameEffects.SetActive(true); // Enable the FlameEffects
                    isBurning = true;             // Start the burning effect
                }
            }

            // If burning, manage the burn duration
            if (isBurning)
            {
                burnTime += Time.deltaTime; // Increase burn timer

                // Disable the GameObject after burnDuration
                if (burnTime >= burnDuration)
                {
                    gameObject.SetActive(false); // Disable this GameObject
                }
            }
        }
    }
}
