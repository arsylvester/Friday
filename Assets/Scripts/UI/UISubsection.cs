using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISubsection : MonoBehaviour
{
    private bool isOpen = true;
    [SerializeField] Image toggleIcon;
    [SerializeField] Sprite plusSprite;
    [SerializeField] Sprite minusSprite;

    public void ToggleChildern()
    {
        isOpen = !isOpen;
        for(int x = 0; x < transform.childCount; x++)
        {
            transform.GetChild(x).gameObject.SetActive(isOpen);
        }

        toggleIcon.gameObject.SetActive(true);
        if(isOpen)
            toggleIcon.sprite = minusSprite;
        else
            toggleIcon.sprite = plusSprite;
    }
}
