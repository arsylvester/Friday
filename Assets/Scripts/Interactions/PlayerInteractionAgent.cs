using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Yarn.Unity;

[Serializable] public class PlayerSideInteractionEvent : UnityEvent<Interactable> { }

public class PlayerInteractionAgent : MonoBehaviour
{
    public string InteractKey;
    public Text NameTag;
    public float InteractRange;
    public Transform Player;

    public Texture2D InteractCursor;
    public Texture2D NearCursor;
    bool isInteracted;

    private Camera mainCamera;
    private Interactable lastHovered;

    public PlayerSideInteractionEvent OnInteract;
    public PlayerSideInteractionEvent OnHoverStart;
    public PlayerSideInteractionEvent OnHoverEnd;

    DialogueUI runner;

    void Start()
    {
        mainCamera = GetComponent<Camera>();

        runner = FindObjectOfType<DialogueUI>();
        runner.onDialogueEnd.AddListener(AllowInteraction);
    }

    void Update()
    {
        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        bool hit = Physics.Raycast(cameraRay, out RaycastHit rayInfo);

        if (hit)
        {
            GameObject objectHit = rayInfo.collider.gameObject;
            Interactable interactable = objectHit.GetComponent<Interactable>();

            if (interactable != null && interactable.enabled)
            {
                bool near = Vector3.Distance(Player.position, interactable.transform.position) <= InteractRange;

                if (Input.GetButtonDown(InteractKey) && near)
                {
                    isInteracted = true;
                    Cursor.SetCursor(InteractCursor, Vector2.zero, CursorMode.Auto);
                    StartCoroutine(StartInteraction(interactable));
                }

                if (!isInteracted)
                {
                    if (near)
                    {
                        Cursor.SetCursor(NearCursor, Vector2.zero, CursorMode.Auto);
                    }
                    else
                    {
                        // Cursor.SetCursor(FarCursor, Vector2.zero, CursorMode.Auto);
                        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                    }
                }

                if (lastHovered != null && lastHovered != interactable)
                {
                    OnHoverEnd.Invoke(lastHovered);
                    lastHovered.OnHoverEnd.Invoke();
                }

                if(lastHovered != interactable)
                {
                    OnHoverStart.Invoke(interactable);
                    interactable.OnHoverStart.Invoke();
                }

                lastHovered = interactable;
                NameTag.text = interactable.DisplayName;

                Vector3 tpos = mainCamera.WorldToViewportPoint(interactable.transform.position + transform.right * interactable.DisplayOffset.x + transform.up * interactable.DisplayOffset.y);
                Vector2 canvasSize = NameTag.transform.parent.GetComponentInParent<RectTransform>().rect.size;
                NameTag.rectTransform.anchoredPosition = new Vector2(canvasSize.x * tpos.x, canvasSize.y * tpos.y);
            }
            else if (!isInteracted)
            {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                if (lastHovered != null)
                {
                    OnHoverEnd.Invoke(lastHovered);
                    lastHovered.OnHoverEnd.Invoke();
                    Debug.Log("UNHOVER");
                    NameTag.text = "";
                    lastHovered = null;
                }
            }
        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            if(lastHovered != null)
            {
                OnHoverEnd.Invoke(lastHovered);
                lastHovered.OnHoverEnd.Invoke();
                Debug.Log("UNHOVER");
                NameTag.text = "";
                lastHovered = null;
            }
        }
    }

    IEnumerator StartInteraction(Interactable interactable)
    {
        yield return new WaitForSeconds(0.1f);
        isInteracted = false;
        Interact(interactable);

    }

    public void Interact(Interactable interactable)
    {
        OnInteract.Invoke(interactable);
        interactable.OnInteract.Invoke();
    }

    private void OnDisable()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        if (lastHovered != null)
        {
            OnHoverEnd.Invoke(lastHovered);
            lastHovered.OnHoverEnd.Invoke();
            Debug.Log("UNHOVER");
            NameTag.text = "";
            lastHovered = null;
        }
    }

    public void AllowInteraction()
    {
        this.enabled = true;
    }
}
