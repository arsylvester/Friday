using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent OnInteract;
    public UnityEvent OnHoverStart;
    public UnityEvent OnHoverEnd;

    public string DisplayName;
}
