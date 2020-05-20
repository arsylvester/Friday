using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleStillPrompt : MonoBehaviour
{
    private Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        text.enabled = true;
        text.text += "\n\n(Left click to continue)";

        FindObjectOfType<PlayerInteractionAgent>().enabled = false;
        FindObjectOfType<PlayerMovement>().StopMovement();
        FindObjectOfType<InputFreeLookCam>().FreezeCamera();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            text.enabled = false;
            FindObjectOfType<PlayerInteractionAgent>().enabled = true;
            FindObjectOfType<InputFreeLookCam>().UnfreezeCamera();
            FindObjectOfType<PlayerMovement>().ResumeMovement();
            this.enabled = false;
        }
    }
}
