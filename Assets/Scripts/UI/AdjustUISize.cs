using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdjustUISize : MonoBehaviour
{
    [SerializeField] RectTransform childRect;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform =  GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.rect.Set(0,0,350,childRect.rect.height);
    }
}
