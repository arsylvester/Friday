using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.SceneManagement;

public class YarnCustomCommands : MonoBehaviour
{
    public GameObject forestEntry;
    public GameObject fairies;
    public GameObject wallet;
    public Animator anim;
    //activate forest entry
    [YarnCommand("entry")]
    public void Entry()
    {
        forestEntry.SetActive(true);
    }
    //spawn fairies
    [YarnCommand("fairy")]
    public void Fairy()
    {
        fairies.SetActive(true);
    }
    //screen fade
    [YarnCommand("fadeIn")]
    public void FadeIn()
    {
        anim.SetTrigger("FadeClear");
    }

    [YarnCommand("fadeOut")]
    public void FadeOut()
    {
        anim.SetTrigger("FadeBlack");
    }

    [YarnCommand("wallet")]
    public void Wallet()
    {
        wallet.SetActive(false);
    }

    [YarnCommand("theEnd")]
    public void theEnd()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
