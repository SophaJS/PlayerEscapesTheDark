using UnityEngine;
using UnityEngine.Rendering.Universal; // Needed for Light2D

public class Light2DPulse : MonoBehaviour
{
    private Light2D light2D;

    [Header("Pulse Settings")]
    public float minIntensity = 0f;    // Minimum light intensity
    public float maxIntensity = 1f;    // Maximum light intensity
    public float pulseSpeed = 2f;      // How fast the light pulses

    void Start()
    {
        light2D = GetComponent<Light2D>();
        if (light2D == null)
        {
            Debug.LogWarning("Light2DPulse: No Light2D component found!");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        // Mathf.Sin oscillates between -1 and 1, remap to 0-1
        float t = (Mathf.Sin(Time.time * pulseSpeed * Mathf.PI * 2f) + 1f) / 2f;
        light2D.intensity = Mathf.Lerp(minIntensity, maxIntensity, t);
    }
}
