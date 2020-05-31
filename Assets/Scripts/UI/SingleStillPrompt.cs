using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleStillPrompt : MonoBehaviour
{
    private Text text;
    public GameObject MapUI;
    // Start is called before the first frame update

    private PlayerInteractionAgent agent;
    private PlayerMovement movement;
    private InputFreeLookCam cam;

    void Start()
    {
        text = GetComponent<Text>();
        text.enabled = true;
        text.text += "\n\n(Left click to continue)";

        agent = FindObjectOfType<PlayerInteractionAgent>();
        movement = FindObjectOfType<PlayerMovement>();
        cam = FindObjectOfType<InputFreeLookCam>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.enabled = false;
        movement.StopMovement();
        cam.FreezeCamera();
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            MapUI.SetActive(true);
        }
    }
}
