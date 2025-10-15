using System.Collections;
using TMPro; // TextMeshPro for high-quality UI text
using UnityEngine;

public class TutorialMessageSystem : MonoBehaviour
{
    [Header("Player Reference")]
    public PlayerController playerMovement; // Reference to the player movement script

    [Header("UI Messages")]
    public TextMeshProUGUI firstMessageText;  // First message (blinking “Click to start”)
    public TextMeshProUGUI secondMessageText; // Second message (instructions and follow-ups)

    [Header("Settings")]
    public float blinkSpeed = 4f; // How fast the first message blinks

    // Internal state variables
    private bool activated = false;                // Tracks if the player has clicked to start
    private Color firstBaseColor;                  // Base color of first message (without alpha)
    private Color secondBaseColor;                 // Base color of second message (without alpha)
    private float secondMessageTimer = 0f;         // Timer for second message timing
    private bool secondMessageActive = false;      // Whether second message is being displayed
    private bool secondMessageInputReceived = false; // Whether player pressed WASD after second message
    private float thirdMessageTimer = 0f;          // Timer for final “Now find the exit!” message

    // Coroutine Start allows waiting a frame for Canvas initialization
    private IEnumerator Start()
    {
        // Wait one frame for UI Canvas system to initialize properly
        yield return null;

        // Ensure the UI text objects are active in the scene
        if (firstMessageText != null) firstMessageText.gameObject.SetActive(true);
        if (secondMessageText != null) secondMessageText.gameObject.SetActive(true);

        // Set base colors explicitly
        firstBaseColor = Color.white;
        secondBaseColor = Color.white;

        // Initialize first message fully visible
        firstMessageText.text = "Click your player to start!";
        firstMessageText.color = Color.white;

        // Initialize second message fully transparent
        secondMessageText.text = "";
        secondMessageText.color = new Color(1f, 1f, 1f, 0f);

        // Prevent player movement until first click
        playerMovement.canMove = false;

        // Force the Canvas to rebuild (ensures proper alpha and layout)
        Canvas.ForceUpdateCanvases();
        firstMessageText.ForceMeshUpdate(true);
        secondMessageText.ForceMeshUpdate(true);

        // Ensure alpha is applied immediately
        firstMessageText.CrossFadeAlpha(1f, 0f, true);
    }

    void Update()
    {
        // --- FIRST MESSAGE: blinking "Click your player to start!" ---
        if (!activated)
        {
            // Blink effect using sine wave oscillation
            float alpha = Mathf.Lerp(0f, 1f, (Mathf.Sin(Time.time * blinkSpeed) + 1f) / 2f);
            firstMessageText.color = new Color(firstBaseColor.r, firstBaseColor.g, firstBaseColor.b, alpha);
        }
        else
        {
            // Fade out first message after player clicks
            if (firstMessageText.color.a > 0f)
            {
                float newAlpha = Mathf.MoveTowards(firstMessageText.color.a, 0f, Time.deltaTime / 2f);
                firstMessageText.color = new Color(firstBaseColor.r, firstBaseColor.g, firstBaseColor.b, newAlpha);
            }
            else if (!secondMessageActive)
            {
                // After fade completes, wait 2 seconds before showing second message
                secondMessageTimer += Time.deltaTime;
                if (secondMessageTimer >= 2f)
                {
                    ShowSecondMessage("Use WASD to control your flashlight");
                }
            }
        }

        // --- SECOND MESSAGE: wait for player input ---
        if (secondMessageActive && !secondMessageInputReceived)
        {
            // Detect WASD input
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) ||
                Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
            {
                // Provide immediate feedback
                secondMessageText.text = "Perfect!";
                secondMessageInputReceived = true;
                secondMessageTimer = 0f; // reset timer for fade and next message
            }
        }

        // --- SECOND MESSAGE: fade out and show third message ---
        if (secondMessageInputReceived)
        {
            secondMessageTimer += Time.deltaTime;

            if (secondMessageTimer < 1f)
            {
                // Ensure second message fully visible for a brief moment
                float alpha = Mathf.MoveTowards(secondMessageText.color.a, 1f, Time.deltaTime * 2f);
                secondMessageText.color = new Color(secondBaseColor.r, secondBaseColor.g, secondBaseColor.b, alpha);
            }
            else if (secondMessageTimer >= 1f && secondMessageTimer < 4f)
            {
                // Gradually fade out second message over 3 seconds
                float alpha = Mathf.MoveTowards(secondMessageText.color.a, 0f, Time.deltaTime / 1f);
                secondMessageText.color = new Color(secondBaseColor.r, secondBaseColor.g, secondBaseColor.b, alpha);
            }
            else if (secondMessageTimer >= 4f && secondMessageText.text != "Now find the exit!")
            {
                // Display the final message to guide the player to the exit
                secondMessageText.text = "Now find the exit!";
                secondMessageText.color = new Color(secondBaseColor.r, secondBaseColor.g, secondBaseColor.b, 1f);
                thirdMessageTimer = 0f;
            }
        }

        // --- THIRD MESSAGE: fade out after 3 seconds ---
        if (secondMessageText.text == "Now find the exit!")
        {
            thirdMessageTimer += Time.deltaTime;
            if (thirdMessageTimer >= 3f)
            {
                // Hide the final message
                secondMessageText.color = new Color(secondBaseColor.r, secondBaseColor.g, secondBaseColor.b, 0f);
            }
        }
    }

    // --- PLAYER CLICK HANDLER ---
    void OnMouseDown()
    {
        if (!activated)
        {
            activated = true;              // Mark tutorial as started
            playerMovement.canMove = true; // Allow player movement

            // Give immediate feedback on click
            firstMessageText.text = "Good Job!";
            firstMessageText.color = new Color(firstBaseColor.r, firstBaseColor.g, firstBaseColor.b, 1f);
        }
    }

    // --- FUNCTION TO DISPLAY SECOND MESSAGE ---
    void ShowSecondMessage(string text)
    {
        secondMessageText.text = text;
        secondMessageText.color = new Color(secondBaseColor.r, secondBaseColor.g, secondBaseColor.b, 1f);
        secondMessageActive = true;
        secondMessageTimer = 0f; // reset timer for second message phase
    }
}
