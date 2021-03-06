﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
    public int totalEnemies; //replace with a count of enemies spawned

    private string playerName;
    private int maxHealth;
    private int health;
    private int thisCoins;
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
    private int jump;
    private int rooms;
    private int items;
    private string currentRoom;
    private int currentRoomEnemies;
    private int ammo;

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
        ammo = 5;
        coins = PlayerPrefs.GetInt("totalCoins");
        healthPotionsStart = PlayerPrefs.GetInt("healthPotionsStart") + PlayerPrefs.GetInt("healthPotionsStartTemp");
        PlayerPrefs.SetInt("healthPotionsStartTemp", 0);
        healthPotions = healthPotionsStart;
        stepIncreasersStart = PlayerPrefs.GetInt("stepIncreasersStart") + PlayerPrefs.GetInt("stepIncreasersStartTemp");
        PlayerPrefs.SetInt("stepIncreasersStartTemp", 0);
        stepIncreasers = stepIncreasersStart;
        stepReducersStart = PlayerPrefs.GetInt("stepReducersStart") + PlayerPrefs.GetInt("stepReducersStartTemp");
        PlayerPrefs.SetInt("stepReducersStartTemp", 0);
        stepReducers = stepReducersStart;
        teleportersStart = PlayerPrefs.GetInt("teleportersStart") + PlayerPrefs.GetInt("teleportersStartTemp");
        PlayerPrefs.SetInt("teleportersStartTemp", 0);
        teleporters = teleportersStart;
        reach = PlayerPrefs.GetInt("attackReach") + PlayerPrefs.GetInt("reachTemp");
        PlayerPrefs.SetInt("reachTemp", 0);
        damage = PlayerPrefs.GetInt("attackDamage") + PlayerPrefs.GetInt("damageTemp");
        PlayerPrefs.SetInt("damageTemp", 0);
        UI.Instance.init();
    }


    public int getAmmo()
    {
        return ammo;
    }

    public void pickUpAmmo()
    {
        ammo++;
        UI.Instance.updateAmmo();
    }

    public void throwAmmo()
    {
        ammo--;
        UI.Instance.updateAmmo();
    }

    public string getCurrentRoom()
    {
        return currentRoom;
    }

    public int getCurrentRoomEnemies()
    {
        return currentRoomEnemies;
    }

    public void setCurrentRoom(string room)
    {
        currentRoom = room;
        UI.Instance.updateRoom();
    }

    public void setRoomEnemies(int enemies)
    {
        currentRoomEnemies = enemies;
        UI.Instance.updateRoom();
    }

    public void killRoomEnemy()
    {
        currentRoomEnemies--;
        UI.Instance.updateRoom();
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

    public int getThisCoins()
    {
        return thisCoins;
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
        thisCoins += _coins;
        UI.Instance.coinUpdate();
    }
    IEnumerator flash(Renderer Char)
    {
        yield return new WaitForSeconds(0.15F);
        Char.material.color = Color.white;
    }

    public void setHealth(int _health) 
    {
        Renderer Char = GameObject.FindGameObjectWithTag("Player").GetComponent<Renderer>();
        health += _health;
        if (_health < 0)
        {
            Char.material.color = Color.red;
            StartCoroutine(flash(Char));
        }
        else
        {
            Char.material.color = Color.green;
            StartCoroutine(flash(Char));
        }
        if (health > maxHealth) health = maxHealth;
        else if (health <= 0) {death();}
        UI.Instance.healthUpdate();
    }

    public void increaseRoomCount()
    {
        rooms++;
    }

    public void setMaxHealth(int _maxHealth)
    {
        maxHealth = _maxHealth;
    }

    public void setTotalEnemies(int _totalEnemies)
    {
        totalEnemies = _totalEnemies;
    }

    public void pickUpHealthPotion()
    {
        healthPotions++;
        if (healthPotions > 10)
        {
            healthPotions = 10;
        }
        UI.Instance.healthPotionUpdate();
        items++;
    }

    public void pickUpIncreaser()
    {
        stepIncreasers++;
        if(stepIncreasers > 10)
        {
            stepIncreasers = 10;
        }
        UI.Instance.stepIncreaserUpdate();
        items++;
    }

    public void pickUpReducer()
    {
        stepReducers++;
        if (stepReducers > 10)
        {
            stepReducers = 10;
        }
        UI.Instance.stepDecreaserUpdate();
        items++;
    }

    public void pickUpTeleporter()
    {
        teleporters++;
        if (teleporters > 10)
        {
            teleporters = 10;
        }
        UI.Instance.teleporterUpdate();
        items++;
    }

    public void drinkHealthPotion()
    {
        if (healthPotions > 0)
        {
            healthPotions--;
            setHealth((maxHealth/5));
            UI.Instance.healthPotionUpdate();
        }
    }

    public void drinkIncreaser()
    {
        if (stepIncreasers > 0)
        {
            stepIncreasers--;
            incrementSteps(5);
            UI.Instance.stepIncreaserUpdate();
        }
    }

    public void drinkReducer()
    {
        if (stepReducers > 0)
        {
            stepReducers--;
            incrementSteps(-5);
            UI.Instance.stepDecreaserUpdate();
        }
    }

    public void drinkTeleporter()
    {
        if (teleporters > 0)
        {
            setSteps(-1);
            teleporters--;
            UI.Instance.teleporterUpdate();
        }
    }

    public void setSteps(int _stepsToTeleport)
    {
        stepsToTeleport = _stepsToTeleport;
        UI.Instance.stepUpdate();
        if (stepsToTeleport < 0)
        {
            teleport();
        }
    }

    public void incrementEnemiesKilled()
    {
        enemiesKilled++;
        if (enemiesKilled == totalEnemies)
        {
            win();
        }
        UI.Instance.enemyKillUpdate();
    }

    public void decrementSteps()
    {
        stepsToTeleport--;
        UI.Instance.stepUpdate();
		if (stepsToTeleport < 0) {
            setSteps(-1);
		}
    }

    public void teleport()
    {
        RoomGenerator r = FindObjectOfType<RoomGenerator>();
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        GameObject c = GameObject.FindGameObjectWithTag("MainCamera");
        p.GetComponent<characterControls>().teleport();
        RoomData rd = r.getNextRoom().GetComponent<RoomData>();
        rd.spawnPlayer(p,c);
        this.incrementSteps(rd.getRoomSize());
		incrementJump();
    }

    public void incrementSteps(int i)
    {
        stepsToTeleport += i;
        UI.Instance.stepUpdate();
        if (stepsToTeleport < 0)
        {
            setSteps(-1);
        }
    }

    public void incrementJump()
    {
        jump++;
    }

	public int getJump()
	{
		return jump;
	}

    public void incrementTotalEnemies()
    {
        totalEnemies++;
    }

    public void win()
    {
        //Set kills, rooms cleared, coins, jumps, items and player's death to the apropriate keys in PlayerPrefs
        PlayerPrefs.SetInt("totalKills", (PlayerPrefs.GetInt("totalKills") + enemiesKilled));
        PlayerPrefs.SetInt("enemyKills", enemiesKilled);
        PlayerPrefs.SetInt("totalJumps", (PlayerPrefs.GetInt("totaljumps") + jump));
        PlayerPrefs.SetInt("totalDeaths", (PlayerPrefs.GetInt("totalDeaths") + 1));
        //PlayerPrefs.SetInt("totalItems", (PlayerPrefs.GetInt("totalItems") + items);
        //PlayerPrefs.SetInt("totalRooms", (PlayerPrefs.GetInt("totalRooms") + rooms);
        PlayerPrefs.SetInt("totalCollectedCoins", (PlayerPrefs.GetInt("totalCollectedCoins") + thisCoins));
        PlayerPrefs.SetInt("thisCoins", thisCoins);
        PlayerPrefs.SetInt("totalCoins", coins + thisCoins);
        PlayerPrefs.Save();
        Application.LoadLevel("win");
    }
    public void death()
    {
        //Set kills, rooms cleared, coins, jumps, items and player's death to the apropriate keys in PlayerPrefs
        PlayerPrefs.SetInt("totalKills", (PlayerPrefs.GetInt("totalKills") + enemiesKilled));
        PlayerPrefs.SetInt("enemyKills", enemiesKilled);
        PlayerPrefs.SetInt("totalJumps", (PlayerPrefs.GetInt("totaljumps") + jump));
        PlayerPrefs.SetInt("totalDeaths", (PlayerPrefs.GetInt("totalDeaths") + 1));
        PlayerPrefs.SetInt("totalItems", (PlayerPrefs.GetInt("totalItems") + items));
        PlayerPrefs.SetInt("totalRooms", (PlayerPrefs.GetInt("totalRooms") + rooms));
        PlayerPrefs.SetInt("totalCollectedCoins", (PlayerPrefs.GetInt("totalCollectedCoins") + thisCoins));
        PlayerPrefs.SetInt("thisCoins", thisCoins);
        PlayerPrefs.SetInt("totalCoins", coins + thisCoins);
        PlayerPrefs.Save();
        Application.LoadLevel("GameOver");
    }

}