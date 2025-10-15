using UnityEngine;

public class Door : MonoBehaviour
{
    public string requiredKey = "default";

    private void OnEnable()
    {
        PlayerKeys.OnKeyCollected += HandleKeyCollected;
    }

    private void OnDisable()
    {
        PlayerKeys.OnKeyCollected -= HandleKeyCollected;
    }

    private void HandleKeyCollected(string keyID)
    {
        if (keyID == requiredKey)
        {
            Debug.Log("Door unlocked automatically!");
            Destroy(gameObject); // remove the door tiles
        }
    }
}
