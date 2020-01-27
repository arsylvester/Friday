using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inspectable : MonoBehaviour
{
    public UnityEvent OnFocused;
    public UnityEvent OnUnfocused;
    public string InspectorText;
}
