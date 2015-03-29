using UnityEngine;
using System.Collections;

public class UseTeleporter : MonoBehaviour {
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4)) drinkTeleporter();
    }
    public void drinkTeleporter()
    {
        Player.Instance.drinkTeleporter();
    }
}
