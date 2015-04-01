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
	
	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.collider.tag == "Player" && !collected)
		{
			collect();
		}
	}
	
	public void collect()
	{
		collected = true;
		GetComponent<Collected>().playPickup();
		Player.Instance.pickUpHealthPotion();
	}
}
