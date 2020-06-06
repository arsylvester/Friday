using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Safe : MonoBehaviour
{
    [SerializeField] GameObject openDoor;
    [SerializeField] GameObject openKnob;
    [SerializeField] GameObject closeDoor;
    [SerializeField] GameObject closeKnob;

    [YarnCommand("OpenSafe")]
    public void OpenSafe()
    {
        openDoor.SetActive(true);
        openKnob.SetActive(true);
        closeDoor.SetActive(false);
        closeKnob.SetActive(false);
        GetComponent<BoxCollider>().enabled = false;
    }
}
