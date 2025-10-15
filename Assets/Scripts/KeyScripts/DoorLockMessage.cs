using UnityEngine;
using TMPro;

public class DoorLockMessage : MonoBehaviour
{
    [Header("Key Settings")]
    public string requiredKeyID = "default";

    [Header("Message Settings")]
    [TextArea]
    public string lockedMessage = "The door is locked.";
    public TextMeshProUGUI promptText; // Assign in Inspector

    private bool isUnlocked = false;

    private void Start()
    {
        if (promptText != null)
            promptText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        PlayerKeys playerKeys = collision.GetComponent<PlayerKeys>();
        if (playerKeys == null)
            return;

        // Check if player has the key
        if (playerKeys.HasKey(requiredKeyID))
        {
            isUnlocked = true;
            if (promptText != null)
                promptText.gameObject.SetActive(false);
        }
        else
        {
            if (promptText != null)
            {
                promptText.text = lockedMessage;
                promptText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && promptText != null)
        {
            promptText.gameObject.SetActive(false);
        }
    }
}
