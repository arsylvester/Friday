using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PlaySound : MonoBehaviour
{
    [System.Serializable]
    public struct SoundFXInfo
    {
        public string name;
        public AudioClip clip;
    }

    public SoundFXInfo[] clips;

    /// Create a command to use on a sprite
    [YarnCommand("playsound")]
    public void PlayAudio(string clipName)
    {

        AudioClip c = null;
        foreach (var info in clips)
        {
            if (info.name == clipName)
            {
                c = info.clip;
                break;
            }
        }
        if (c == null)
        {
            Debug.LogErrorFormat("Can't find audio clip named {0}!", clipName);
            return;
        }

        GetComponent<AudioSource>().PlayOneShot(c);
    }
}
