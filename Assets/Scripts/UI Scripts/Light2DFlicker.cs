using UnityEngine;
using UnityEngine.Rendering.Universal; // Needed for Light2D

public class Light2DFlicker : MonoBehaviour
{
    private Light2D light2D;

    [Header("Flicker Settings")]
    public float minIntensity = 0.7f;   // Minimum light intensity
    public float maxIntensity = 1.0f;   // Maximum light intensity
    public float flickerSpeed = 0.1f;   // How fast it flickers
    public bool randomizeSpeed = true;  // Optional for more chaotic flicker

    private float targetIntensity;
    private float timer;

    void Start()
    {
        light2D = GetComponent<Light2D>();
        if (light2D == null)
        {
            Debug.LogWarning("Light2DFlicker: No Light2D component found!");
            enabled = false;
            return;
        }

        targetIntensity = light2D.intensity;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            // Pick a new random target intensity
            targetIntensity = Random.Range(minIntensity, maxIntensity);

            // Pick how long before next change
            timer = randomizeSpeed ? Random.Range(flickerSpeed * 0.5f, flickerSpeed * 1.5f) : flickerSpeed;
        }

        // Smoothly move toward the target intensity
        light2D.intensity = Mathf.Lerp(light2D.intensity, targetIntensity, Time.deltaTime * 10f);
    }
}
