using UnityEngine;
using System.Collections;

public class UseStepReducer : MonoBehaviour {
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3)) drinkReducer();
    }
    public void drinkReducer()
    {
        Player.Instance.drinkReducer();
    }
}
