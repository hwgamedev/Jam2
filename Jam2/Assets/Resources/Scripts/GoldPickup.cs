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

    void OnCollisionEnter2D(Collider2D col)
    {
        print("Gold collected!");
        if (col.tag == "Player" && !collected)
        {
            collect(value);
        }
    }

    public void collect(int finalValue)
    {
        print("Being collected!");
        if (finalValue == -1)
            finalValue = value;
        collected = true;
        GetComponent<Collected>().playPickup();
        Player.Instance.setCoins(finalValue);
    }
}
