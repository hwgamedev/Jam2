using UnityEngine;
using System.Collections;

public class Web : MonoBehaviour
{
	bool collected = false;
	int initialSteps;
	int initialJump;
	int stepLife;
	
	// Use this for initialization
	void Start () {
		initialSteps = Player.Instance.getSteps();
		initialJump = Player.Instance.getJump();
		stepLife = 6;
	}
	
	// Update is called once per frame
	void Update () {
		if (initialJump != Player.Instance.getJump())
			Destroy(gameObject);

		if ((initialSteps - Player.Instance.getSteps()) > stepLife)
			removeWeb();

	}
	
	void OnCollisionEnter2D(Collision2D col)
	{
		//print("Increaser collected!");
		if (col.collider.tag == "Player" && !collected)
		{
			webAction();
		}
	}
	
	public void webAction()
	{
		//print("Being collected!");
		collected = true;
		GetComponent<Collected>().playPickup();
		Player.Instance.decrementSteps();
	}

	public void removeWeb()
	{
		collected = true;
		GetComponent<Collected>().playPickup();
	}
}

