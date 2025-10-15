using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int targetFPS = 60;          // Desired FPS

    private void Awake()
    {
        // Disable VSync so the target FPS works
        QualitySettings.vSyncCount = 0;

        // Set the FPS cap
        Application.targetFrameRate = targetFPS;

        // Persist across all scenes
        DontDestroyOnLoad(gameObject);
    }

}