using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTextOnFlag : MonoBehaviour
{
    private Item item;
    private Journal journal;
    public string FlagName = "";
    public string PostFlagText = "";
    // Start is called before the first frame update
    void Start()
    {
        journal = FindObjectOfType<Journal>();
        item = GetComponent<Item>();
    }

    // Update is called once per frame
    void Update()
    {
        if(journal.HasCompletedDeduction(FlagName))
        {
            item.name = PostFlagText;
        }
    }
}
