using UnityEngine;
using System.Collections;

public class Ranged : EnemyBase {

	//stats
	public int range;

	override public void Start () {
		base.Start();
		range = 5;
	}

	override public void Update () {
		float distanceToPlayer = Mathf.Sqrt(Mathf.Pow(transform.position.x - player.transform.position.x, 2) +
		                                    Mathf.Pow(transform.position.y - player.transform.position.y, 2));
		if(doSteps > 0 && range > (distanceToPlayer))
		{
			shoot();
			doSteps--;
		}else
			base.Update();
	
	}

	void shoot(){
			Debug.Log("Shoot");
	}
}
