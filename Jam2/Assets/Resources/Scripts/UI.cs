using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UI : MonoBehaviour {

    public Text nameText;
    public Text goldText;
    public Text stepsText;
    public Text killText;
    public Text healthText;
    public Text healthPotionQuantity;
    public Text increaserQuantity;
    public Text decreaserQuantity;
    public Text teleporterQuantity;
    public Image buff;
    public Image debuff1;
    public Image debuff2;
    public Button healthPotion;
    public Button increaser;
    public Button decreaser;
    public Button teleporter;
    public static UI Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void init()
    {
        nameUpdate();
        healthUpdate();
        coinUpdate();
        enemyKillUpdate();
        healthPotionUpdate();
        stepIncreaserUpdate();
        stepDecreaserUpdate();
        teleporterUpdate();
        stepUpdate();
        speedBuffUpdate();
        poisonDebuffUpdate();
        slowedDebuffUpdate();
    }

    public void nameUpdate()
    {
        nameText.text = Player.Instance.getName();
    }
    public void healthUpdate()
    {
        healthText.text = Player.Instance.getHealth().ToString();
    }

    public void coinUpdate()
    {
        int coins = Player.Instance.getCoins();
        if (coins > 9) goldText.text = coins.ToString();
        else goldText.text = "0" + coins.ToString();
    }

    public void enemyKillUpdate()
    {
        killText.text = Player.Instance.getEnemyPercentKilled() + "%"; 
    }

    public void healthPotionUpdate()
    {
        int item = Player.Instance.getHealthPotions();
        healthPotionQuantity.text = item.ToString();
        if (item == 0) healthPotion.interactable = false;
        else healthPotion.interactable = true;
    }

    public void stepIncreaserUpdate()
    {
        int item = Player.Instance.getStepIncreasers();
        increaserQuantity.text = item.ToString();
        if (item == 0) increaser.interactable = false;
        else increaser.interactable = true;
    }

    public void stepDecreaserUpdate()
    {
        int item = Player.Instance.getStepDecreasers();
        decreaserQuantity.text = item.ToString();
        if (item == 0) decreaser.interactable = false;
        else decreaser.interactable = true;
    }

    public void teleporterUpdate()
    {
        int item = Player.Instance.getTeleporters();
        teleporterQuantity.text = item.ToString();
        if (item == 0) teleporter.interactable = false;
        else teleporter.interactable = true;
    }

    public void stepUpdate()
    {
        stepsText.text = Player.Instance.getSteps().ToString();
    }

    public void speedBuffUpdate()
    {
        buff.enabled = Player.Instance.getBuff();
    }

    public void poisonDebuffUpdate()
    {
        debuff1.enabled = Player.Instance.getDebuff1();
    }

    public void slowedDebuffUpdate()
    {
        debuff2.enabled = Player.Instance.getDebuff2();
    }
}
