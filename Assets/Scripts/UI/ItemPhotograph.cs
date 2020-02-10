using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPhotograph : MonoBehaviour
{
    [SerializeField] Image photo;
    [SerializeField] Text flavorText;
    [SerializeField] Camera mainCamera;

    public void ExpandImage(Sprite image, string flavor)
    {
        photo.sprite = image;
        flavorText.text = flavor;
        gameObject.SetActive(true);
    }

    public void CloseImage()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        /* Could use this if we don't want the image to disapear when clicking on specific things.
        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        bool hit = Physics.Raycast(cameraRay, out RaycastHit rayInfo);

        if(hit)
        {
            if(Input.GetMouseButtonDown(0) && !rayInfo.transform.GetComponent<ItemPhotograph>())
            {
                CloseImage();
            }
        } */

        if (Input.GetMouseButtonDown(0))
        {
            CloseImage();
        }
    }
}
