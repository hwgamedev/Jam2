using UnityEngine;
using System.Collections;

public class UseHealthPotion : MonoBehaviour {

    public void drink()
    {
        Player.Instance.drinkHealthPotion();
    }
}
