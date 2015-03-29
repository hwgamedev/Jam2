using UnityEngine;
using System.Collections;

public class StepManager : MonoBehaviour
{

	public static StepManager Instance;
	void Awake()
	{
		if (Instance == null) Instance = this;
	}

	// Use this for initialization
	void Start ()
	{
		
	}
	public void newStep(){
		Player.Instance.decrementSteps();
		GridManager.Instance.pathMove ();
	}
}

