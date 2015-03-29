using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EnemyBase : MonoBehaviour {

	public GameObject player;
	public GameVariables vars;

	//general
	public bool awake;

	//moving
	public float speed = 1.0f;
	private float startTime;
	public float journeyLength;
	public Vector3 endPosition;
	public bool moving;
	float xDistance;
	float yDistance;
	public Vector2 moveDirection;
	public Vector3 initialPos;

	//stats
	public int maxHealth;
	public int health;
	public int dmg;

	//health bar
	//public Slider healthBarSlider;

	//step counters
	public int doSteps;
	public int stepsTaken;
	public bool wait;
	public float waitInit;
	Dictionary <string,bool> possDirections;
	// Use this for initialization
	public virtual void Start () {
		//Player.Instance.incrementTotalEnemies();
		player = GameObject.FindWithTag("Player");

		awake = false;
		moving = false;
		stepsTaken = 0;
		wait = false;

		//stats
		maxHealth = 100;
		health = maxHealth;
		dmg = 5;

		//set tag for enemies
		gameObject.tag = "Enemy";

		//register to the step manager
		StepManager.Instance.register (this);
	}
	
	// Update is called once per frame
	public virtual void Update () {
		if(health <= 0){
			die ();
			return;
		}
		if(wait){
			checkWait();
			return;
		}
		if(!awake)
			return;
		if(doSteps > 0 )
		{
			bool canHit = checkCanHit();
			if (awake && !moving && !canHit)
			{	
				/*
				startTime = Time.time;
				initialPos = transform.position;
				endPosition = initialPos;*/
				startTime = Time.time;
				initialPos = transform.position;
				endPosition = initialPos;

				calculateMovement();
				if(!wait){
					journeyLength = Vector3.Distance (initialPos, endPosition);
					stepsTaken++;
					moving = true;
					faceMoveDirection();
				}
				stepsTaken++;
				doSteps--;
				

			}else{
				if(canHit && !moving){
					startWait();
					faceMoveDirection();
					attack();
					doSteps--;
					stepsTaken++;
				}
			}
		}
		if(moving)
			move();
	}

	void move()
	{
		float distCovered = (Time.time - startTime) * speed;
		float fracJourney = distCovered / journeyLength;
		transform.position = Vector3.Lerp (initialPos, endPosition, fracJourney);
		if (transform.position == endPosition) {
			moving = false;
		}
	}

	void faceLeft(){
		transform.rotation = Quaternion.Euler(0, 0, -90);
	}
	void faceRight(){
		transform.rotation = Quaternion.Euler(0, 0, 90);
	}
	void faceUp(){
		transform.rotation = Quaternion.Euler(0, 0, 180);
	}
	void faceDown(){
		transform.rotation = Quaternion.Euler(0, 0, 0);
	}

	virtual public void faceMoveDirection()
	{
		switch((int)moveDirection.x){
		case 1: faceRight(); break;
		case -1: faceLeft(); break;
		}
		switch((int)moveDirection.y){
		case 1: faceUp(); break;
		case -1: faceDown(); break;
		}
	}

	bool checkCanHit()
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position , -Vector2.up, 1, LayerMask.GetMask("Player"));
		if (hit.collider != null) {
			moveDirection= new Vector2(0,-1);
			return true;
		}
		hit = Physics2D.Raycast(transform.position , Vector2.up, 1, LayerMask.GetMask("Player"));
		if (hit.collider != null) {
			moveDirection= new Vector2(0,1);
			return true;
		}
		hit = Physics2D.Raycast(transform.position, Vector2.right, 1, LayerMask.GetMask("Player"));
		if (hit.collider != null) {
			moveDirection= new Vector2(1,0);
			return true;
		}
		hit = Physics2D.Raycast(transform.position, -Vector2.right, 1, LayerMask.GetMask("Player"));
		if (hit.collider != null) {
			moveDirection= new Vector2(-1,0);
			return true;
		}
		return false;
	}

	private void getPossDirections()
	{
		int layer = LayerMask.NameToLayer("RaycastLayer");
		possDirections = new Dictionary<string, bool>();
		RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, 0, 0), Vector2.up, 1,layer);
		possDirections.Add("up", (hit.collider == null));
		hit = Physics2D.Raycast(transform.position + new Vector3(0, 0, 0), -Vector2.up, 1,layer);
		possDirections.Add("down", (hit.collider == null));
		hit = Physics2D.Raycast(transform.position + new Vector3(0, 0, 0), -Vector2.right, 1,layer);
		possDirections.Add("left", (hit.collider == null));
		hit = Physics2D.Raycast(transform.position + new Vector3(0, 0, 0), Vector2.right, 1,layer);
		possDirections.Add("right", (hit.collider == null));
	}

	private void calculateMovement()
	{
		getPossDirections();
		Vector2 targetPos = player.transform.position;
		xDistance = targetPos.x - transform.position.x;
		yDistance = targetPos.y - transform.position.y;
		bool right, left, up, down;
		possDirections.TryGetValue("right", out right);
		if(Mathf.Abs(xDistance) > Mathf.Abs(yDistance) && xDistance > 0  && right)
		{
			moveDirection = new Vector2( 1, 0);
			endPosition += new Vector3(1, 0, 0);
			return;
		}
		possDirections.TryGetValue("left", out left);
		if(Mathf.Abs(xDistance) > Mathf.Abs(yDistance) && xDistance < 0  && left)
		{
			moveDirection = new Vector2( 1, 0);
			endPosition += new Vector3(1, 0, 0);
			return;
		}
		possDirections.TryGetValue("up", out up);
		possDirections.TryGetValue("down", out down);
		if((up && !down)|| (up && yDistance > 0 && down))
		{
			moveDirection = new Vector2( 0, 1);
			endPosition += new Vector3(0, 1, 0);
			return;
		}
		if(down)
		{
			moveDirection = new Vector2( 0, -1);
			endPosition += new Vector3(0, -1, 0);
			return;
		}
		if((right && !left)|| (right && xDistance > 0 && left))
		{
			moveDirection = new Vector2( 1, 0);
			endPosition += new Vector3(1, 0, 0);
			return;
		}
		if(left)
		{
			moveDirection = new Vector2( 1, 0);
			endPosition += new Vector3(1, 0, 0);
			return;
		}
		startWait();
	}
	
	
	public virtual void attack()
	{
		startWait();
		Debug.Log ("ATTACK");
		Player.Instance.setHealth(-dmg);
	}

	public void takeDamage(int damage)
	{
		health -= damage;
	}

	public void incrementSteps()
	{
		doSteps++;
	}

	public void die()
	{
		Player.Instance.incrementEnemiesKilled();
		//dropItem();
		Destroy(gameObject);
	}

	public void dropItem(GameObject o){
		GameObject newdrop = Instantiate(o);
		newdrop.transform.position = transform.position;
	}

	public void startWait(){
		waitInit = Time.time;
		wait = true;
	}
	public bool checkWait()
	{
		if(Time.time - waitInit >= 1)
			wait = false;
		return wait;
	}

	
	
}
	