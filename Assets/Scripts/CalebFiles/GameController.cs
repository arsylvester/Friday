using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public GameObject Lot;
    public GameObject Park;
    public GameObject Forest;
    public GameObject DeepF;
    public TextMeshProUGUI TitleText;

    // Start is called before the first frame update
    void Start()
    {
        Lot = GameObject.Find("ParkingLotObjs");
        Park = GameObject.Find("ParkObjs");
        Forest = GameObject.Find("ForestObjs");
        DeepF = GameObject.Find("DeepForestObjs");


        Park.SetActive(false);
        Forest.SetActive(false);
        DeepF.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeArea(string area)
    {
        if (area == "ToPark")
        {
            //fade
            Lot.SetActive(false);
            TitleText.text = "Park";
            Park.SetActive(true);
            //fade
        }

        if (area == "ToParkingLot")
        {
            //fade
            Park.SetActive(false);
            TitleText.text = "Parking Lot";
            Lot.SetActive(true);
            //fade
        }

        if (area == "ToForest")
        {
            //fade
            Park.SetActive(false);
            TitleText.text = "Forest";
            Forest.SetActive(true);
            //fade
        }

        if (area == "ToDeepForest")
        {
            //fade
            Forest.SetActive(false);
            TitleText.text = "Deep Forest";
            DeepF.SetActive(true);
            //fade
        }
    }
}
