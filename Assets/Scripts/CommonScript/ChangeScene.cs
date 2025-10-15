using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This simple script allows you to switch to another scene in Unity.
/// It can be attached to a button or any GameObject and called from code or UI events.
/// </summary>

public class ChangeScene : MonoBehaviour
{
    // This public method loads a new scene based on its build index.
    // The "sceneID" corresponds to the scene's order in Unity's Build Settings.
    public void MoveToScene(int sceneID)
    {
        // SceneManager handles loading and unloading scenes in Unity.
        // LoadScene(sceneID) will immediately load the scene with that index.
        SceneManager.LoadScene(sceneID);
    }
}
