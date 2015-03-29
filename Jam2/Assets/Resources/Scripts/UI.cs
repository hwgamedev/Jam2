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
    public Slider healthBarSlider;
    private int maxHealth;
    public static UI Instance;


    void Awake()
    {
        if (Instance == null) Instance = this;
    }


    public void init()
    {
        maxHealth = PlayerPrefs.GetInt("maxHealth");
        nameUpdate();
        healthUpdate();
        coinUpdate();
        enemyKillUpdate();
        healthPotionUpdate();
        stepIncreaserUpdate();
        stepDecreaserUpdate();
        teleporterUpdate();
        stepUpdate();
    }

    public void nameUpdate()
    {
        nameText.text = Player.Instance.getName();
    }
    public void healthUpdate()
    {
        int health = Player.Instance.getHealth();
        float percent = health / maxHealth;
        healthBarSlider.normalizedValue = percent;
    }

    public void coinUpdate()
    {
        int coins = Player.Instance.getCoins();
        if (coins > 1000) goldText.text = coins.ToString();
        else goldText.text = "0" + coins.ToString();
    }

    public void enemyKillUpdate()
    {
        float percent = Player.Instance.getEnemyPercentKilled();
        if (percent > 9) killText.text = percent + "%"; 
        killText.text = "0" + percent + "%"; 
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
}
