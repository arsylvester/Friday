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

    public float InspectionDistance;
    public float FocusTime;
    public Text FlavorText;
    public string InteractKey;
    public string VerticalRotationAxis;
    public string HorizontalRotationAxis;
    public string DragKey;
    public Camera InspectorCamera;

    public PlayerSideInspectEvent OnFocused;
    public PlayerSideInspectEvent OnUnfocused;

    private IEnumerator focusCoroutine;
    private Inspectable target;

    private Dictionary<Inspectable, IEnumerator> unfocusCoroutines = new Dictionary<Inspectable, IEnumerator>();
    private Dictionary<Inspectable, Quaternion> originalRotations = new Dictionary<Inspectable, Quaternion>();
    private Dictionary<Inspectable, Vector3> originalPositions = new Dictionary<Inspectable, Vector3>();

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
                    Release();
                }
            }

            if(Input.GetButton(DragKey))
            {
                target.transform.Rotate(transform.right, Input.GetAxis(VerticalRotationAxis), Space.World);
                target.transform.Rotate(transform.up, Input.GetAxis(HorizontalRotationAxis), Space.World);
            }
        }
    }

    public static float Sirp(float y0, float y1, float t)
    {
        float dy = y1 - y0;
        return (Mathf.Sin((Mathf.Clamp01(t) + 1.5F) * Mathf.PI) + 1) * dy / 2 + y0;
    }

    public void Inspect(Interactable interactable)
    {
        Inspectable inspectable = interactable.GetComponent<Inspectable>();
        if(inspectable != null)
        {
            target = inspectable;

            if(unfocusCoroutines.ContainsKey(inspectable) && unfocusCoroutines[inspectable] != null)
            {
                StopCoroutine(unfocusCoroutines[inspectable]);
            }
            else
            {
                Transform originalTransform = target.transform;
                originalPositions[inspectable] = originalTransform.position;
                originalRotations[inspectable] = originalTransform.rotation;
            }
            
            FlavorText.text = target.InspectorText;
            OnFocused.Invoke(target);
            focusCoroutine = FocusObject(target.gameObject);
            StartCoroutine(focusCoroutine);
        }
    }

    public IEnumerator FocusObject(GameObject target)
    {
        float t = 0;
        Vector3 targetPosition;
        Vector3 startPosition = target.transform.position;
        target.layer = LayerMask.NameToLayer("Inspector");

        do
        {
            t += Time.deltaTime;
            targetPosition = transform.position + transform.forward * InspectionDistance;
            target.transform.position = Vector3.Lerp(startPosition, targetPosition, Sirp(0, 1, t / FocusTime));
            yield return null;

        } while (t < FocusTime);

        Debug.Log("Focused");
    }

    public IEnumerator UnfocusObject(GameObject target, Quaternion originalRotation, Vector3 originalPosition)
    {
        float t = 0;
        Vector3 originalEuler = originalRotation.eulerAngles;
        Vector3 focusAngularVelocity = Vector3.zero;
        Vector3 startingPosition = target.transform.position;
        Vector3 startingEuler = target.transform.rotation.eulerAngles;

        do
        {
            t += Time.deltaTime;
            target.transform.position = Vector3.Lerp(startingPosition, originalPosition, Sirp(0, 1, t / FocusTime));
            Vector3 eulerRotation = target.transform.rotation.eulerAngles;
            eulerRotation.x = Sirp(startingEuler.x, originalEuler.x, t / FocusTime);
            eulerRotation.y = Sirp(startingEuler.y, originalEuler.y, t / FocusTime);
            eulerRotation.z = Sirp(startingEuler.z, originalEuler.z, t / FocusTime);
            yield return null;
        } while (t < FocusTime);

        target.layer = LayerMask.NameToLayer("Default");
        Debug.Log("Unfocused");
    }

    public void Release()
    {
        if(focusCoroutine != null)
        {
            StopCoroutine(focusCoroutine);
        }

        OnUnfocused.Invoke(target);
        FlavorText.text = "";

        IEnumerator unfocusCoroutine = UnfocusObject(target.gameObject, originalRotations[target], originalPositions[target]);
        unfocusCoroutines[target] = unfocusCoroutine;
        StartCoroutine(unfocusCoroutine);
        target = null;
    }
}
