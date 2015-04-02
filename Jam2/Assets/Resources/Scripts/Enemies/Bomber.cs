using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bomber : EnemyBase
{

	public int stepsToExplode;
	public bool preparingExplosion;
	public List <EnemyBase> dmgEnemies;
	public bool damagePlayer = false;
	private SpriteRenderer render;
	public bool exploding;

	// Use this for initialization
	override public void Start ()
	{
		base.Start();
		stepsToExplode = 2;
		dmg = dmg;
		dmgEnemies = new List<EnemyBase>();
		preparingExplosion = false;
		exploding = false;
		render = (SpriteRenderer)gameObject.GetComponent("SpriteRenderer");
	}
	
	// Update is called once per frame
	override public void Update ()
	{
		if(!preparingExplosion)
			if(!exploding)
				base.Update();
			else
				//if(!wait)
					Explode ();
		else{
			//if(!wait){
				render.material.SetColor("_Color", Color.yellow);
				exploding = true;
				//startWait ();
				Explode ();
			/*}else{
				checkWait();
				if (Time.time - waitInit > 0.6)
					render.material.SetColor("_Color", Color.red);
				else{
					if (Time.time - waitInit > 0.2)
						render.material.SetColor("_Color", Color.white);
				}
			}*/
		}
	}

	override public void faceMoveDirection()
	{
	}
	override public void attack(){
		//startWait ();
		//render.material.SetColor("_Color", Color.red);
		//preparingExplosion = true;
		Player.Instance.setHealth(-dmg);
		die ();
	}

	void Explode(){
		foreach(EnemyBase enemy in dmgEnemies)
			enemy.takeDamage(dmg/2);
		if(damagePlayer == true)
			Player.Instance.setHealth(-dmg);
		die ();

	}

	void OnTriggerStay2D(Collider2D other) {
		if(exploding){
			if (other.CompareTag("Enemy"))
			{
				EnemyBase tmp = (EnemyBase)other.GetComponent("EnemyBase");
				if(!dmgEnemies.Contains(tmp))
					dmgEnemies.Add(tmp);
			}
			if (other.CompareTag("Player"))
			{
				damagePlayer = true;
			}
		}
			     
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if(exploding){
			if (other.CompareTag("Enemy"))
			{
				EnemyBase tmp = (EnemyBase)other.GetComponent("EnemyBase");
				if(!dmgEnemies.Contains(tmp))
					dmgEnemies.Add(tmp);
			}
			if (other.CompareTag("Player"))
			{
				damagePlayer = true;
			}
		}
		
	}
}

