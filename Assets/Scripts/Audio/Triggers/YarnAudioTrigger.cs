using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class YarnAudioTrigger : MonoBehaviour
{
    private AudioScript audioScript;

    // Start is called before the first frame update
    void Start()
    {
        audioScript = GetComponent<AudioScript>();
    }

    [YarnCommand("PlaySound")]
    public void PlaySound()
    {
        audioScript.PlaySound(0);
    }
}
