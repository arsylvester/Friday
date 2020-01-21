using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class InputFreeLookCam : MonoBehaviour
{
    private bool isFreeLookActive;

    public int cameraZoomIn;
    public int cameraZoomOut;
    public float cameraZoomSmoothing;

    CinemachineFreeLook cam;

    // Use this for initialization
    private void Start()
    {
        CinemachineCore.GetInputAxis = GetInputAxis;

        cam = GetComponent<CinemachineFreeLook>();
    }

    private void Update()
    {
        // allow player to rotate camera with right click
        isFreeLookActive = Input.GetMouseButton(1);

        CameraZoom();
    }

    float GetInputAxis(string axisName)
    {
        if (!isFreeLookActive)
            return 0;
        else
        {
            if (axisName == "Mouse Y")
                return Input.GetAxis("Mouse Y");
            else
                return Input.GetAxis("Mouse X");
        }
    }

    void CameraZoom()
    {
        // zoom in
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
            cam.m_Lens.FieldOfView = Mathf.Lerp(cam.m_Lens.FieldOfView, cameraZoomIn, Time.deltaTime * cameraZoomSmoothing);

        // zoom out
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
            cam.m_Lens.FieldOfView = Mathf.Lerp(cam.m_Lens.FieldOfView, cameraZoomOut, Time.deltaTime * cameraZoomSmoothing);
    }
}
