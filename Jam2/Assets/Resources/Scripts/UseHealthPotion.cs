using UnityEngine;
using System.Collections;

public class UseHealthPotion : MonoBehaviour {
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) drink();
    }
    public void drink()
    {
        Player.Instance.drinkHealthPotion();
    }
}
