using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISubsection : MonoBehaviour
{
    private bool isOpen = true;

    public void ToggleChildern()
    {
        isOpen = !isOpen;
        for(int x = 0; x < transform.childCount; x++)
        {
            transform.GetChild(x).gameObject.SetActive(isOpen);
        }
    }
}
