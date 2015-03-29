using UnityEngine;
using System.Collections;

public class UseStepIncreaser : MonoBehaviour {
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2)) drinkStepIncreaser();
    }
    public void drinkStepIncreaser()
    {
        Player.Instance.drinkIncreaser();
	}
}
