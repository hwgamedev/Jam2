using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Shop : MonoBehaviour {
    public Text gold;
    public InputField createChar;
    public GameObject shop;
    public GameObject creator;

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
        PlayerPrefs.SetInt("maxHealth", 10);
        PlayerPrefs.SetInt("totalCoins", 0);
        PlayerPrefs.SetInt("attackReach", 1);
        PlayerPrefs.SetInt("attackDamage", 1);
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
        //TempValues
        PlayerPrefs.SetInt("tempExtraHealth", 0);
        PlayerPrefs.SetInt("reachTemp", 0);
        PlayerPrefs.SetInt("damageTemp", 0);
        PlayerPrefs.SetInt("healthPotionsStartTemp", 0);
        PlayerPrefs.SetInt("stepIncreasersStartTemp", 0);
        PlayerPrefs.SetInt("stepReducersStartTemp", 0);
        PlayerPrefs.SetInt("teleportersStartTemp", 0);
        tempExtraHealth = PlayerPrefs.GetInt("tempExtraHealth");
        reachTemp = PlayerPrefs.GetInt("reachTemp");
        damageTemp = PlayerPrefs.GetInt("damageTemp");
        healthPotionsTempStart = PlayerPrefs.GetInt("healthPotionsStartTemp");
        stepIncreasersTempStart = PlayerPrefs.GetInt("stepIncreasersStartTemp");
        stepReducersTempStart = PlayerPrefs.GetInt("stepReducersStartTemp");
        teleportersTempStart =  PlayerPrefs.GetInt("teleportersStartTemp");
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
                if (totalCoins > healthCost)
                {
                    totalCoins -= healthCost;
                    updateCoinCount();
                    maxHealth++;
                    PlayerPrefs.SetInt("maxHealth", maxHealth);
                }
                break;
            case 2:
                if (totalCoins > healthCostTemp)
                {
                    totalCoins -= healthCostTemp;
                    updateCoinCount();
                    tempExtraHealth++;
                    PlayerPrefs.SetInt("tempExtraHealth", tempExtraHealth);
                }
                break;
            default:
                break;
        }
        healthCost = 1000 + ((maxHealth - 10) * 1000);
        healthCostTemp = 100 + (tempExtraHealth * 100);
        perm[0].GetComponentInChildren<Text>().text = healthCost.ToString() + "g";
        temp[0].GetComponentInChildren<Text>().text = healthCostTemp.ToString() + "g";
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
                if (totalCoins > damageCost)
                {
                    totalCoins -= damageCost;
                    updateCoinCount();
                    damage++;
                    PlayerPrefs.SetInt("attackDamage", damage);
                }
                break;
            case 2:
                if (totalCoins > damageCostTemp)
                {
                    totalCoins -= damageCostTemp;
                    updateCoinCount();
                    damageTemp++;
                    PlayerPrefs.SetInt("damageTemp", damageTemp);
                }
                break;
            default:
                break;
        }
        damageCost = 1000 + ((damage - 1) * 1000);
        damageCostTemp = 100 + (damageTemp * 100);
        perm[1].GetComponentInChildren<Text>().text = damageCost.ToString() + "g";
        temp[1].GetComponentInChildren<Text>().text = damageCostTemp.ToString() + "g";
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
                if (totalCoins > reachCost)
                {
                    totalCoins -= reachCost;
                    updateCoinCount();
                    reach++;
                    PlayerPrefs.SetInt("attackReach", reach);
                }
                break;
            case 2:
                if (totalCoins > reachCostTemp)
                {
                    totalCoins -= reachCostTemp;
                    updateCoinCount();
                    reachTemp++;
                    PlayerPrefs.SetInt("reachTemp", reachTemp);
                }
                break;
            default:
                break;
        }
        reachCost = 1000 + ((reach - 1) * 1000);
        reachCostTemp = 100 + (reachTemp * 100);
        perm[2].GetComponentInChildren<Text>().text = reachCost.ToString() + "g";
        temp[2].GetComponentInChildren<Text>().text = reachCostTemp.ToString() + "g";
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
                if (totalCoins > healthpotCost)
                {
                    totalCoins -= healthpotCost;
                    updateCoinCount();
                    healthPotionsStart++;
                    PlayerPrefs.SetInt("healthPotionsStart", healthPotionsStart);
                }
                break;
            case 2:
                if (totalCoins > healthpotCostTemp)
                {
                    totalCoins -= healthpotCostTemp;
                    updateCoinCount();
                    healthPotionsTempStart++;
                    PlayerPrefs.SetInt("healthPotionsStartTemp", healthPotionsTempStart);
                }
                break;
            default:
                break;
        }
        healthpotCost = 1000 + ((healthPotionsStart - 1) * 1000);
        healthpotCostTemp = 100 + (healthPotionsTempStart * 100);
        perm[3].GetComponentInChildren<Text>().text = healthpotCost.ToString() + "g";
        temp[3].GetComponentInChildren<Text>().text = healthpotCostTemp.ToString() + "g";
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
                if (totalCoins > incrCost)
                {
                    totalCoins -= incrCost;
                    updateCoinCount();
                    stepIncreasersStart++;
                    PlayerPrefs.SetInt("stepIncreasersStart", stepIncreasersStart);
                }
                break;
            case 2:
                if (totalCoins > incrCostTemp)
                {
                    totalCoins -= incrCostTemp;
                    updateCoinCount();
                    stepIncreasersTempStart++;
                    PlayerPrefs.SetInt("stepIncreasersStartTemp", stepIncreasersTempStart);
                }
                break;
            default:
                break;
        }
        incrCost = 1000 + ((stepIncreasersStart - 1) * 1000);
        incrCostTemp = 100 + (stepIncreasersTempStart * 100);
        perm[4].GetComponentInChildren<Text>().text = incrCost.ToString() + "g";
        temp[4].GetComponentInChildren<Text>().text = incrCostTemp.ToString() + "g";
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
                if (totalCoins > reduceCost)
                {
                    totalCoins -= reduceCost;
                    updateCoinCount();
                    stepReducersStart++;
                    PlayerPrefs.SetInt("stepReducersStart", stepReducersStart);
                }
                break;
            case 2:
                if (totalCoins > reduceCostTemp)
                {
                    totalCoins -= reduceCostTemp;
                    updateCoinCount();
                    stepReducersTempStart++;
                    PlayerPrefs.SetInt("stepReducersStartTemp", stepReducersTempStart);
                }
                break;
            default:
                break;
        }
        reduceCost = 1000 + ((stepReducersStart - 1) * 1000);
        reduceCostTemp = 100 + (stepReducersTempStart * 100);
        perm[5].GetComponentInChildren<Text>().text = reduceCost.ToString() + "g";
        temp[5].GetComponentInChildren<Text>().text = reduceCostTemp.ToString() + "g";
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
                if (totalCoins > teleCost)
                {
                    totalCoins -= teleCost;
                    updateCoinCount();
                    teleportersStart++;
                    PlayerPrefs.SetInt("teleporttersStart", teleportersStart);
                }
                break;
            case 2:
                if (totalCoins > teleCostTemp)
                {
                    totalCoins -= teleCostTemp;
                    updateCoinCount();
                    teleportersTempStart++;
                    PlayerPrefs.SetInt("teleportersStartTemp", teleportersTempStart);
                }
                break;
            default:
                break;
        }
        teleCost = 1000 + ((teleportersStart - 1) * 1000);
        teleCostTemp = 100 + (teleportersTempStart * 100);
        perm[6].GetComponentInChildren<Text>().text = teleCost.ToString() + "g";
        temp[6].GetComponentInChildren<Text>().text = teleCostTemp.ToString() + "g";
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
        Application.LoadLevel("UITestScene");
    }

    public void updateCoinCount()
    {
        PlayerPrefs.SetInt("totalCoins", totalCoins);
        gold.text = "Current Wealth: " + totalCoins.ToString() + "g";
        PlayerPrefs.Save();
    }









}




