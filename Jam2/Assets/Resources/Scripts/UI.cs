using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UI : MonoBehaviour {

    public Text room;
    public Text nameText;
    public Text goldText;
    public Text stepsText;
    public Text stepsTextBig;
    public Text killText;
    public Text healthPotionQuantity;
    public Text increaserQuantity;
    public Text decreaserQuantity;
    public Text teleporterQuantity;
    public Button healthPotion;
    public Button increaser;
    public Button decreaser;
    public Button teleporter;
    public Slider healthBarSlider;
    private int maxHealth;
    public static UI Instance;
    public GameObject help;
    public GameObject help2;
    public Text ammoText;
    public GameObject ammoDisplay;



    void Awake()
    {
        if (Instance == null) Instance = this;
        if(PlayerPrefs.GetInt("newPlayer") == 1)
        {
            Time.timeScale = 0;
            help.SetActive(true);
            help2.SetActive(false);
        }
        else
        {
            help.SetActive(false);
            help2.SetActive(false);
        }
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
        updateAmmo();
    }

    public void helpNext()
    {
        help2.SetActive(true);
        help.SetActive(false);
    }

    public void helpBack()
    {
        help2.SetActive(false);
        help.SetActive(true);
    }

    public void helpClose()
    {
        help.SetActive(false);
        help2.SetActive(false);
        Time.timeScale = 1;
    }

    public void helpOpen()
    {
        Time.timeScale = 0;
        help.SetActive(true);
        help2.SetActive(false);
    }

    public void nameUpdate()
    {
        nameText.text = Player.Instance.getName();
    }

    public void updateAmmo()
    {
        int ammo = Player.Instance.getAmmo();
        ammoText.text = ammo.ToString();
        if (ammo == 0)
        {
            ammoDisplay.SetActive(false);
        }
        else
        {
            ammoDisplay.SetActive(true);
        }
    }

    public void healthUpdate()
    {
        int health = Player.Instance.getHealth();
        float percent = (float)health / maxHealth;
        healthBarSlider.normalizedValue = percent;
    }

    public void updateRoom()
    {
        room.text = Player.Instance.getCurrentRoom() + ": " + Player.Instance.getCurrentRoomEnemies().ToString() + " enemies remain";
        if (Player.Instance.getCurrentRoomEnemies() == 0) Player.Instance.increaseRoomCount();
    }

    public void coinUpdate()
    {
        int coins = (Player.Instance.getCoins() + Player.Instance.getThisCoins());
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
        int steps = Player.Instance.getSteps();
        if (steps <=5)
        {
            stepsText.enabled = false;
            stepsTextBig.enabled = true;
            stepsTextBig.text = "0" + steps.ToString();
        }
        else
        {
            stepsText.enabled = true;
            stepsTextBig.enabled = false;
            stepsText.text = steps.ToString();
        }

    }
}
