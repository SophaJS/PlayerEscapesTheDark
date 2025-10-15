using UnityEngine;
using UnityEngine.Rendering.Universal; // For 2D lights
using UnityEngine.SceneManagement;     // For scene loading
using UnityEngine.UI;                  // For UI Image control

public class ButtonSceneTransition : MonoBehaviour
{
    [Header("Scene Transition Components")]
    public Light2D light2D;                  // The scene’s Light2D object that will turn off instantly
    public AudioSource clickSound;           // Audio source for button click sound effect
    public AudioSource fluorescentLight;     // Background light or ambience audio that should stop
    public Image fadeImage;                  // Fullscreen black UI image used for fading out
    public Image blinkImage;                 // Optional: secondary image used for visual blink
    public float fadeDuration = 1f;          // How long it takes for the screen to fade fully to black
    public float delayBeforeFade = 1f;       // Wait time (in seconds) before fade starts after button press
    public int sceneToLoad;                  // Scene build index to load when transition finishes

    [Header("Optional Scripts")]
    public Light2DFlicker flickerScript;     // Reference to any Light2DFlicker script that should be disabled
    public ImagePulse imageFader;            // Reference to the UI pulsing image script to disable during transition

    // Internal state control
    private bool isTransitioning = false;    // Prevents multiple transitions from being triggered
    private float timer = 0f;                // Timer used for delay and fade tracking
    private bool fadeStarted = false;        // Tracks when the fade portion has begun

    private void Start()
    {
        // Ensure fade image starts off inactive (invisible)
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(false);
        }
    }

    // Called by a UI Button’s OnClick() event
    public void OnButtonClicked()
    {
        // Prevent triggering multiple times
        if (isTransitioning) return;
        isTransitioning = true;

        // ───────────────────────────────────────────────
        // IMMEDIATE ACTIONS — things that happen right when button is pressed
        // ───────────────────────────────────────────────

        // Play click SFX (if provided)
        if (clickSound != null)
            clickSound.PlayOneShot(clickSound.clip);

        // Stop background or ambient sound immediately
        if (fluorescentLight != null)
            fluorescentLight.Stop();

        // Disable flickering effect script to freeze light behavior
        if (flickerScript != null)
            flickerScript.enabled = false;

        // Instantly turn off light source
        if (light2D != null)
            light2D.intensity = 0f;

        // Stop any UI pulsing effects
        if (imageFader != null)
            imageFader.enabled = false;

        // Dim blink image slightly to create a “flash” or visual feedback effect
        if (blinkImage != null)
        {
            // Copy the current color, modify only alpha, and reapply
            Color c = blinkImage.color;
            c.a = 0.5f;
            blinkImage.color = c;
        }

        // ───────────────────────────────────────────────
        // SETUP FOR FADE TO BLACK
        // ───────────────────────────────────────────────

        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);         // Make sure it’s visible
            fadeImage.color = new Color(0, 0, 0, 0);      // Start fully transparent
        }

        // Reset timers
        timer = 0f;
        fadeStarted = false;
    }

    private void Update()
    {
        // Only update logic if a transition has started
        if (!isTransitioning) return;

        // Keep track of time elapsed since transition began
        timer += Time.deltaTime;

        // ───────────────────────────────────────────────
        // DELAY PHASE — wait before fade starts
        // ───────────────────────────────────────────────
        if (!fadeStarted && timer >= delayBeforeFade)
        {
            fadeStarted = true;
            timer = 0f; // Reset timer for the fade process
        }

        // ───────────────────────────────────────────────
        // FADE PHASE — gradually fade image to full black
        // ───────────────────────────────────────────────
        if (fadeStarted)
        {
            // Calculate fade progress from 0 → 1
            float t = Mathf.Clamp01(timer / fadeDuration);

            // Apply new alpha value to fade image
            if (fadeImage != null)
            {
                Color c = fadeImage.color;
                c.a = t; // Alpha increases from 0 (transparent) to 1 (black)
                fadeImage.color = c;
            }

            // ───────────────────────────────────────────────
            // LOAD NEW SCENE WHEN FADE COMPLETE
            // ───────────────────────────────────────────────
            if (t >= 1f)
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }
}
