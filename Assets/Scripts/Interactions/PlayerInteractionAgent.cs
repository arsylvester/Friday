using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[Serializable] public class PlayerSideInteractionEvent : UnityEvent<Interactable> { }

public class PlayerInteractionAgent : MonoBehaviour
{
    public string InteractKey;
    public Text NameTag;
    public float InteractRange;
    public Transform Player;

    private Camera mainCamera;
    private Interactable lastHovered;

    public PlayerSideInteractionEvent OnInteract;
    public PlayerSideInteractionEvent OnHoverStart;
    public PlayerSideInteractionEvent OnHoverEnd;

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
                    OnHoverEnd.Invoke(lastHovered);
                    lastHovered.OnHoverEnd.Invoke();
                    OnHoverStart.Invoke(interactable);
                    interactable.OnHoverStart.Invoke();
                }

                lastHovered = interactable;
                NameTag.text = interactable.DisplayName;

                Vector3 tpos = mainCamera.WorldToViewportPoint(interactable.transform.position + transform.right * interactable.DisplayOffset.x + transform.up * interactable.DisplayOffset.y);
                Vector2 canvasSize = NameTag.transform.parent.GetComponentInParent<RectTransform>().rect.size;
                NameTag.rectTransform.anchoredPosition = new Vector2(canvasSize.x * tpos.x, canvasSize.y * tpos.y);

                if (Input.GetButtonDown(InteractKey) && Vector3.Distance(Player.position, interactable.transform.position) <= InteractRange)
                {
                    OnInteract.Invoke(interactable);
                    interactable.OnInteract.Invoke();
                }
            }
            else
            {
                if (lastHovered != null)
                {
                    OnHoverEnd.Invoke(lastHovered);
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
                OnHoverEnd.Invoke(lastHovered);
                lastHovered.OnHoverEnd.Invoke();
                NameTag.text = "";
                lastHovered = null;
            }
        }
    }

    private void OnDisable()
    {
        if (lastHovered != null)
        {
            OnHoverEnd.Invoke(lastHovered);
            lastHovered.OnHoverEnd.Invoke();
            NameTag.text = "";
            lastHovered = null;
        }
    }
}
