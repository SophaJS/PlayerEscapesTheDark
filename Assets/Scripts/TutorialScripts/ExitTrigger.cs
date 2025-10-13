using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class ExitTrigger : MonoBehaviour
{
    public TextMeshProUGUI exitMessageText; // assign a UI text for final message
    public int nextSceneId;                 // scene to load
    public float messageDuration = 3f;      // how long to show the message before scene change
    public PlayerController player;         // reference to the player's movement script

    private bool triggered = false;

    void Start()
    {
        // Start transparent
        exitMessageText.color = new Color(exitMessageText.color.r, exitMessageText.color.g, exitMessageText.color.b, 0f);
        exitMessageText.text = "";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!triggered && collision.CompareTag("Player"))
        {
            triggered = true;

            if (player != null)
                player.canMove = false;

            exitMessageText.text = "Congratulations! You found the exit!";
            exitMessageText.color = new Color(exitMessageText.color.r, exitMessageText.color.g, exitMessageText.color.b, 1f);

            StartCoroutine(LoadSceneAfterDelay());
        }
    }

    private IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(messageDuration);
        SceneManager.LoadScene(nextSceneId);
    }


    public void MoveToScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }
}
