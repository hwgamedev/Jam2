using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EnemyBase : MonoBehaviour {

	public GameObject player;
	public GameVariables vars;

	//stats
	public int maxHealth;
	public int health;
	public int dmg;

	//step counters
	public int doSteps;
    public int stepsTaken;
	Dictionary <string,bool> possDirections;

    //need to know which room the enemy's at
    private string room;

    //movement
    private FigureMovement mover;
    float xDistance, yDistance;

	//
	public List<GameObject> drop = new List<GameObject>();

	// Use this for initialization
	public virtual void Start () {
        mover = GetComponent<FigureMovement>();
		//Player.Instance.incrementTotalEnemies();
		player = GameObject.FindWithTag("Player");
		//initDrops ();

		//stats
		maxHealth = 6;
		health = maxHealth;
		dmg = 5;

		//set tag for enemies
		gameObject.tag = "Enemy";

		//register to the step manager
		StepManager.Instance.register (this);
	}
	
	// Update is called once per frame
    public virtual void Update()
    {
		if(doSteps > 0 )
		{
            if (checkShouldWait())
            {
                doSteps--;
                return;
            }
			bool canHit = checkCanHit();
			if (!mover.isMoving() && !canHit)
			{
                stepsTaken++;
				calculateMovement();
				faceMoveDirection();
				doSteps--;
				

			}else if (canHit && !mover.isMoving())
            {
				attack();
				doSteps--;
			}
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
        switch((int)mover.getMovementX()) {
		case 1: faceRight(); break;
		case -1: faceLeft(); break;
		}
        switch((int)mover.getMovementY()) {
		case 1: faceDown(); break;
		case -1: faceUp(); break;
		}
	}

    bool checkShouldWait()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, 0.75f, 0), Vector2.up, .5f);
        if (hit.collider != null && hit.collider.tag.Equals("PlayerMover"))
        {
            Debug.Log("Should wait up!");
            faceDown();
            return true;
        }
        hit = Physics2D.Raycast(transform.position + new Vector3(0, -0.75f, 0), -Vector2.up, .5f);
        if (hit.collider != null && hit.collider.tag.Equals("PlayerMover"))
        {
            Debug.Log("Should wait down!");
            faceUp();
            return true;
        }
        hit = Physics2D.Raycast(transform.position + new Vector3(-0.75f, 0, 0), -Vector2.right, .5f);
        if (hit.collider != null && hit.collider.tag.Equals("PlayerMover"))
        {
            Debug.Log("Should wait left!");
            faceLeft();
            return true;
        }
        hit = Physics2D.Raycast(transform.position + new Vector3(0.75f, 0, 0), Vector2.right, .5f);
        if (hit.collider != null && hit.collider.tag.Equals("PlayerMover"))
        {
            Debug.Log("Should wait right!");
            faceRight();
            return true;
        }

        return false;
    }

	bool checkCanHit()
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position , -Vector2.up, 1, LayerMask.GetMask("Player"));
		if (hit.collider != null) {
            faceDown();
			return true;
		}
		hit = Physics2D.Raycast(transform.position , Vector2.up, 1, LayerMask.GetMask("Player"));
		if (hit.collider != null) {
            faceUp();
			return true;
		}
		hit = Physics2D.Raycast(transform.position, Vector2.right, 1, LayerMask.GetMask("Player"));
		if (hit.collider != null) {
            faceRight();
			return true;
		}
		hit = Physics2D.Raycast(transform.position, -Vector2.right, 1, LayerMask.GetMask("Player"));
		if (hit.collider != null) {
            faceLeft();
			return true;
		}
		return false;
	}

	private void getPossDirections()
	{	
		int layer = LayerMask.GetMask("RaycastLayer"); 
		possDirections = new Dictionary<string, bool>();
		//Debug.DrawRay(transform.position + new Vector3(0, 0.75f, 0), Vector2.up, Color.cyan ); 
		RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, 0.75f, 0), Vector2.up, .5f,layer);
		possDirections.Add("up", (hit.collider == null));
        hit = Physics2D.Raycast(transform.position + new Vector3(0, -0.75f, 0), -Vector2.up, .5f, layer);
		possDirections.Add("down", (hit.collider == null));
        hit = Physics2D.Raycast(transform.position + new Vector3(-0.75f, 0, 0), -Vector2.right, .5f, layer);
		possDirections.Add("left", (hit.collider == null));
        hit = Physics2D.Raycast(transform.position + new Vector3(0.75f, 0, 0), Vector2.right, .5f, layer);
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
            mover.startMove(1,0);
			return;
		}
		possDirections.TryGetValue("left", out left);
		if(Mathf.Abs(xDistance) > Mathf.Abs(yDistance) && xDistance < 0  && left)
        {
            mover.startMove(-1, 0);
			return;
		}
		possDirections.TryGetValue("up", out up);
		possDirections.TryGetValue("down", out down);
		if((up && !down)|| (up && yDistance > 0 && down))
		{
            mover.startMove(0, -1);
			return;
		}
		if(down)
        {
            mover.startMove(0, 1);
			return;
		}
		if((right && !left)|| (right && xDistance > 0 && left))
        {
            mover.startMove(1, 0);
			return;
		}
		if(left)
        {
            mover.startMove(-1, 0);
			return;
		}
	}
	
	
	public virtual void attack()
	{
		Player.Instance.setHealth(-dmg);
	}

	public void takeDamage(int damage)
	{
        health -= damage;
        if (health <= 0)
            die();
	}

	public void incrementSteps()
	{
		doSteps++;
	}

	public void die()
	{
        if(Random.Range(0,10) > 3)
        { 
            dropItem(); 
        }
        FindObjectOfType<StepManager>().unsubscribe(this);
        GameObject.Find(room).GetComponent<RoomData>().setEnemyAsDead(this.gameObject);
		Player.Instance.incrementEnemiesKilled();
		Destroy(gameObject);
	}

	public void dropItem(){
		GameObject item = Instantiate(drop[Random.Range(0, drop.Count)]);
		item.transform.position = transform.position;
	}

    public void setRoom(string roomName)
    {
        room = roomName;
    }
}
	