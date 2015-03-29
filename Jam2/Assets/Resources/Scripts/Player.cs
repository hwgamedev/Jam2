﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    public int totalEnemies; //replace with a count of enemies spawned

    private string playerName;
    private int maxHealth;
    private int health;
    private int coins;
    private int damage;
    private int reach;
    private int stepsToTeleport;
    private int enemiesKilled = 0;
    private int healthPotions;
    private int stepIncreasers;
    private int stepReducers;
    private int teleporters;
    private int healthPotionsStart;
    private int stepIncreasersStart;
    private int stepReducersStart;
    private int teleportersStart;

    public static Player Instance;
    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        playerName = PlayerPrefs.GetString("playerName");
        maxHealth = PlayerPrefs.GetInt("maxHealth") + PlayerPrefs.GetInt("tempExtraHealth");
        PlayerPrefs.SetInt("tempExtraHealth", 0);
        health = maxHealth;
        coins = PlayerPrefs.GetInt("totalCoins");
        healthPotionsStart = PlayerPrefs.GetInt("healthPotionsStart") + PlayerPrefs.GetInt("healthPotionsStartTemp");
        PlayerPrefs.SetInt("healthPotionsStartTemp", 0);
        healthPotions = healthPotionsStart;
        stepIncreasersStart = PlayerPrefs.GetInt("stepIncreasersStart") + PlayerPrefs.GetInt("stepIncreasersStartTemp");
        PlayerPrefs.SetInt("stepIncreasersStart", 0);
        stepIncreasers = stepIncreasersStart;
        stepReducersStart = PlayerPrefs.GetInt("stepReducersStart") + PlayerPrefs.GetInt("stepReducersStartTemp");
        PlayerPrefs.SetInt("stepReducersStart", 0);
        stepReducers = stepReducersStart;
        teleportersStart = PlayerPrefs.GetInt("teleportersStart") + PlayerPrefs.GetInt("teleportersStartTemp");
        PlayerPrefs.SetInt("teleportersStart", 0);
        teleporters = teleportersStart;
        reach = PlayerPrefs.GetInt("attackReach") + PlayerPrefs.GetInt("reachTemp");
        PlayerPrefs.SetInt("reachTemp", 0);
        damage = PlayerPrefs.GetInt("attackDamage") + PlayerPrefs.GetInt("damageTemp");
        PlayerPrefs.SetInt("damageTemp", 0);
        UI.Instance.init();
    }

    public int getReach()
    {
        return reach;
    }

    public int getDamage()
    {
        return damage;
    }

    public void increaseMaxHealthPerma()
    {
        PlayerPrefs.SetInt("maxHealth", maxHealth++);
    }

    public void increaseMaxIncreasers()
    {
        PlayerPrefs.SetInt("stepIncreasersStart", stepIncreasersStart++);
    }

    public void increaseMaxReducers()
    {
        PlayerPrefs.SetInt("stepReducersStart", stepReducersStart++);
    }

    public void increaseMaxHealthPotions()
    {
        PlayerPrefs.SetInt("healthPotionsStart", healthPotionsStart++);
    }

    public void increaseMaxTeleporters()
    {
        PlayerPrefs.SetInt("teleportersStart", teleportersStart++);
    }

    public string getName()
    {
        return playerName;
    }

    public int getCoins()
    {
        return coins;
    }

    public int getHealth()
    {
        return health;
    }

    public float getEnemyPercentKilled()
    {
        return ((float)enemiesKilled / totalEnemies) * 100F;
    }

    public int getHealthPotions()
    {
        return healthPotions;
    }

    public int getStepIncreasers()
    {
        return stepIncreasers;
    }

    public int getStepDecreasers()
    {
        return stepReducers;
    }

    public int getTeleporters()
    {
        return teleporters;
    }

    public int getSteps()
    {
        return stepsToTeleport;
    }

    public void setName(string _name)
    {
        playerName = _name;
        UI.Instance.nameUpdate();
    }

    public void setCoins(int _coins)
    {
        coins = _coins;
        UI.Instance.coinUpdate();
    }

    public void setHealth (int _health) 
    {
        health += _health;
        if (health > maxHealth) health = maxHealth;
        else if (health <= 0) {death();}
        UI.Instance.healthUpdate();
    }

    public void setMaxHealth(int _maxHealth)
    {
        maxHealth = _maxHealth;
    }

    public void setTotalEnemies(int _totalEnemies)
    {
        totalEnemies = _totalEnemies;
    }

    public void setSteps(int _stepsToTeleport)
    {
        stepsToTeleport = _stepsToTeleport;
        UI.Instance.stepUpdate();
    }

    public void pickUpHealthPotion()
    {
        healthPotions++;
        UI.Instance.healthPotionUpdate();
    }

    public void pickUpIncreaser()
    {
        stepIncreasers++;
        UI.Instance.stepIncreaserUpdate();
    }

    public void pickUpReducer()
    {
        stepReducers++;
        UI.Instance.stepDecreaserUpdate();
    }

    public void pickUpTeleporter()
    {
        teleporters++;
        UI.Instance.teleporterUpdate();
    }

    public void drinkHealthPotion()
    {   
        if(healthPotions > 0)
        healthPotions--;
        setHealth(2);
        UI.Instance.healthPotionUpdate();
    }

    public void drinkIncreaser()
    {
        if (stepIncreasers > 0)
        stepIncreasers--;
        incrementSteps(5);
        UI.Instance.stepIncreaserUpdate();
    }

    public void drinkReducer()
    {
        if (stepReducers > 0)
        stepReducers--;
        incrementSteps(-5);
        UI.Instance.stepDecreaserUpdate();
    }

    public void drinkTeleporter()
    {
        if (teleporters > 0)
        //call teleport
        teleporters--;
        UI.Instance.teleporterUpdate();
    }

    public void incrementEnemiesKilled()
    {
        enemiesKilled++;
        UI.Instance.enemyKillUpdate();
    }

    public void decrementSteps()
    {
        stepsToTeleport --;
        //if steps >= 0
        //call teleport
        if (stepsToTeleport <= 0) stepsToTeleport = 0;
        UI.Instance.stepUpdate();

    }

    public void incrementSteps(int i)
    {
        stepsToTeleport += i;
        //if steps >= 0
        //call teleport
        if (stepsToTeleport <= 0) stepsToTeleport = 0;
        UI.Instance.stepUpdate();
    }

    public void death()
    {
        //Set kills, rooms cleared, coins, jumps, items and player's death to the apropriate keys in PlayerPrefs
        //PlayerPrefs.SetInt("totalKills", (PlayerPrefs.GetInt("totalKills") + kills);
        //PlayerPrefs.SetInt("totalJumps", (PlayerPrefs.SetInt("totaljumps") + jumps);
        //PlayerPrefs.SetInt("totalDeaths", (PlayerPrefs.SetInt("totalDeaths") + 1);
        //PlayerPrefs.SetInt("totalItems", (PlayerPrefs.SetInt("totalItems") + items);
        //PlayerPrefs.SetInt("totalRoomsCleared", (PlayerPrefs.SetInt("totalRoomsCleared") + rooms);
        //PlayerPrefs.setInt("totalCollectedCoins", (PlayerPrefs.GetInt("totalCollecedCoins") + coins);
        PlayerPrefs.SetInt("thisCoins", coins);
        PlayerPrefs.SetInt("totalCoins", (PlayerPrefs.GetInt("totalCoins") + coins));
        PlayerPrefs.Save();
        Application.LoadLevel("GameOver");
    }

}