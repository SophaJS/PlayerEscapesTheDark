using UnityEngine;
using UnityEngine.UI;

// This script smoothly fades a UI Image in and out, creating a pulsing glow or breathing effect.
// Great for highlighting clickable objects or drawing subtle attention to something.
public class ImagePulse : MonoBehaviour
{
    // The Image component whose alpha will pulse
    public Image targetImage;

    // How long it takes to go from minAlpha → maxAlpha (half of a full in-out cycle)
    public float halfCycleDuration = 1f;

    // The minimum and maximum alpha (transparency) values
    // 0 = fully transparent, 1 = fully opaque
    public float minAlpha = 0f;
    public float maxAlpha = 0.10f;

    // Internal timer to track progress through a half cycle
    private float timer = 0f;

    // Tracks whether the image is currently fading in (true) or fading out (false)
    private bool fadingIn = true;

    // Called once at the start
    void Start()
    {
        // If no image is assigned in the Inspector, try to get one on this GameObject
        if (targetImage == null)
            targetImage = GetComponent<Image>();
    }

    // Called every frame
    void Update()
    {
        // If there’s no Image to modify, do nothing
        if (targetImage == null) return;

        // ----------------------------
        // 1️⃣ Update the timer
        // ----------------------------
        // Use unscaledDeltaTime so the pulsing isn't affected by time scaling (e.g., pausing the game)
        timer += Time.unscaledDeltaTime;

        // ----------------------------
        // 2️⃣ Check if half the cycle is done
        // ----------------------------
        if (timer >= halfCycleDuration)
        {
            // Subtract the half-cycle length instead of resetting to zero
            // This keeps the timing accurate even if the frame overshoots a little
            timer -= halfCycleDuration;

            // Reverse the direction (fade in → fade out → fade in → ...)
            fadingIn = !fadingIn;
        }

        // ----------------------------
        // 3️⃣ Calculate new alpha based on progress
        // ----------------------------
        float progress = timer / halfCycleDuration; // normalized 0–1 progress within the half cycle

        // Interpolate alpha value depending on direction
        float alpha = fadingIn
            ? Mathf.Lerp(minAlpha, maxAlpha, progress)   // increasing alpha
            : Mathf.Lerp(maxAlpha, minAlpha, progress);  // decreasing alpha

        // ----------------------------
        // 4️⃣ Apply the alpha to the Image color
        // ----------------------------
        Color c = targetImage.color;
        c.a = alpha;
        targetImage.color = c;
    }
}
