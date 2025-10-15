using UnityEngine;
using UnityEngine.UI;

public class ImagePulse : MonoBehaviour
{
    public Image targetImage;
    public float halfCycleDuration = 1f; // seconds to go from minAlpha -> maxAlpha
    public float minAlpha = 0f;
    public float maxAlpha = 0.10f;

    private float timer = 0f;
    private bool fadingIn = true;

    void Start()
    {
        if (targetImage == null)
            targetImage = GetComponent<Image>();
    }

    void Update()
    {
        if (targetImage == null) return;

        // Increment timer
        timer += Time.unscaledDeltaTime;

        // Check if we need to switch direction
        if (timer >= halfCycleDuration)
        {
            timer -= halfCycleDuration; // reset timer but keep remainder
            fadingIn = !fadingIn;
        }

        // Interpolate alpha
        float progress = timer / halfCycleDuration;
        float alpha = fadingIn
            ? Mathf.Lerp(minAlpha, maxAlpha, progress)
            : Mathf.Lerp(maxAlpha, minAlpha, progress);

        Color c = targetImage.color;
        c.a = alpha;
        targetImage.color = c;
    }
}
