using UnityEngine;

/// <summary>
/// Plays audio at the start of this script becoming active. Usually, this
/// happens once at the start of a scene loading.
/// </summary>
public class PlayOnStart : MonoBehaviour
{
    /// <summary>
    /// The audio source containing the sound to be played.
    /// </summary>
    public AudioScript AudioSource;
    /// <summary>
    /// Event handler used to play audio at the start of this script becoming
    /// active.
    /// </summary>
    void Start()
    {
        AudioSource.PlaySound(0);
    }

}
