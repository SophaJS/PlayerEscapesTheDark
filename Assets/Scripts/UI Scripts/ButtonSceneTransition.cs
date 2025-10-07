using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonSceneTransition : MonoBehaviour
{
    public Light2D light2D;          // Light to turn off instantly
    public AudioSource clickSound;   // Click SFX
    public AudioSource fluorescentLight;
    public Image fadeImage;          // Full-screen UI Image (black)
    public Image blinkImage;
    public float fadeDuration = 1f;  // How long fade lasts
    public float delayBeforeFade = 1f; // Wait time before fade
    public int sceneToLoad;          // Scene build index
    public Light2DFlicker flickerScript; // assign your flicker script
    public ImageFader imageFader;

    private bool isTransitioning = false;
    private float timer = 0f;
    private bool fadeStarted = false;

    private void Start()
    {
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(false);
        }
    }

    public void OnButtonClicked()
    {
        if (isTransitioning) return;
        isTransitioning = true;

        // 1️⃣ Instant actions
        if (clickSound != null)
            clickSound.PlayOneShot(clickSound.clip);

        // Stop background music
        if (fluorescentLight != null)
            fluorescentLight.Stop(); // immediately stops playback

        // Disable flicker
        if (flickerScript != null)
            flickerScript.enabled = false;

        if (light2D != null)
            light2D.intensity = 0f;

        if (imageFader != null)
            imageFader.enabled = false;
        
        if (blinkImage != null)
        {
            // Get current color
            Color c = blinkImage.color;

            // Change only the alpha
            c.a = 0.5f;

            // Apply it back to the Image
            blinkImage.color = c;
        }

        // 2️⃣ Prepare fade image
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            fadeImage.color = new Color(0, 0, 0, 0);
        }

        timer = 0f;
        fadeStarted = false;
    }

    private void Update()
    {
        if (!isTransitioning) return;

        timer += Time.deltaTime;

        // Wait delayBeforeFade seconds before starting fade
        if (!fadeStarted && timer >= delayBeforeFade)
        {
            fadeStarted = true;
            timer = 0f; // reset timer for fade duration
        }

        // Fade logic
        if (fadeStarted)
        {
            float t = Mathf.Clamp01(timer / fadeDuration);
            if (fadeImage != null)
            {
                Color c = fadeImage.color;
                c.a = t;
                fadeImage.color = c;
            }

            // When fade completes, load scene
            if (t >= 1f)
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }
}

