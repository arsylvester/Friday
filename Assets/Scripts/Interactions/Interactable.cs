using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    public UnityEvent OnInteract;
    public UnityEvent OnHoverStart;
    public UnityEvent OnHoverEnd;

    public string DisplayName;
    public Vector2 DisplayOffset;
    public GameObject NameTag;

    private Canvas Canvas;
    private Camera mainCamera;
    private PlayerMovement player;
    private PlayerInteractionAgent agent;

    public void Awake()
    {
        NameTag = Instantiate(NameTag, transform, false);
        agent = FindObjectOfType<PlayerInteractionAgent>();
        Canvas = agent.Canvas;
        mainCamera = agent.GetComponent<Camera>();
        player = FindObjectOfType<PlayerMovement>();
    }

    private void Update()
    {
        Vector3 tpos = mainCamera.WorldToViewportPoint(transform.position);
        Vector2 canvasSize = Canvas.GetComponent<RectTransform>().rect.size;
        NameTag.GetComponent<RectTransform>().anchoredPosition = new Vector2(canvasSize.x * tpos.x + DisplayOffset.x, canvasSize.y * tpos.y + DisplayOffset.y);
        
        if(Vector3.Distance(player.transform.position, transform.position) <= agent.InteractRange)
        {
            NameTag.GetComponent<Text>().text = DisplayName;
        }
        else
        {
            NameTag.GetComponent<Text>().text = "";
        }
    }
}
