using UnityEngine;

/// <summary>
/// This script plays an AudioSource clip all the way through once (from start to finish),
/// then continuously loops only a specific section of it (between loopStart and loopEnd).
/// </summary>


// Ensures the GameObject has an AudioSource component
[RequireComponent(typeof(AudioSource))]
public class LoopFromSectionAfterFullPlay : MonoBehaviour
{
    // Reference to the AudioSource component that will play the clip
    public AudioSource source;

    [Tooltip("Where the loop should start (in seconds).")]
    public float loopStart = 3f;  // After the first full play, looping begins from 3 seconds

    [Tooltip("Where the loop should end (in seconds).")]
    public float loopEnd = 113f;  // 113 seconds = 1:53 mark, where looping will restart

    // Tracks whether the clip has already been played once fully
    private bool hasPlayedOnce = false;

    void Start()
    {
        // If no AudioSource is assigned in the Inspector, get the one attached to this GameObject
        if (source == null)
            source = GetComponent<AudioSource>();

        // Disable built-in looping so we can control looping manually
        source.loop = false;

        // Begin playing from the very start (first full playthrough)
        source.Play();
    }

    void Update()
    {
        // -----------------------------
        // Detect when the first full playthrough ends
        // -----------------------------
        if (!source.isPlaying)
        {
            // If the clip stopped because it finished playing
            // and we haven’t yet started the looping section
            if (!hasPlayedOnce)
            {
                hasPlayedOnce = true;     // Mark that the first playthrough is done

                // Jump to the loop start position (e.g., 3 seconds)
                source.time = loopStart;

                // Start playing again, now entering the looping behavior
                source.Play();
            }
        }

        // -----------------------------
        // Handle the manual loop after the first playthrough
        // -----------------------------
        if (hasPlayedOnce && source.time >= loopEnd)
        {
            // If we've reached the end of the loop section,
            // jump back to the loop start point (no pause or gap)
            source.time = loopStart;
        }
    }
}
