using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    public int totalEnemies; //replace with a count of enemies spawned

    private string playerName;
    private int maxHealth;
    private int health;
    private int coins;
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
        initPlayerPref();
        playerName = PlayerPrefs.GetString("playerName");
        maxHealth = PlayerPrefs.GetInt("maxHealth");
        health = maxHealth;
        coins = PlayerPrefs.GetInt("totalCoins");
        healthPotionsStart = PlayerPrefs.GetInt("healthPotionsStart");
        healthPotions = healthPotionsStart;
        stepIncreasersStart = PlayerPrefs.GetInt("stepIncreasersStart");
        stepIncreasers = stepIncreasersStart;
        stepReducersStart = PlayerPrefs.GetInt("stepReducersStart");
        stepReducers = stepReducersStart;
        teleportersStart = PlayerPrefs.GetInt("teleportersStart");
        teleporters = teleportersStart;
        UI.Instance.init();
    }

    void initPlayerPref()
    {
        PlayerPrefs.SetString("playerName", "John");
        PlayerPrefs.SetInt("maxHealth", 10);
        PlayerPrefs.SetInt("totalCoins", 44);
        PlayerPrefs.SetInt("healthPotionsStart", 1);
        PlayerPrefs.SetInt("stepIncreasersStart", 2);
        PlayerPrefs.SetInt("stepReducersStart", 3);
        PlayerPrefs.SetInt("teleportersStart", 4);
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
        PlayerPrefs.SetInt("totalCoins", coins);
        PlayerPrefs.Save();
    }

}