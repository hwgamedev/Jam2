using UnityEngine;
using System.Collections;

public class Spider : EnemyMoveToPlayer{

	public int abilityStepCD;
	public int lastWebStep;
	public GameObject web;
	// Use this for initialization
	override public void Start () {
		base.Start();
		abilityStepCD = 3;
		lastWebStep = 0;
	}
	
	// Update is called once per frame
	override public void Update () {
		base.Update();
		if(((stepsTaken) % abilityStepCD ==0) && (stepsTaken != lastWebStep))
			doWeb ();
	}

	void doWeb()
	{
		lastWebStep = stepsTaken;
		GameObject newWeb = Instantiate(web);
		newWeb.transform.position = transform.position;

	}
}
