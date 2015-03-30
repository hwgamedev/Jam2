using UnityEngine;
using System.Collections;

public class StepManager : MonoBehaviour
{

	public static StepManager Instance;
	void Awake()
	{
		if (Instance == null) Instance = this;
	}

	private ArrayList EnemiesArray = new ArrayList ();

	// Use this for initialization
	void Start ()
	{
		
	}

	public void register(EnemyBase enemy){
		EnemiesArray.Add (enemy);
	}

	public void newStep(){
		Player.Instance.decrementSteps();
		foreach(EnemyBase enemy in EnemiesArray){
            if(enemy.enabled)
			    enemy.incrementSteps();
		}
	}
}

