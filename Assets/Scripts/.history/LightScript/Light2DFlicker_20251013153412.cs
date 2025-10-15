using UnityEngine;
using UnityEngine.Rendering.Universal; // Needed for Light2D component

public class Light2DFlicker : MonoBehaviour
{
    private Light2D light2D; // Reference to the Light2D component

    [Header("Flicker Settings")]
    public float minIntensity = 0.7f;   // Minimum light intensity
    public float maxIntensity = 1.0f;   // Maximum light intensity
    public float flickerSpeed = 0.1f;   // How often intensity changes
    public bool randomizeSpeed = true;  // Whether to randomize flicker timing

    private float targetIntensity; // The next intensity value to flicker toward
    private float timer;            // Time until the next intensity change

    void Start()
    {
        // Get the Light2D component on this GameObject
        light2D = GetComponent<Light2D>();

        // Safety check: if no Light2D found, disable script to avoid errors
        if (light2D == null)
        {
            Debug.LogWarning("Light2DFlicker: No Light2D component found!");
            enabled = false;
            return;
        }

        // Initialize starting intensity
        targetIntensity = light2D.intensity;
    }

    void Update()
    {
        // Reduce the timer by the time that has passed since the last frame
        timer -= Time.deltaTime;

        // When timer reaches zero, pick a new target intensity and reset timer
        if (timer <= 0f)
        {
            // Pick a random target intensity between min and max
            targetIntensity = Random.Range(minIntensity, maxIntensity);

            // Pick a new random delay before the next flicker, if enabled
            timer = randomizeSpeed
                ? Random.Range(flickerSpeed * 0.5f, flickerSpeed * 1.5f)
                : flickerSpeed;
        }

        // Smoothly move current intensity toward the new target intensity
        light2D.intensity = Mathf.Lerp(
            light2D.intensity,
            targetIntensity,
            Time.deltaTime * 10f // Higher value = faster transition
        );
    }
}
