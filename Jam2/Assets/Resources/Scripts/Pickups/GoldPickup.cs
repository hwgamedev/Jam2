using UnityEngine;
using System.Collections;

public class GoldPickup : MonoBehaviour {

    int value = Random.Range(5, 20);
    bool collected = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        print("Gold collected!");
        if (col.collider.tag == "Player" && !collected)
        {
            collected = true;
            collect(value);
        }
    }

    public void collect(int finalValue)
    {
        print("Being collected!");
        if (finalValue == -1)
            finalValue = value;
        GetComponent<Collected>().playPickup();
        Player.Instance.setCoins(finalValue);
    }
}
