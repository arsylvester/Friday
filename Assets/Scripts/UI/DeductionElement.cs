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

    //For objectives
    public string ObjKey = "";

    public void SetUpDeduction(string mainTxt, string sumText, DeductionSummary panel, string key)
    {
        mainTextbox.text = mainTxt;
        mainText = mainTxt;
        summaryText = sumText;
        deductionSummary = panel;
        ObjKey = key;

        mainTextbox.color = Color.magenta;
    }

    public void DeductionClicked()
    {
        deductionSummary.OpenSummary(mainText, summaryText);
        mainTextbox.color = Color.black;
    }
}
