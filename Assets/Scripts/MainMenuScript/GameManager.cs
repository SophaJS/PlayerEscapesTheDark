using UnityEngine;
using UnityEngine.UI;

// Simple GameManager script to control global settings like FPS
// Can also be extended later for other game-wide systems
public class GameManager : MonoBehaviour
{
    [Header("Performance Settings")]
    public int targetFPS = 60;  // Desired frame rate for the game

    private void Awake()
    {
        // ----------------------------
        // Disable VSync
        // ----------------------------
        // VSync synchronizes the game frame rate with the monitor refresh rate
        // If VSync is enabled, setting Application.targetFrameRate has no effect
        QualitySettings.vSyncCount = 0;

        // ----------------------------
        // Set the target frame rate
        // ----------------------------
        // Caps the game FPS to the specified value
        // Helps maintain consistent performance and reduces unnecessary CPU/GPU load
        Application.targetFrameRate = targetFPS;

        // ----------------------------
        // Make this object persist across scene changes
        // ----------------------------
        // This ensures the GameManager is not destroyed when loading new scenes
        DontDestroyOnLoad(gameObject);
    }
}
