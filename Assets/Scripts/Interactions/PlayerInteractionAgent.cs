using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractionAgent : MonoBehaviour
{
    public string InteractKey;
    public Text NameTag;

    private Camera mainCamera;
    private Interactable lastHovered;
    
    void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    void Update()
    {
        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        bool hit = Physics.Raycast(cameraRay, out RaycastHit rayInfo);

        if (hit)
        {
            GameObject objectHit = rayInfo.collider.gameObject;
            Interactable interactable = objectHit.GetComponent<Interactable>();

            if(interactable != null)
            {
                if (lastHovered != null && lastHovered != interactable)
                {
                    lastHovered.OnHoverEnd.Invoke();
                    interactable.OnHoverStart.Invoke();
                }

                lastHovered = interactable;
                NameTag.text = interactable.DisplayName;

                if (Input.GetButtonDown(InteractKey))
                {
                    interactable.OnInteract.Invoke();
                }
            }
            else
            {
                if (lastHovered != null)
                {
                    lastHovered.OnHoverEnd.Invoke();
                    NameTag.text = "";
                    lastHovered = null;
                }
            }
        }
        else
        {
            if(lastHovered != null)
            {
                lastHovered.OnHoverEnd.Invoke();
                NameTag.text = "";
                lastHovered = null;
            }
        }
    }
}
