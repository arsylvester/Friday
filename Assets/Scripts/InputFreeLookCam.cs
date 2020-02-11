using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Yarn.Unity;

public class InputFreeLookCam : MonoBehaviour
{
    bool isStopped = false;
    private bool isFreeLookActive;
    public string ActiveChannel;

    public void SwapChannel(string channel)
    {
        ActiveChannel = channel;
    }

    public int cameraZoomIn;
    public int cameraZoomOut;
    public float cameraZoomSmoothing;

    CinemachineFreeLook cineDynamicCam;

    DialogueUI runner;

    // Use this for initialization
    private void Start()
    {
        CinemachineCore.GetInputAxis = GetInputAxis;
        cineDynamicCam = GetComponent<CinemachineFreeLook>();

        runner = FindObjectOfType<DialogueUI>();
        runner.onDialogueStart.AddListener(FreezeCamera);
        runner.onDialogueEnd.AddListener(UnfreezeCamera);
    }

    private void Update()
    {
        if (!isStopped)
        {
            // allow player to rotate camera with right click
            isFreeLookActive = Input.GetMouseButton(1);

            // CameraZoom();
        }
    }

    float GetInputAxis(string axisName)
    {
        string mx = "Mouse X " + ActiveChannel;
        string my = "Mouse Y " + ActiveChannel;
        if (!isFreeLookActive || (mx != axisName && my != axisName))
            return 0;
        else
        {
            if (axisName == my)
                return Input.GetAxis("Mouse Y");
            else
                return Input.GetAxis("Mouse X");
        }
    }

    void CameraZoom()
    {
        // zoom in
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
            cineDynamicCam.m_Lens.FieldOfView = Mathf.Lerp(cineDynamicCam.m_Lens.FieldOfView, cameraZoomIn, Time.deltaTime * cameraZoomSmoothing);

        // zoom out
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
            cineDynamicCam.m_Lens.FieldOfView = Mathf.Lerp(cineDynamicCam.m_Lens.FieldOfView, cameraZoomOut, Time.deltaTime * cameraZoomSmoothing);
    }

    public void FreezeCamera()
    {
        isStopped = true;
    }

    public void UnfreezeCamera()
    {
        isStopped = false;
    }
}
