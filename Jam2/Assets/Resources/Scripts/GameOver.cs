using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOver : MonoBehaviour {

    public Text gameOver;

    void Start()
    {
        gameOver.text = "You earned" + PlayerPrefs.GetInt("thisCoins").ToString() + "coins this attempt, try buying some upgrades  in the shop.";
        PlayerPrefs.SetInt("thisCoins", 0);
    }
    
    public void retry()
    {
        Application.LoadLevel("shop");
    }
}
