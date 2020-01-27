using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorFollow : MonoBehaviour
{
    private RectTransform rt;
    void Start()
    {
        rt = GetComponent<RectTransform>();
    }
    void Update()
    {
        Vector3 mp = Input.mousePosition;
        Vector2 ap = rt.anchoredPosition;
        Vector2 cs = transform.parent.GetComponentInParent<RectTransform>().rect.size;
        Vector2 ts = rt.rect.size;
        ap.x = Mathf.Min(Screen.width - ts.x / 2, mp.x / Screen.width * cs.x + ts.x / 2);
        ap.y = Mathf.Max(ts.y / 2, mp.y / Screen.height * cs.y - ts.y / 2);
        rt.anchoredPosition = ap;
    }
}
