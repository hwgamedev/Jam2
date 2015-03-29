using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bomber : EnemyBase
{

	public int stepsToExplode;
	public bool preparingExplosion;
	public List <EnemyBase> dmgEnemies;

	// Use this for initialization
	override public void Start ()
	{
		base.Start();
		stepsToExplode = 2;
		dmg = 100;
		dmgEnemies = new List<EnemyBase>();
	}
	
	// Update is called once per frame
	override public void Update ()
	{
		if(!preparingExplosion)
			base.Update();
		if(dmgEnemies.Count >=1)
			Explode();
	}

	override public void attack(){
		preparingExplosion = true;
	}

	void Explode(){
		foreach(EnemyBase enemy in dmgEnemies)
			enemy.takeDamage(dmg);
		Player.Instance.setHealth(-dmg);
	}

	void OnTriggerStay2D(Collider2D other) {
		if(preparingExplosion){
			if (other.CompareTag("Enemy"))
			{
				EnemyBase tmp = (EnemyBase)other.GetComponent("EnemyBase");
				if(!dmgEnemies.Contains(tmp))
					dmgEnemies.Add(tmp);
			}
		}
			     
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if(preparingExplosion){
			if (other.CompareTag("Enemy"))
			{
				EnemyBase tmp = (EnemyBase)other.GetComponent("EnemyBase");
				if(!dmgEnemies.Contains(tmp))
					dmgEnemies.Add(tmp);
			}
		}
		
	}
}

