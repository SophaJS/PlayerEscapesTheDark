using UnityEngine;
using UnityEngine.Rendering.Universal; // Needed for Light2D

public class Light2DPulse : MonoBehaviour
{
    private Light2D light2D; // Reference to the 2D light component on this object

    [Header("Pulse Settings")]
    public float minIntensity = 0f;   // Lowest light intensity (dim point)
    public float maxIntensity = 1f;   // Highest light intensity (bright point)
    public float pulseSpeed = 2f;     // How fast the light fades in and out

    void Start()
    {
        // Try to get the Light2D component attached to this GameObject
        light2D = GetComponent<Light2D>();

        // If no Light2D component is found, disable this script to prevent errors
        if (light2D == null)
        {
            Debug.LogWarning("Light2DPulse: No Light2D component found!");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        // Mathf.Sin(Time.time * speed * 2π) oscillates smoothly between -1 and +1 over time
        // By adding 1 and dividing by 2, we remap the range from [-1, 1] to [0, 1]
        float t = (Mathf.Sin(Time.time * pulseSpeed * Mathf.PI * 2f) + 1f) / 2f;

        // Smoothly interpolate (fade) between minIntensity and maxIntensity using t
        light2D.intensity = Mathf.Lerp(minIntensity, maxIntensity, t);

        // The result is a smooth pulsing effect:
        // → gradually brightens to max
        // → then dims to min
        // → repeats continuously
    }
}
