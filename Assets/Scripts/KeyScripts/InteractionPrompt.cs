using TMPro;
using UnityEngine;

public class InteractionPrompt : MonoBehaviour
{
    public static InteractionPrompt Instance { get; private set; }

    public TextMeshProUGUI promptText; // just the text object

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        HidePrompt();
    }

    public void ShowPrompt(string message)
    {
        if (promptText != null)
        {
            promptText.gameObject.SetActive(true);
            promptText.text = message;
        }
    }

    public void HidePrompt()
    {
        if (promptText != null)
            promptText.gameObject.SetActive(false);
    }
}
