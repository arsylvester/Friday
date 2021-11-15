using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Yarn.Unity;

public class HoverOver : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI objectText;
    private DialogueRunner dialogue;
    // hey
    // Start is called before the first frame update
    void Start()
    {
        dialogue = FindObjectOfType<DialogueRunner>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        objectText.gameObject.transform.position = new Vector3(mousePos.x, mousePos.y + 50, mousePos.z);
    }

    private void OnMouseEnter()
    {
        transform.localScale = new Vector3(transform.localScale.x + 0.2f, transform.localScale.y + 0.2f, transform.localScale.z + 0.2f);
        objectText.text = transform.name;  
    }
    private void OnMouseExit()
    {
        transform.localScale = new Vector3(transform.localScale.x - 0.2f, transform.localScale.y - 0.2f, transform.localScale.z - 0.2f);
        objectText.text = "";
    }
    private void OnMouseDown()
    {
        objectText.text = "";
    }
}
