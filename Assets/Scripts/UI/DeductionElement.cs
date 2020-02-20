using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeductionElement : MonoBehaviour
{
    [SerializeField] Text mainTextbox;
    private string mainText;
    private string summaryText;
    private DeductionSummary deductionSummary;

    public void SetUpDeduction(string mainTxt, string sumText, DeductionSummary panel)
    {
        mainTextbox.text = mainTxt;
        mainText = mainTxt;
        summaryText = sumText;
        deductionSummary = panel;
    }

    public void DeductionClicked()
    {
        deductionSummary.OpenSummary(mainText, summaryText);
    }
}
