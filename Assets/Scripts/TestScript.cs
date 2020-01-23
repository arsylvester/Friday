using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.SetSiblingIndex(0);
        print(transform.childCount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
