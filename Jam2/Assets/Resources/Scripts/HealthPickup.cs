using UnityEngine;
using System.Collections;

public class HealthPickup : MonoBehaviour
{
	bool collected = false;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnCollisionEnter2D(Collider2D col)
	{
		print("Potion collected!");
		if (col.tag == "Player" && !collected)
		{
			collect();
		}
	}
	
	public void collect()
	{
		print("Being collected!");
		collected = true;
		GetComponent<Collected>().playPickup();
		Player.Instance.pickUpHealthPotion();
	}
}
