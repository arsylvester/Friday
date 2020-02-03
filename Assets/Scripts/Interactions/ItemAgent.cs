using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemAgent : MonoBehaviour
{
    public string RecordKey;
    public Camera CaptureCamera;
    public Text FlavorText;

    private Item item;
    public void FocusItem(Inspectable item)
    {
        this.item = item as Item;
    }

    public void UnfocusItem()
    {
        this.item = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(item != null)
        {
            FlavorText.text = item.FlavorText;

            if(Input.GetButtonDown(RecordKey))
            {
                SecretItem secret = null;
                foreach (SecretItem s in item.GetComponentsInChildren<SecretItem>())
                {
                    if(!Physics.Linecast(transform.position, s.transform.position, LayerMask.GetMask("Inspector")))
                    {
                        secret = s;
                    }
                }

                Debug.Log("Record");
                CaptureCamera.Render();
                RenderTexture rt = CaptureCamera.targetTexture;
                Texture2D td = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false, true);
                td.Apply(false);
                Graphics.CopyTexture(rt, td);
                Sprite capture = Sprite.Create(td, new Rect(0, 0, rt.width, rt.height), new Vector2(rt.width / 2.0F, rt.height / 2.0F));

                Journal jrnl = FindObjectOfType<Journal>();

                if (secret != null)
                {
                    Debug.Log("Secret");
                    jrnl.SaveItem(secret.EntryName, secret.Description, secret.FlavorText, capture, secret.Keyname);
                }
                else
                {
                    jrnl.SaveItem(item.EntryName, item.Description, item.FlavorText, capture, item.Keyname);
                }
            }
        }
        else
        {
            FlavorText.text = "";
        }
    }
}
