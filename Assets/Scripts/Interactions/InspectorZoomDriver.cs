using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class InspectorZoomDriver : MonoBehaviour
{
    private Camera current;
    public Camera Target;
    // Start is called before the first frame update
    void Start()
    {
        current = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        current.fieldOfView = Target.fieldOfView;
    }
}
