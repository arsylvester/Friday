using UnityEngine;

/// <summary>
/// Plays a sound upon entering a collision with any other object.
/// </summary>
public class PlayOnCollision : MonoBehaviour
{
    /// <summary>
    /// The audio source containing the sound to be played.
    /// </summary>
    public AudioScript AudioSource;

    /// <summary>
    /// Event handler used to play audio on entering a collision.
    /// </summary>
    /// <param name="collision">Unused. Information about the collision.</param>
    void OnCollisionEnter(Collision collision)
    {
        AudioSource.PlaySound(0);
    }
}
