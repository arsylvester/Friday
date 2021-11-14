using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Yarn.Unity;

public class GameController : MonoBehaviour
{
    public GameObject Lot;
    public GameObject Park;
    public GameObject Forest;
    public GameObject DeepF;
    public TextMeshProUGUI TitleText;


    [SerializeField] AudioClip NormalBackgroundMusicClip;
    [SerializeField] AudioClip FairyBackgroundMusicClip;
    [SerializeField] AudioSource AudioSource;

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

        PlayBackgroundMusic();
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
            PlayBackgroundMusic();
            //fade
        }

        if (area == "ToParkingLot")
        {
            //fade
            Park.SetActive(false);
            TitleText.text = "Parking Lot";
            Lot.SetActive(true);
            PlayBackgroundMusic();
            //fade
        }

        if (area == "ToForest")
        {
            //fade
            Park.SetActive(false);
            TitleText.text = "Forest";
            Forest.SetActive(true);
            AudioSource.Stop();
            //fade
        }

        if (area == "ToDeepForest")
        {
            //fade
            Forest.SetActive(false);
            TitleText.text = "Deep Forest";
            DeepF.SetActive(true);
            PlayFairyMusic();
            //fade
        }
    }

    private void PlayBackgroundMusic()
    {
        if (AudioSource.clip != NormalBackgroundMusicClip || !AudioSource.isPlaying)
        {
            AudioSource.Stop();
            AudioSource.clip = NormalBackgroundMusicClip;
            AudioSource.Play();
        }
    }

    private void PlayFairyMusic()
    {
        if (AudioSource.clip != FairyBackgroundMusicClip || !AudioSource.isPlaying)
        {
            AudioSource.Stop();
            AudioSource.clip = FairyBackgroundMusicClip;
            AudioSource.Play();
        }
    }

}
