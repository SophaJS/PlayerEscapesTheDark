using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script fades a full-screen UI Image (like a black overlay)
/// from opaque → transparent over time.
/// Commonly used for fade-in effects when starting a scene.
/// </summary>

public class UIFadeIn : MonoBehaviour
{
    // The black image that covers the screen (usually a full-screen UI panel)
    public Image fadeImage;

    // How long (in seconds) the fade should take
    public float fadeDuration = 3f;

    // Internal timer tracking how long we've been fading
    private float elapsed = 0f;

    // Whether the fade effect is currently running
    private bool fading = true;

    void Start()
    {
        // ----------------------------
        // Initialize the fade image to fully opaque (black)
        // ----------------------------
        // Get the current color of the image
        Color c = fadeImage.color;

        // Set its alpha to 1 (fully visible)
        c.a = 1f;

        // Apply the color back to the Image
        fadeImage.color = c;
    }

    void Update()
    {
        // If fade is done, skip updates
        if (!fading) return;

        // ----------------------------
        // Track time progression
        // ----------------------------
        // Add how much time passed this frame
        elapsed += Time.deltaTime;

        // ----------------------------
        // Compute new alpha value
        // ----------------------------
        // Lerp smoothly from 1 (black) to 0 (transparent)
        float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);

        // Apply new alpha to the image color
        Color c = fadeImage.color;
        c.a = alpha;
        fadeImage.color = c;

        // ----------------------------
        // Stop when fade is complete
        // ----------------------------
        if (alpha <= 0f)
        {
            fading = false; // Stop updating once fully transparent

            // Optionally disable the image so it doesn't block UI clicks
            fadeImage.gameObject.SetActive(false);
        }
    }
}
