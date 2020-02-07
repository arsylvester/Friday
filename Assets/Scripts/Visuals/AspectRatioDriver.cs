using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectRatioDriver : MonoBehaviour
{
    public int Width = 16;
    public int Height = 9;
    Camera mainCamera;
    // Start is called before the first frame update
    int lastWidth;
    int lastHeight;
    void Start()
    {
        mainCamera = GetComponent<Camera>();
        lastWidth = Screen.width;
        lastHeight = Screen.height;
        UpdateCameraRect();
    }

    // Update is called once per frame
    void Update()
    {
        int w = Screen.width, h = Screen.height;
        if(w != lastWidth || h != lastHeight)
        {
            UpdateCameraRect();
        }
        lastWidth = w;
        lastHeight = h;
    }

    public void UpdateCameraRect()
    {
        Rect rect = mainCamera.pixelRect;
        Vector2 pos = Vector3.zero;
        Vector2 size = Vector2.zero;
        float w = Screen.width, h = Screen.height;
        if (w * Height > h * Width)
        {
            size.x = h * Width / Height;
            pos.x = (w - size.x) / 2;
            size.y = h;
        }
        else
        {
            size.x = w;
            size.y = w * Height / Width;
            pos.y = (h - size.y) / 2;
        }
        rect.position = pos;
        rect.size = size;
        mainCamera.pixelRect = rect;
    }
}
