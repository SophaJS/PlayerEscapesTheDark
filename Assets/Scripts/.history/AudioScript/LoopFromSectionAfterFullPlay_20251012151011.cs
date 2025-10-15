using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class LoopFromSectionAfterFullPlay : MonoBehaviour
{
    public AudioSource source;

    [Tooltip("Where the loop should start (in seconds).")]
    public float loopStart = 3f;

    [Tooltip("Where the loop should end (in seconds).")]
    public float loopEnd = 113f; // 1:53 = 113 seconds

    private bool hasPlayedOnce = false;

    void Start()
    {
        if (source == null)
            source = GetComponent<AudioSource>();

        source.loop = false; // we'll handle looping manually
        source.Play();
    }

    void Update()
    {
        if (!source.isPlaying)
        {
            // When first playthrough (0→1:53) ends, start looping section
            if (!hasPlayedOnce)
            {
                hasPlayedOnce = true;
                source.time = loopStart;
                source.Play();
            }
        }

        // After first play, keep looping 3→1:53
        if (hasPlayedOnce && source.time >= loopEnd)
        {
            source.time = loopStart;
        }
    }
}
