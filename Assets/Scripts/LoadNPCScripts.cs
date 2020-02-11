using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class LoadNPCScripts : MonoBehaviour
{
    public YarnProgram[] scriptsToLoad;

    void Start()
    {
        DialogueRunner runner = FindObjectOfType<DialogueRunner>();
        if (runner != null)
        {
            foreach(YarnProgram npc in scriptsToLoad)
            {
                runner.Add(npc);
            }
        }
    }
}
