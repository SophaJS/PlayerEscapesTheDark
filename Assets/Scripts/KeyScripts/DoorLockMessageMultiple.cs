using UnityEngine;
using TMPro;

[System.Serializable]
public class KeyRequirement
{
    public string keyID = "default";
    public int amountRequired = 1;
}

public class DoorLockMessageMultiple : MonoBehaviour
{
    [Header("Key Requirements")]
    [Tooltip("List of keys and amounts required to unlock this door.")]
    public KeyRequirement[] requiredKeys;

    [Header("Message Settings")]
    [TextArea]
    public string lockedMessage = "The door is locked. You need more keys.";
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

        PlayerKeysLevel playerKeys = collision.GetComponent<PlayerKeysLevel>();
        if (playerKeys == null)
            return;

        // Check all required keys
        bool allKeysMet = true;
        string missingText = lockedMessage + "\n";

        foreach (var req in requiredKeys)
        {
            int playerHas = playerKeys.GetKeyCount(req.keyID);
            int missing = req.amountRequired - playerHas;

            if (missing > 0)
            {
                allKeysMet = false;
                missingText += $"- {req.keyID}: need {missing} more\n";
            }
        }

        if (allKeysMet)
        {
            isUnlocked = true;
            if (promptText != null)
                promptText.gameObject.SetActive(false);
        }
        else
        {
            if (promptText != null)
            {
                promptText.text = missingText.TrimEnd();
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
