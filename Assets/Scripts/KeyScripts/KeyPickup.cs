using UnityEngine;
using System.Collections;

public class KeyPickup : MonoBehaviour
{
    public string keyID = "default";
    public KeyCode interactKey = KeyCode.E;
    [TextArea]
    public string pickupMessage = "You picked up the key."; // customize in Inspector

    private bool playerInRange = false;
    private PlayerKeys playerKeys;
    private bool pickedUp = false;
    private SpriteRenderer spriteRenderer; // hide the key visually
    private Collider2D col;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (playerInRange && !pickedUp && Input.GetKeyDown(interactKey))
        {
            pickedUp = true;

            // Add key immediately
            playerKeys.AddKey(keyID);

            // Hide the key immediately
            if (spriteRenderer != null) spriteRenderer.enabled = false;
            if (col != null) col.enabled = false;

            // Show pickup message
            StartCoroutine(ShowPickupMessage());
        }
    }

    private IEnumerator ShowPickupMessage()
    {
        InteractionPrompt.Instance.ShowPrompt(pickupMessage);

        // Keep message on screen for 2 seconds
        yield return new WaitForSeconds(2f);

        InteractionPrompt.Instance.HidePrompt();

        // Now destroy the GameObject completely
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !pickedUp)
        {
            playerInRange = true;
            playerKeys = collision.GetComponent<PlayerKeys>();
            InteractionPrompt.Instance.ShowPrompt("Press E to pick up key");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !pickedUp)
        {
            playerInRange = false;
            playerKeys = null;
            InteractionPrompt.Instance.HidePrompt();
        }
    }
}
