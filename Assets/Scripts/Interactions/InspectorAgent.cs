using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Cinemachine;

[Serializable] public class PlayerSideInspectEvent : UnityEvent<Inspectable> { }
public class InspectorAgent : MonoBehaviour
{
    public Text FlavorText;
    public string InteractKey;
    public CinemachineVirtualCameraBase InspectorFreeLook;
    public Camera InspectorCamera;

    public PlayerSideInspectEvent OnFocused;
    public PlayerSideInspectEvent OnUnfocused;

    private Inspectable target;
    private Camera mainCamera;
    void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        if(target != null)
        {
            if(Input.GetButtonDown(InteractKey))
            {
                
                Ray cameraRay = InspectorCamera.ScreenPointToRay(Input.mousePosition);
                bool hit = Physics.Raycast(cameraRay, out RaycastHit rayInfo, 100F, ~LayerMask.NameToLayer("Inspector"));

                if (!hit || rayInfo.collider.gameObject != target.gameObject)
                {
                    Debug.Log("A");
                    Release();
                }
            }
        }
    }
    public void Inspect(Interactable interactable)
    {
        Inspectable inspectable = interactable.GetComponent<Inspectable>();
        if(inspectable != null)
        {
            target = inspectable;
            FlavorText.text = target.InspectorText;
            InspectorFreeLook.LookAt = target.transform;
            InspectorFreeLook.Follow = target.transform;
            target.gameObject.layer = LayerMask.NameToLayer("Inspector");
            OnFocused.Invoke(target);
        }
    }

    public void Release()
    {
        OnUnfocused.Invoke(target);
        target.gameObject.layer = LayerMask.NameToLayer("Default");
        FlavorText.text = "";
        target = null;
    }
}
