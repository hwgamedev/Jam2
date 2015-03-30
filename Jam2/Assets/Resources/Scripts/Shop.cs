using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Shop : MonoBehaviour {
    public Text gold;
    public InputField createChar;
    public GameObject shop;
    public GameObject creator;
    public AudioClip till;
    public AudioClip error;
    private string playerName;
    private int totalCoins;
    private int maxHealth;
    private int maxTempHealth;
    private int totalKills;
    private int totalItems;
    private int totalDeaths;
    private int totalRooms;
    private int totalJumps;
    private int totalCollectedCoins;
    private int healthPotionsStart;
    private int stepIncreasersStart;
    private int stepReducersStart;
    private int teleportersStart;
    private int healthPotionsTempStart;
    private int stepIncreasersTempStart;
    private int stepReducersTempStart;
    private int teleportersTempStart;
    private int damage;
    private int reach;
    private int damageTemp;
    private int reachTemp;
    private int tempExtraHealth;

    public int startHealth;
    public int healthUpgrade;
    public int healthLimit;

    public int startDamage;
    public int damageUpgrade;
    public int damageLimit;

    public int startReach;
    public int reachUpgrade;
    public int reachLimit;

    public int potionCap;
    public int teleporterCap;

    int healthCost;
    int healthCostTemp;
    int damageCost;
    int damageCostTemp;
    int reachCost;
    int reachCostTemp;
    int healthpotCost;
    int healthpotCostTemp;
    int incrCost;
    int incrCostTemp;
    int reduceCost;
    int reduceCostTemp;
    int teleCost;
    int teleCostTemp;

    public Text[] overview = new Text[7];
    public Text[] current = new Text[7];
    public Button[] perm = new Button[7];
    public Button[] temp = new Button[7];

    void Start()
    {
        //PlayerPrefs.DeleteAll();  //Uncomment to delete your save for testing
        if (PlayerPrefs.HasKey("playerName"))
        {
            initStats();
        }
        else
        {
            creator.SetActive(true);
        }
    }

    public void createCharacter()
    {
        PlayerPrefs.SetString("playerName", createChar.text);
        PlayerPrefs.SetInt("maxHealth", startHealth);
        PlayerPrefs.SetInt("totalCoins", 0);
        PlayerPrefs.SetInt("attackReach", startReach);
        PlayerPrefs.SetInt("attackDamage", startDamage);
        PlayerPrefs.SetInt("healthPotionsStart", 0);
        PlayerPrefs.SetInt("stepIncreasersStart", 0);
        PlayerPrefs.SetInt("stepReducersStart", 0);
        PlayerPrefs.SetInt("teleportersStart", 0);
        PlayerPrefs.SetInt("totalKills", 0);
        PlayerPrefs.SetInt("totalCollectedCoins", 0);
        PlayerPrefs.SetInt("totalJumps", 0);
        PlayerPrefs.SetInt("totalDeaths", 0);
        PlayerPrefs.SetInt("totalItems", 0);
        PlayerPrefs.SetInt("totalRoomsCleared", 0);
        PlayerPrefs.Save();
        initStats();
        creator.SetActive(false);
    }

    public void initStats()
    {
        totalCoins = PlayerPrefs.GetInt("totalCoins");

        totalCoins += 100000;
        //TempValues
        PlayerPrefs.SetInt("tempExtraHealth", 0);
        PlayerPrefs.SetInt("reachTemp", 0);
        PlayerPrefs.SetInt("damageTemp", 0);
        PlayerPrefs.SetInt("healthPotionsStartTemp", 0);
        PlayerPrefs.SetInt("stepIncreasersStartTemp", 0);
        PlayerPrefs.SetInt("stepReducersStartTemp", 0);
        PlayerPrefs.SetInt("teleportersStartTemp", 0);
        tempExtraHealth = 0;
        reachTemp = 0;
        damageTemp = 0;
        healthPotionsTempStart = 0;
        stepIncreasersTempStart = 0;
        stepReducersTempStart = 0;
        teleportersTempStart =  0;
        //Overview
        playerName = PlayerPrefs.GetString("playerName");
        overview[0].text = playerName;
        totalKills = PlayerPrefs.GetInt("totalKills");
        overview[1].text = totalKills.ToString();
        totalDeaths = PlayerPrefs.GetInt("totalDeaths");
        overview[2].text = totalDeaths.ToString();
        totalRooms = PlayerPrefs.GetInt("totalRooms");
        overview[3].text = totalRooms.ToString();
        totalCollectedCoins = PlayerPrefs.GetInt("totalCollectedCoins");
        overview[4].text = totalCollectedCoins.ToString();
        totalItems = PlayerPrefs.GetInt("totalItems");
        overview[5].text = totalItems.ToString();
        totalJumps = PlayerPrefs.GetInt("totalJumps");
        overview[6].text = totalJumps.ToString();
        //Current Stats
        maxHealth = PlayerPrefs.GetInt("maxHealth");
        current[0].text = maxHealth.ToString();
        damage = PlayerPrefs.GetInt("attackDamage");
        current[1].text = damage.ToString();
        reach = PlayerPrefs.GetInt("attackReach");
        current[2].text = reach.ToString();
        healthPotionsStart = PlayerPrefs.GetInt("healthPotionsStart");
        current[3].text = healthPotionsStart.ToString();
        stepIncreasersStart = PlayerPrefs.GetInt("stepIncreasersStart");
        current[4].text = stepIncreasersStart.ToString();
        stepReducersStart = PlayerPrefs.GetInt("stepReducersStart");
        current[5].text = stepReducersStart.ToString();
        teleportersStart = PlayerPrefs.GetInt("teleportersStart");
        current[6].text = teleportersStart.ToString();
        //Perm Buttons
        updateHealth(0);
        updateDamage(0);
        updateReach(0);
        updateHealthPotions(0);
        updateIncreasers(0);
        updateReducers(0);
        updateTeleporters(0);
        updateCoinCount();
    }

    public void updateHealth(int x)
    {
        switch (x)
        {
            case 1:
                if (totalCoins >= healthCost)
                {
                    totalCoins -= healthCost;
                    updateCoinCount();
                    maxHealth += healthUpgrade;
                    PlayerPrefs.SetInt("maxHealth", maxHealth);
                }
                else
                {
                    AudioSource.PlayClipAtPoint(error, transform.position);
                }
                break;
            case 2:
                if (totalCoins >= healthCostTemp)
                {
                    totalCoins -= healthCostTemp;
                    updateCoinCount();
                    tempExtraHealth += healthUpgrade;
                    PlayerPrefs.SetInt("tempExtraHealth", tempExtraHealth);
                }
                else
                {
                    AudioSource.PlayClipAtPoint(error, transform.position);
                }
                break;
            default:
                break;
        }
        if (tempExtraHealth + maxHealth >= healthLimit)
        {
            perm[0].GetComponentInChildren<Text>().text = "FULL";
            perm[0].interactable = false;
            temp[0].GetComponentInChildren<Text>().text = "FULL";
            temp[0].interactable = false;
        }
        else
        {


            healthCost = (maxHealth + tempExtraHealth) * 100;
            healthCostTemp = (maxHealth + tempExtraHealth) * 30;
            perm[0].GetComponentInChildren<Text>().text = healthCost.ToString() + "g";
            temp[0].GetComponentInChildren<Text>().text = healthCostTemp.ToString() + "g";
        }
        if (tempExtraHealth > 0)
        {
            current[0].text = maxHealth.ToString() + " (+" + tempExtraHealth.ToString() + ")";
        }
        else
        {
            current[0].text = maxHealth.ToString();
        }
    }

    public void updateDamage(int x)
    {
        switch (x)
        {
            case 1:
                if (totalCoins >= damageCost)
                {
                    totalCoins -= damageCost;
                    updateCoinCount();
                    damage++;
                    PlayerPrefs.SetInt("attackDamage", damage);
                }
                else
                {
                    AudioSource.PlayClipAtPoint(error, transform.position);
                }
                break;
            case 2:
                if (totalCoins >= damageCostTemp)
                {
                    totalCoins -= damageCostTemp;
                    updateCoinCount();
                    damageTemp++;
                    PlayerPrefs.SetInt("damageTemp", damageTemp);
                }
                else
                {
                    AudioSource.PlayClipAtPoint(error, transform.position);
                }
                break;
            default:
                break;
        }
        if (damageTemp + damage >= damageLimit)
        {
            perm[1].GetComponentInChildren<Text>().text = "FULL";
            perm[1].interactable = false;
            temp[1].GetComponentInChildren<Text>().text = "FULL";
            temp[1].interactable = false;
        }
        else
        {
            damageCost = 2500 * (damage + damageTemp);
            damageCostTemp = 750 * (damage + damageTemp);
            perm[1].GetComponentInChildren<Text>().text = damageCost.ToString() + "g";
            temp[1].GetComponentInChildren<Text>().text = damageCostTemp.ToString() + "g";
        }
        if (damageTemp > 0)
        {
            current[1].text = damage.ToString() + " (+" + damageTemp.ToString() + ")";
        }
        else
        {
            current[1].text = damage.ToString();
        }
    }

    public void updateReach(int x)
    {
        switch (x)
        {
            case 1:
                if (totalCoins >= reachCost)
                {
                    totalCoins -= reachCost;
                    updateCoinCount();
                    reach++;
                    PlayerPrefs.SetInt("attackReach", reach);
                }
                else
                {
                    AudioSource.PlayClipAtPoint(error, transform.position);
                }
                break;
            case 2:
                if (totalCoins >= reachCostTemp)
                {
                    totalCoins -= reachCostTemp;
                    updateCoinCount();
                    reachTemp++;
                    PlayerPrefs.SetInt("reachTemp", reachTemp);
                }
                else
                {
                    AudioSource.PlayClipAtPoint(error, transform.position);
                }
                break;
            default:
                break;
        }
        if (reachTemp + reach >= reachLimit)
        {
            perm[2].GetComponentInChildren<Text>().text = "FULL";
            perm[2].interactable = false;
            temp[2].GetComponentInChildren<Text>().text = "FULL";
            temp[2].interactable = false;
        }
        else
        {
            reachCost = 2500 * (reach + reachTemp);
            reachCostTemp = 750 * (reach + reachTemp);
            perm[2].GetComponentInChildren<Text>().text = reachCost.ToString() + "g";
            temp[2].GetComponentInChildren<Text>().text = reachCostTemp.ToString() + "g";
        }
        if (reachTemp > 0)
        {
            current[2].text = reach.ToString() + " (+" + reachTemp.ToString() + ")";
        }
        else
        {
            current[2].text = reach.ToString();
        }
    }

    public void updateHealthPotions(int x)
    {
        switch (x)
        {
            case 1:
                if (totalCoins >= healthpotCost)
                {
                    totalCoins -= healthpotCost;
                    updateCoinCount();
                    healthPotionsStart++;
                    PlayerPrefs.SetInt("healthPotionsStart", healthPotionsStart);
                    AudioSource.PlayClipAtPoint(till, transform.position);
                }
                else
                {
                    AudioSource.PlayClipAtPoint(error, transform.position);
                }
                break;
            case 2:
                if (totalCoins >= healthpotCostTemp)
                {
                    totalCoins -= healthpotCostTemp;
                    updateCoinCount();
                    healthPotionsTempStart++;
                    PlayerPrefs.SetInt("healthPotionsStartTemp", healthPotionsTempStart);
                    AudioSource.PlayClipAtPoint(till, transform.position);
                }
                else
                {
                    AudioSource.PlayClipAtPoint(error, transform.position);
                }
                break;
            default:
                break;
        }
        if (healthPotionsStart >= potionCap)
        {
            perm[3].GetComponentInChildren<Text>().text = "FULL";
            perm[3].interactable = false;

        }
        if (healthPotionsTempStart + healthPotionsStart >= potionCap + 1)
        {
            temp[3].GetComponentInChildren<Text>().text = "FULL";
            temp[3].interactable = false;
            perm[3].GetComponentInChildren<Text>().text = "FULL";
            perm[3].interactable = false;
        }
        else if (healthPotionsStart != potionCap)
        {
            healthpotCost = 2000 + (healthPotionsStart + healthPotionsTempStart) * 1000;
            healthpotCostTemp = 500 + (healthPotionsTempStart + healthPotionsStart) * 250;
            perm[3].GetComponentInChildren<Text>().text = healthpotCost.ToString() + "g";
            temp[3].GetComponentInChildren<Text>().text = healthpotCostTemp.ToString() + "g";
        }
        if (healthPotionsTempStart > 0)
        {
            current[3].text = healthPotionsStart.ToString() + " (+" + healthPotionsTempStart.ToString() + ")";
        }
        else
        {
            current[3].text = healthPotionsStart.ToString();
        }
    }

    public void updateIncreasers(int x)
    {
        switch (x)
        {
            case 1:
                if (totalCoins >= incrCost)
                {
                    totalCoins -= incrCost;
                    updateCoinCount();
                    stepIncreasersStart++;
                    PlayerPrefs.SetInt("stepIncreasersStart", stepIncreasersStart);
                    AudioSource.PlayClipAtPoint(till, transform.position);
                }
                else
                {
                    AudioSource.PlayClipAtPoint(error, transform.position);
                }
                break;
            case 2:
                if (totalCoins >= incrCostTemp)
                {
                    totalCoins -= incrCostTemp;
                    updateCoinCount();
                    stepIncreasersTempStart++;
                    PlayerPrefs.SetInt("stepIncreasersStartTemp", stepIncreasersTempStart);
                    AudioSource.PlayClipAtPoint(till, transform.position);
                }
                else
                {
                    AudioSource.PlayClipAtPoint(error, transform.position);
                }
                break;
            default:
                break;
        }
        if (stepIncreasersStart >= potionCap)
        {
            perm[4].GetComponentInChildren<Text>().text = "FULL";
            perm[4].interactable = false;

        }
        if (stepIncreasersTempStart + stepIncreasersStart >= potionCap + 1)
        {
            temp[4].GetComponentInChildren<Text>().text = "FULL";
            temp[4].interactable = false;
            perm[4].GetComponentInChildren<Text>().text = "FULL";
            perm[4].interactable = false;
        }
        else if (stepIncreasersStart != potionCap)
        {
            incrCost = 2000 + (stepIncreasersStart + stepIncreasersTempStart) * 1000;
            incrCostTemp = 500 + (stepIncreasersTempStart + stepIncreasersStart) * 250;
            perm[4].GetComponentInChildren<Text>().text = incrCost.ToString() + "g";
            temp[4].GetComponentInChildren<Text>().text = incrCostTemp.ToString() + "g";
        }
        if (stepIncreasersTempStart > 0)
        {
            current[4].text = stepIncreasersStart.ToString() + " (+" + stepIncreasersTempStart.ToString() + ")";
        }
        else
        {
            current[4].text = stepIncreasersStart.ToString();
        }
    }

    public void updateReducers(int x)
    {
        switch (x)
        {
            case 1:
                if (totalCoins >= reduceCost)
                {
                    totalCoins -= reduceCost;
                    updateCoinCount();
                    stepReducersStart++;
                    PlayerPrefs.SetInt("stepReducersStart", stepReducersStart);
                    AudioSource.PlayClipAtPoint(till, transform.position);
                }
                else
                {
                    AudioSource.PlayClipAtPoint(error, transform.position);
                }
                break;
            case 2:
                if (totalCoins >= reduceCostTemp)
                {
                    totalCoins -= reduceCostTemp;
                    updateCoinCount();
                    stepReducersTempStart++;
                    PlayerPrefs.SetInt("stepReducersStartTemp", stepReducersTempStart);
                    AudioSource.PlayClipAtPoint(till, transform.position);
                }
                else
                {
                    AudioSource.PlayClipAtPoint(error, transform.position);
                }
                break;
            default:
                break;
        }
        if (stepReducersStart >= potionCap)
        {
            perm[5].GetComponentInChildren<Text>().text = "FULL";
            perm[5].interactable = false;
        }
        if (stepReducersTempStart + stepReducersStart >= potionCap + 1)
        {
            temp[5].GetComponentInChildren<Text>().text = "FULL";
            temp[5].interactable = false;
            perm[5].GetComponentInChildren<Text>().text = "FULL";
            perm[5].interactable = false;
        }
        else if (stepReducersStart != potionCap)
        {
            reduceCost = 2000 + (stepReducersStart + stepReducersTempStart) * 1000;
            reduceCostTemp = 500 + (stepReducersTempStart + stepReducersStart) * 250;
            perm[5].GetComponentInChildren<Text>().text = reduceCost.ToString() + "g";
            temp[5].GetComponentInChildren<Text>().text = reduceCostTemp.ToString() + "g";
        }
        if (stepReducersTempStart > 0)
        {
            current[5].text = stepReducersStart.ToString() + " (+" + stepReducersTempStart.ToString() + ")";
        }
        else
        {
            current[5].text = stepReducersStart.ToString();
        }
    }

    public void updateTeleporters(int x)
    {
        switch (x)
        {
            case 1:
                if (totalCoins >= teleCost)
                {
                    totalCoins -= teleCost;
                    updateCoinCount();
                    teleportersStart++;
                    PlayerPrefs.SetInt("teleportersStart", teleportersStart);
                    AudioSource.PlayClipAtPoint(till, transform.position);
                }
                else
                {
                    AudioSource.PlayClipAtPoint(error, transform.position);
                }
                break;
            case 2:
                if (totalCoins >= teleCostTemp)
                {
                    totalCoins -= teleCostTemp;
                    updateCoinCount();
                    teleportersTempStart++;
                    PlayerPrefs.SetInt("teleportersStartTemp", teleportersTempStart);
                    AudioSource.PlayClipAtPoint(till, transform.position);
                }
                else
                {
                    AudioSource.PlayClipAtPoint(error, transform.position);
                }
                break;
            default:
                break;
        }
        if (teleportersStart >= teleporterCap)
        {
            perm[6].GetComponentInChildren<Text>().text = "FULL";
            perm[6].interactable = false;
        }
        if (teleportersTempStart + teleportersStart >= teleporterCap + 1)
        {
            temp[6].GetComponentInChildren<Text>().text = "FULL";
            temp[6].interactable = false;
            perm[6].GetComponentInChildren<Text>().text = "FULL";
            perm[6].interactable = false;
        }
        else if (teleportersStart != potionCap)
        {
            teleCost = 5000 + (teleportersStart + teleportersTempStart) * 5000;
            teleCostTemp = 2500 + (teleportersTempStart + teleportersStart) * 1250;
            perm[6].GetComponentInChildren<Text>().text = teleCost.ToString() + "g";
            temp[6].GetComponentInChildren<Text>().text = teleCostTemp.ToString() + "g";
        }
        if (teleportersTempStart > 0)
        {
            current[6].text = teleportersStart.ToString() + " (+" + teleportersTempStart.ToString() + ")";
        }
        else
        {
            current[6].text = teleportersStart.ToString();
        }
    }

    public void startGame()
    {
        PlayerPrefs.Save();
        Application.LoadLevel("UIOnRoom");
    }

    public void updateCoinCount()
    {
        PlayerPrefs.SetInt("totalCoins", totalCoins);
        gold.text = "Current Wealth: " + totalCoins.ToString() + "g";
        PlayerPrefs.Save();
    }









}




