using UnityEngine;
using System.Collections;

public class Ranged : EnemyBase {

	public int range;
	// Use this for initialization
	override public void Start () {
		base.Start();
		range = 5;
	}
	
	// Update is called once per frame
	override public void Update () {
		float distanceToPlayer = Mathf.Sqrt(Mathf.Pow(transform.position.x - player.transform.position.x, 2) +
		                                    Mathf.Pow(transform.position.y - player.transform.position.y, 2));
		if(doSteps > 0 && range > (distanceToPlayer))
		{
			Shoot();
			doSteps--;
		}else
			base.Update();
	
	}

	void Shoot(){
			Debug.Log("Shoot");
	}
}
