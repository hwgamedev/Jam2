using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Shop : MonoBehaviour {
    public Text gold;
    public InputField createChar;
    public GameObject shop;
    public GameObject creator;
    void Start()
    {
        //PlayerPrefs.DeleteAll();  //Uncomment to delete your save for testing
        if (PlayerPrefs.HasKey("playerName"))
        {
            shop.SetActive(true);
            gold.text = PlayerPrefs.GetInt("totalCoins").ToString() + "g";
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
        PlayerPrefs.SetInt("totalCoins", 10000);
        PlayerPrefs.SetInt("healthPotionsStart", 0);
        PlayerPrefs.SetInt("stepIncreasersStart", 0);
        PlayerPrefs.SetInt("stepReducersStart", 0);
        PlayerPrefs.SetInt("teleportersStart", 0);
        PlayerPrefs.Save();
        gold.text = PlayerPrefs.GetInt("totalCoins").ToString() + "g";
        creator.SetActive(false);
        shop.SetActive(true);
    }
    public void buyPermanentHealth()
    {
        if (PlayerPrefs.GetInt("totalCoins") > 3000)
        {
            PlayerPrefs.SetInt("totalCoins", (PlayerPrefs.GetInt("totalCoins") - 3000));
            PlayerPrefs.SetInt("maxHealth", (PlayerPrefs.GetInt("maxHealth") + 1));
            gold.text = PlayerPrefs.GetInt("totalCoins").ToString() + "g";
        }
    }

    public void buyTempHealth()
    {
        if (PlayerPrefs.GetInt("totalCoins") > 500)
        {
            PlayerPrefs.SetInt("totalCoins", (PlayerPrefs.GetInt("totalCoins") - 500));
            PlayerPrefs.SetInt("tempExtraHealth", (PlayerPrefs.GetInt("tempExtraHealth") + 1));
            gold.text = PlayerPrefs.GetInt("totalCoins").ToString() + "g";
        }
    }

    public void startGame()
    {
        PlayerPrefs.Save();
        Application.LoadLevel("UITestScene");
    }
}
