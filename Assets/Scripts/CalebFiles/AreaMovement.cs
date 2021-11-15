using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class AreaMovement : MonoBehaviour
{
    public GameObject gameManager;
    private DialogueRunner dialogue;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        dialogue = FindObjectOfType<DialogueRunner>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if(!dialogue.isDialogueRunning)
        {
            if (transform.name == "ToPark")
            {
                gameManager.GetComponent<GameController>().changeArea("ToPark");
            }

            if (transform.name == "ToParkingLot")
            {
                gameManager.GetComponent<GameController>().changeArea("ToParkingLot");
            }

            if (transform.name == "ToForest")
            {
                gameManager.GetComponent<GameController>().changeArea("ToForest");
            }

            if (transform.name == "ToDeepForest")
            {
                gameManager.GetComponent<GameController>().changeArea("ToDeepForest");
            }
        }
        
    }
}
