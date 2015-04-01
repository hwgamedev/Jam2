using UnityEngine;
using System.Collections;

public class Spawner : EnemyBase{

	//spawning
	public int spawnStepCD;
	public int lastSpawnStep;
	public GameObject spawnObject;

	override public void Start () {
		base.Start();
        maxHealth = 4;
        health = 4;
		spawnStepCD = 3;
		lastSpawnStep = 0;
	}
	
	// Update is called once per frame
	override public void Update () {
		base.Update();
		if(((stepsTaken % spawnStepCD) == 0) && (stepsTaken != lastSpawnStep))
			spawn ();
	}

	void spawn()
	{
		lastSpawnStep = stepsTaken;
		GameObject newWeb = Instantiate(spawnObject);
		newWeb.transform.position = transform.position;

	}
}
