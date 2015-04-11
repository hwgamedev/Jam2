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

	//sprites for potions
	public Sprite[] HP;
	public Sprite[] Increase;
	public Sprite[] Decrease;

    void Awake()
    {
        if (Instance == null) Instance = this;
        if(PlayerPrefs.GetInt("newPlayer") == 1)
        {
            Time.timeScale = 0;
            help.SetActive(true);
            help2.SetActive(false);
            PlayerPrefs.SetInt("newPlayer", 0);
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
        if (item == 0) {
			healthPotion.interactable = false;
			healthPotion.gameObject.GetComponent<Image>().sprite = HP[HP.Length-1];
		}else {
			healthPotion.interactable = true;
			if(item < 2)
				healthPotion.gameObject.GetComponent<Image>().sprite = HP[HP.Length-1];
			if(item < 4 && item > 1)
				healthPotion.gameObject.GetComponent<Image>().sprite = HP[HP.Length-2];
			if(item < 7 && item > 3)
				healthPotion.gameObject.GetComponent<Image>().sprite = HP[HP.Length-3];
			if(item > 7)
				healthPotion.gameObject.GetComponent<Image>().sprite = HP[HP.Length-4];
		}
    }

    public void stepIncreaserUpdate()
    {
        int item = Player.Instance.getStepIncreasers();
        increaserQuantity.text = item.ToString();
		if (item == 0) {
			increaser.interactable = false;
			increaser.gameObject.GetComponent<Image>().sprite = Increase[Increase.Length-1];
		}else {
			increaser.interactable = true;
			if(item < 2)
				increaser.gameObject.GetComponent<Image>().sprite = Increase[Increase.Length-1];
			if(item < 4 && item > 1)
				increaser.gameObject.GetComponent<Image>().sprite = Increase[Increase.Length-2];
			if(item < 7 && item > 3)
				increaser.gameObject.GetComponent<Image>().sprite = Increase[Increase.Length-3];
			if(item > 7)
				increaser.gameObject.GetComponent<Image>().sprite = Increase[Increase.Length-4];
		}

    }

    public void stepDecreaserUpdate()
    {
        int item = Player.Instance.getStepDecreasers();
        decreaserQuantity.text = item.ToString();
        if (item == 0) {
			decreaser.interactable = false;
			decreaser.gameObject.GetComponent<Image>().sprite = Decrease[Decrease.Length-1];
		}else {
			decreaser.interactable = true;
			if(item < 2)
				decreaser.gameObject.GetComponent<Image>().sprite = Decrease[Decrease.Length-1];
			if(item < 4 && item > 1)
				decreaser.gameObject.GetComponent<Image>().sprite = Decrease[Decrease.Length-2];
			if(item < 7 && item > 3)
				decreaser.gameObject.GetComponent<Image>().sprite = Decrease[Decrease.Length-3];
			if(item > 7)
				decreaser.gameObject.GetComponent<Image>().sprite = Decrease[Decrease.Length-4];
		}
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
