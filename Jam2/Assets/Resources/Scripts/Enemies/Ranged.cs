using UnityEngine;
using System.Collections;

public class Ranged : EnemyBase {

	//stats
	public int range;
	private SpriteRenderer render;

	override public void Start () {
		base.Start();
        maxHealth = 3;
        health = 3;
		range = 5;
		dmg = dmg/3;
		render = (SpriteRenderer)gameObject.GetComponent("SpriteRenderer");
	}

	override public void Update () {
		if(wait && Time.time - waitInit > 0.5)
			render.material.SetColor("_Color", Color.white);
		float distanceToPlayer = Mathf.Sqrt(Mathf.Pow(transform.position.x - player.transform.position.x, 2) +
		                                    Mathf.Pow(transform.position.y - player.transform.position.y, 2));
		if(/*awake && */!wait && doSteps > 0 && range > (distanceToPlayer))
		{
			shoot();
			doSteps--;
		}else
			base.Update();
	}

	void shoot(){
		render.material.SetColor("_Color", Color.blue);
		base.attack();
	}
}
