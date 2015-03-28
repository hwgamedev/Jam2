using UnityEngine;
using System.Collections;

public class EnemyBase : MonoBehaviour {

	public GameObject player;
	public GameVariables vars;

	//general
	public bool awake;

	//moving
	public float speed = 1.0f;
	private float startTime;
	private float journeyLength;
	public Vector3 endPosition;
	public bool moving;
	float xDistance;
	float yDistance;
	Vector2 moveDirection;
	public Vector3 initialPos;

	//stats
	public int health;
	public int dmg;

	//step counters
	public int doSteps;
	public int stepsTaken;

	// Use this for initialization
	public virtual void Start () {
		player = GameObject.FindWithTag("Player");
		vars = (GameVariables) GameObject.FindWithTag("GameVariables").GetComponent("GameVariables");

		awake = true;
		moving = false;
		//TO DO - remove
		vars.TileSize = 1;
		doSteps = 8;
		//------------
		stepsTaken = 0;

		//stats
		health = 100;
		dmg = 5;
	}
	
	// Update is called once per frame
	public virtual void Update () {
		if(health <= 0){
			die ();
			return;
		}
		if(doSteps > 0 )
		{
			bool canHit = checkCanHit();
			if (awake && !moving && !canHit)
			{
				Vector2 targetPos = player.transform.position;
				xDistance = targetPos.x - transform.position.x;
				yDistance = targetPos.y - transform.position.y;
				startTime = Time.time;
				initialPos = transform.position;
				endPosition = initialPos;
				if(Mathf.Abs(xDistance) > Mathf.Abs(yDistance)){
					if ( xDistance > 0 ){
						moveDirection = new Vector2( 1, 0);
						endPosition += new Vector3(1, 0, 0);
					}else{
						moveDirection = new Vector2( -1, 0);
						endPosition -= new Vector3(1, 0, 0);
					}
				}else {
					if ( yDistance > 0 ){
						moveDirection = new Vector2(0, 1);
						endPosition += new Vector3(0, 1, 0);
					}else
						moveDirection = new Vector2( 0, -1);
						endPosition -= new Vector3(0, 1, 0);
				}
				journeyLength = Vector3.Distance (initialPos, endPosition);
				doSteps--;
				stepsTaken++;
				moving = true;
				faceMoveDirection();

			}else{
				if(canHit && !moving){
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

	void faceMoveDirection()
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
		RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, vars.TileSize/2, 0), -Vector2.up, vars.TileSize, LayerMask.GetMask("Player"));
		if (hit.collider != null) {
			moveDirection= new Vector2(0,-1);
			return true;
		}
		hit = Physics2D.Raycast(transform.position + new Vector3(0, vars.TileSize/2, 0), Vector2.up, vars.TileSize, LayerMask.GetMask("Player"));
		if (hit.collider != null) {
			moveDirection= new Vector2(0,1);
			return true;
		}
		hit = Physics2D.Raycast(transform.position + new Vector3(0, vars.TileSize/2, 0), Vector2.right, vars.TileSize, LayerMask.GetMask("Player"));
		if (hit.collider != null) {
			moveDirection= new Vector2(1,0);
			return true;
		}
		hit = Physics2D.Raycast(transform.position + new Vector3(0, vars.TileSize/2, 0), -Vector2.right, vars.TileSize, LayerMask.GetMask("Player"));
		if (hit.collider != null) {
			moveDirection= new Vector2(-1,0);
			return true;
		}
		return false;
	}

	void attack()
	{
		Debug.Log("DIE");
	}

	public void takeDamage(int damage)
	{
		health -= damage;
	}

	void die()
	{
		Destroy(this);
	}

}
	