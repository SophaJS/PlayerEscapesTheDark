using UnityEngine;
using UnityEngine.UI;

public class UIFade : MonoBehaviour
{
    public Image fadeImage;        // Your black UI image
    public float fadeDuration = 3f;

    private float elapsed = 0f;
    private bool fading = true;

    void Start()
    {
        // Start fully black
        Color c = fadeImage.color;
        c.a = 1f;
        fadeImage.color = c;
    }

    void Update()
    {
        if (!fading) return;

        elapsed += Time.deltaTime;
        float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);

        Color c = fadeImage.color;
        c.a = alpha;
        fadeImage.color = c;

        if (alpha <= 0f)
        {
            fading = false;       // Stop updating once fully transparent
            fadeImage.gameObject.SetActive(false); // optional, hide image
        }
    }
}
