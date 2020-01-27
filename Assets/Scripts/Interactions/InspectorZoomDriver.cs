using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class InspectorZoomDriver : MonoBehaviour
{
    private CinemachineVirtualCamera current;
    public CinemachineFreeLook Target;
    // Start is called before the first frame update
    void Start()
    {
        current = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        current.m_Lens.FieldOfView = Target.m_Lens.FieldOfView;
    }
}
