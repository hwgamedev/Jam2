using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    //replace public variables with a player file
    public int maxHealth;
    public int startCoins;
    public int healthPotionsStart;
    public int stepIncreasersStart;
    public int stepReducersStart;
    public int teleportersStart;
    public string playerName;
    public int totalEnemies;

    private int health;
    private int coins;
    private int stepsToTeleport;
    private int enemiesKilled = 0;
    private int healthPotions;
    private int stepIncreasers;
    private int stepReducers;
    private int teleporters;
    private bool buff = false;
    private bool debuff1 = false;
    private bool debuff2 = false;

    public static Player Instance;
    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        //set player name
        //set totalEnemies
        health = maxHealth;
        coins = startCoins;
        healthPotions = healthPotionsStart;
        stepIncreasers = stepIncreasersStart;
        stepReducers = stepReducersStart;
        teleporters = teleportersStart;
        UI.Instance.init();
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

    public bool getBuff()
    {
        return buff;
    }

    public bool getDebuff1()
    {
        return debuff1;
    }

    public bool getDebuff2()
    {
        return debuff2;
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
        healthPotions--;
        setHealth(2);
        UI.Instance.healthPotionUpdate();
    }

    public void drinkIncreaser()
    {
        stepIncreasers--;
        incrementSteps(5);
        UI.Instance.stepIncreaserUpdate();
    }

    public void drinkReducer()
    {
        stepReducers--;
        incrementSteps(-5);
        UI.Instance.stepDecreaserUpdate();
    }

    public void drinkTeleporter()
    {
        //call teleport
        Debug.Log("Teleport");
        teleporters--;
        UI.Instance.teleporterUpdate();
    }

    public void incrementEnemiesKilled()
    {
        enemiesKilled++;
        UI.Instance.enemyKillUpdate();
    }

    public void setBuff(bool _buff)
    {
        buff = _buff;
        UI.Instance.speedBuffUpdate();
    }

    public void setDebuff1(bool _buff)
    {
        debuff1 = _buff;
        UI.Instance.poisonDebuffUpdate();
    }

    public void setDebuff2(bool _buff)
    {
        debuff2 = _buff;
        UI.Instance.slowedDebuffUpdate();
    }

    public void decrementSteps()
    {
        stepsToTeleport --;
        UI.Instance.stepUpdate();

    }

    public void incrementSteps(int i)
    {
        stepsToTeleport += i;
        //if steps >= 0
        //call teleport
        if (stepsToTeleport <= 0) Debug.Log("Teleport");
        UI.Instance.stepUpdate();
    }

}