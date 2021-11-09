using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverHighlight : BaseMeshEffect, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] bool UseImage;
	[SerializeField] Image OutlineImage;
	[SerializeField] float OutlineAlpha = 0.5f;
    [SerializeField] Color OutlineColor = new Color (0f, 0f, 0f, 0.5f);
	[SerializeField] Vector2 OutlineDistance = new Vector2 (1f, -1f);
	[SerializeField] bool UseGraphicAlpha = true;
	private List<UIVertex> verts = new List<UIVertex>();

	private Color EffectColor
	{
		get => OutlineColor;
		set
		{
			OutlineColor = value;
			if (graphic != null)
			{
				graphic.SetVerticesDirty();
			}
		}
	}

	private Vector2 EffectDistance
	{
		get => OutlineDistance;
		set
		{
			if (value.x > 600f)
			{
				value.x = 600f;
			}
			if (value.x < -600f)
			{
				value.x = -600f;
			}
			if (value.y > 600f)
			{
				value.y = 600f;
			}
			if (value.y < -600f)
			{
				value.y = -600f;
			}
			if (OutlineDistance == value)
			{
				return;
			}
			OutlineDistance = value;
			if (graphic != null)
			{
				graphic.SetVerticesDirty ();
			}
		}
	}

	private bool GraphicAlpha
	{
		get => UseGraphicAlpha;
		set
		{
			UseGraphicAlpha = value;
			if (graphic != null)
			{
				graphic.SetVerticesDirty();
			}
		}
	}

	private void Awake()
	{
		base.Awake();
		if (OutlineImage == null)
			UseImage = false;
		OnPointerExit(null);
	}

    protected void ApplyShadowZeroAlloc(List<UIVertex> verts, Color32 color, int start, int end, float x, float y)
    {
        UIVertex vertex;
        var capacity = verts.Count * 2;
        if (verts.Capacity < capacity)
            verts.Capacity = capacity;

        for (int i = start; i < end; ++i)
        {
            vertex = verts[i];
            verts.Add(vertex);
            var position = vertex.position;
            position.x += x;
            position.y += y;
            vertex.position = position;
            var newColor = color;
            if (UseGraphicAlpha)
                newColor.a = (byte)(newColor.a * verts[i].color.a / 255);
            vertex.color = newColor;
            verts[i] = vertex;
        }
    }

    protected void ApplyShadow(List<UIVertex> verts, Color32 color, int start, int end, float x, float y)
    {
        var capacity = verts.Count * 2;
        if (verts.Capacity < capacity)
            verts.Capacity = capacity;
        ApplyShadowZeroAlloc(verts, color, start, end, x, y);
    }


    public override void ModifyMesh(VertexHelper helper)
    {
        if (!IsActive ())
	        return;

        verts.Clear();
        helper.GetUIVertexStream(verts);
        var text = GetComponent<Text>();
        float fitAdjustment = 1f;
        if (text && text.resizeTextForBestFit)
		{
			fitAdjustment = (float)text.cachedTextGenerator.fontSizeUsedForBestFit / (text.resizeTextMaxSize-1);
		}

		float distanceX = EffectDistance.x * fitAdjustment;
		float distanceY = EffectDistance.y * fitAdjustment;
		int start = 0;
		int count = verts.Count;
		ApplyShadow (verts, EffectColor, start, verts.Count, distanceX, distanceY);
		start = count;
		count = verts.Count;
		ApplyShadow (verts, EffectColor, start, verts.Count, distanceX, -distanceY);
		start = count;
		count = verts.Count;
		ApplyShadow (verts, EffectColor, start, verts.Count, -distanceX, distanceY);
		start = count;
		count = verts.Count;
		ApplyShadow (verts, EffectColor, start, verts.Count, -distanceX, -distanceY);
		start = count;
		count = verts.Count;
		ApplyShadow (verts, EffectColor, start, verts.Count, distanceX, 0);
		start = count;
		count = verts.Count;
		ApplyShadow (verts, EffectColor, start, verts.Count, -distanceX, 0);
		start = count;
		count = verts.Count;
		ApplyShadow (verts, EffectColor, start, verts.Count, 0, distanceY);
		start = count;
		count = verts.Count;
		ApplyShadow (verts, EffectColor, start, verts.Count, 0, -distanceY);
		helper.Clear();
        helper.AddUIVertexTriangleStream(verts);
    }

#if UNITY_EDITOR
	protected override void OnValidate ()
	{
		EffectDistance = OutlineDistance;
		base.OnValidate ();
	}
#endif

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (UseImage)
		{
			var imageColor = OutlineImage.color;
			imageColor.a = 1;
			OutlineImage.color = imageColor;
		}
		else
		{
			var color = OutlineColor;
			color.a = OutlineAlpha;
			EffectColor = color;	
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (UseImage)
		{
			var imageColor = OutlineImage.color;
			imageColor.a = 0;
			OutlineImage.color = imageColor;
		}
		else
		{
			var color = OutlineColor;
			color.a = 0f;
			EffectColor = color;	
		}
	}
}
