using UnityEngine;
using System.Collections;
public class ChangeAlpha : MonoBehaviour
{
    private Renderer objectRenderer;  // Reference to the object's Renderer
    public float animationDuration = 2f;  // Duration for the full animation (0 -> 1 -> 0)
    private float timeElapsed = 0f;  // To track elapsed time during animation
    public float maxTint=0.8f; //maximum tint for the animation

    void Start()
    {
        // Get the Renderer component from this GameObject
        objectRenderer = GetComponent<Renderer>();

        StartCoroutine(AnimateAlpha());
    }

    public IEnumerator AnimateAlpha()
    {
        // Loop to animate the alpha from 0 to 1 and back to 0
        while (timeElapsed < animationDuration)
        {
            // Calculate alpha using Mathf.PingPong
            float alpha = Mathf.Lerp(0f, 1f, timeElapsed / (animationDuration / 2));  // First half: fade in
            alpha = Mathf.Lerp(1f, 0f, (timeElapsed - (animationDuration / 2)) / (animationDuration / 2));  // Second half: fade out
            // Get the current tint color and modify alpha
            Color currentColor = objectRenderer.material.GetColor("_TintColor");
            currentColor.a = alpha;

            // Apply the new color with the updated alpha
            objectRenderer.material.SetColor("_TintColor", currentColor);

            // Increment the timeElapsed (time for animation)
            timeElapsed += Time.deltaTime;

            // Yield to continue the animation in the next frame
            yield return null;
        }
        Color color = objectRenderer.material.GetColor("_TintColor");
        color.a = 0;
        objectRenderer.material.SetColor("_TintColor", color);
        timeElapsed=0;
    }
}
