using UnityEngine;
using UnityEngine.SceneManagement; // For loading scenes
using TMPro;                        // For TextMeshProUGUI
using System.Collections;           // For using Coroutines

public class TutorialExitTrigger : MonoBehaviour
{
    [Header("UI & Scene Settings")]
    public TextMeshProUGUI exitMessageText; // UI text to show final message
    public int nextSceneId;                 // Build index of the next scene to load
    public float messageDuration = 3f;      // How long to display the exit message before scene change

    [Header("Player Reference")]
    public PlayerController player;         // Reference to the player's movement script

    // Internal flag to ensure trigger only happens once
    private bool triggered = false;

    void Start()
    {
        // Start the exit message fully transparent
        exitMessageText.color = new Color(
            exitMessageText.color.r, 
            exitMessageText.color.g, 
            exitMessageText.color.b, 
            0f
        );

        // Clear any text at start
        exitMessageText.text = "";
    }

    // Called when another collider enters this trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only trigger once, and only if the player enters
        if (!triggered && collision.CompareTag("Player"))
        {
            triggered = true;

            // Disable player movement
            if (player != null)
                player.canMove = false;

            // Set the exit message text
            exitMessageText.text = "Congratulations! You found the exit!";

            // Make message fully visible
            exitMessageText.color = new Color(
                exitMessageText.color.r,
                exitMessageText.color.g,
                exitMessageText.color.b,
                1f
            );

            // Start coroutine to wait for a few seconds before loading the next scene
            StartCoroutine(LoadSceneAfterDelay());
        }
    }

    // Coroutine handles waiting before scene transition
    private IEnumerator LoadSceneAfterDelay()
    {
        // Wait for the duration specified
        yield return new WaitForSeconds(messageDuration);

        // Load the next scene
        SceneManager.LoadScene(nextSceneId);
    }

    // Optional method to load any scene immediately (can be called elsewhere)
    public void MoveToScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }
}
