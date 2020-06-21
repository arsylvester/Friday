using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemAgent : MonoBehaviour
{
    //public string RecordKey;
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

    private Image cameraFlash;

    ////////////////////////////////////////////
    ///TUTORIAL ONLY
    InspectorTutorial tutorial;
    private void Start()
    {
        tutorial = GetComponent<InspectorTutorial>();
        cameraFlash = GameObject.Find("CameraFlash").GetComponent<Image>();
    }
    ////////////////////////////////////////////
    
    // Update is called once per frame
    void Update()
    {
        if(item != null)
        {
            FlavorText.text = item.FlavorText;

            //if(Input.GetButtonDown(RecordKey))
            if(Input.GetKeyDown(KeyCode.F))
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
                StartCoroutine(Flash());
                ////////////////////////////////////////////
                ///TUTORIAL ONLY
                if(tutorial != null) tutorial.RecordItem();
                ////////////////////////////////////////////
                if (secret != null)
                {
                    ////////////////////////////////////////////
                    ///TUTORIAL ONLY
                    if(tutorial != null) tutorial.RecordSecret();
                    ////////////////////////////////////////////
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

    public IEnumerator Flash()
    {
        float time = 0;
        while (time < 1)
        {
            time += Time.deltaTime;
            cameraFlash.color = Color.Lerp(Color.clear, Color.white, time);
            yield return null;
        }
        while (time > 0)
        {
            time -= Time.deltaTime;
            cameraFlash.color = Color.Lerp(Color.clear, Color.white, time);
            yield return null;
        }
    }
}
