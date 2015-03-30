using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameWin : MonoBehaviour {

    public Text gamewin;

    void Start()
    {
        gamewin.text = "You earned " + PlayerPrefs.GetInt("thisCoins").ToString() + " coins this attempt and killed " +  PlayerPrefs.GetInt("enemyKills").ToString() + " enemies";
        PlayerPrefs.SetInt("thisCoins", 0);
        PlayerPrefs.SetInt("enemyKills", 0);
    }
    
    public void retry()
    {
        Application.LoadLevel("shop");
    }
}

