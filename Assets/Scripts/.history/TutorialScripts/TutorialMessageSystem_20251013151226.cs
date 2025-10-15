using System.Collections;
using TMPro;
using UnityEngine;

public class TutorialMessageSystem : MonoBehaviour
{
    public PlayerController playerMovement;

    [Header("UI Messages")]
    public TextMeshProUGUI firstMessageText;
    public TextMeshProUGUI secondMessageText;

    [Header("Settings")]
    public float blinkSpeed = 4f;

    private bool activated = false;
    private Color firstBaseColor;
    private Color secondBaseColor;

    private float secondMessageTimer = 0f;
    private bool secondMessageActive = false;
    private bool secondMessageInputReceived = false;
    private float thirdMessageTimer = 0f;

    private IEnumerator Start()
    {
        // Wait one frame for the Canvas system to initialize
        yield return null;

        // Make sure the text objects are active
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

        playerMovement.canMove = false;

        // Force the UI system to rebuild after first frame
        Canvas.ForceUpdateCanvases();
        firstMessageText.ForceMeshUpdate(true);
        secondMessageText.ForceMeshUpdate(true);

        // Force alpha to be applied
        firstMessageText.CrossFadeAlpha(1f, 0f, true);
    }

    void Update()
    {
        // --- First message blinking ---
        if (!activated)
        {
            float alpha = Mathf.Lerp(0f, 1f, (Mathf.Sin(Time.time * blinkSpeed) + 1f) / 2f);
            firstMessageText.color = new Color(firstBaseColor.r, firstBaseColor.g, firstBaseColor.b, alpha);
        }
        else
        {
            // Fade out first message
            if (firstMessageText.color.a > 0f)
            {
                float newAlpha = Mathf.MoveTowards(firstMessageText.color.a, 0f, Time.deltaTime / 2f);
                firstMessageText.color = new Color(firstBaseColor.r, firstBaseColor.g, firstBaseColor.b, newAlpha);
            }
            else if (!secondMessageActive)
            {
                // Start timer for second message after first fades
                secondMessageTimer += Time.deltaTime;
                if (secondMessageTimer >= 2f)
                {
                    ShowSecondMessage("Use WASD to control your flashlight");
                }
            }
        }

        // --- Second message input response ---
        if (secondMessageActive && !secondMessageInputReceived)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) ||
                Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
            {
                secondMessageText.text = "Perfect!";
                secondMessageInputReceived = true;
                secondMessageTimer = 0f; // reset timer for next phase
            }
        }

        // --- Second message disappearance and third message ---
        if (secondMessageInputReceived)
        {
            secondMessageTimer += Time.deltaTime;

            // Fade out second message after 1s
            if (secondMessageTimer < 1f)
            {
                float alpha = Mathf.MoveTowards(secondMessageText.color.a, 1f, Time.deltaTime * 2f); // ensure fully visible
                secondMessageText.color = new Color(secondBaseColor.r, secondBaseColor.g, secondBaseColor.b, alpha);
            }
            else if (secondMessageTimer >= 1f && secondMessageTimer < 4f)
            {
                // Fade out gradually
                float alpha = Mathf.MoveTowards(secondMessageText.color.a, 0f, Time.deltaTime / 1f);
                secondMessageText.color = new Color(secondBaseColor.r, secondBaseColor.g, secondBaseColor.b, alpha);
            }
            else if (secondMessageTimer >= 4f && secondMessageText.text != "Now find the exit!")
            {
                // Show final message
                secondMessageText.text = "Now find the exit!";
                secondMessageText.color = new Color(secondBaseColor.r, secondBaseColor.g, secondBaseColor.b, 1f);
                thirdMessageTimer = 0f;
            }
        }

        // --- Third message disappearance ---
        if (secondMessageText.text == "Now find the exit!")
        {
            thirdMessageTimer += Time.deltaTime;
            if (thirdMessageTimer >= 3f)
            {
                secondMessageText.color = new Color(secondBaseColor.r, secondBaseColor.g, secondBaseColor.b, 0f);
            }
        }
    }

    void OnMouseDown()
    {
        if (!activated)
        {
            activated = true;
            playerMovement.canMove = true;

            // Show first message fully on click
            firstMessageText.text = "Good Job!";
            firstMessageText.color = new Color(firstBaseColor.r, firstBaseColor.g, firstBaseColor.b, 1f);
        }
    }

    void ShowSecondMessage(string text)
    {
        secondMessageText.text = text;
        secondMessageText.color = new Color(secondBaseColor.r, secondBaseColor.g, secondBaseColor.b, 1f);
        secondMessageActive = true;
        secondMessageTimer = 0f;
    }
}
