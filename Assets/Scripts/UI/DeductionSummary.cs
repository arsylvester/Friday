using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeductionSummary : MonoBehaviour
{
    [SerializeField] Text summaryText;
    [SerializeField] Text mainText;

    private bool open = false;

    public void OpenSummary(string mText, string sumText)
    {
        mainText.text = mText;
        summaryText.text = sumText;
        gameObject.SetActive(true);
        open = true;
    }

    public void CloseSummary()
    {
        gameObject.SetActive(false);
        open = false;
    }

    private void Update()
    {
        if (open && Input.GetMouseButtonDown(0))
        {
            CloseSummary();
        }
    }
}
