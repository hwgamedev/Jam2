using UnityEngine;
using System.Collections;

public class characterControls : MonoBehaviour {

	//public GameObject character;

	// stats of character
	public float speed = 1.0f;
	public float shortRange = 0f;
	public float longRange = 11f;

    private FigureMovement mover;

	private Animator anim;

	// movement helpers
	private float startTime;
	private float journeyLength;
	private Vector3 startPosition;
	private Vector3 endPosition;

	//states
	public bool attacking = false;

	//if need be to add a particle system when attacking
	//public ParticleSystem trace;
	public GameObject dagger;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		//synchronise with player data
		Player.Instance.incrementSteps (50);

        mover = GetComponent<FigureMovement>();
	}

	// Update is called once per frame
	void Update () {

        //check if the player is not in the middle of something right now
		if (!mover.isMoving() && !attacking) {
            //gather the directions of movement
            int[] movementDir = getMovementDir();
            //check for movement
            if (movementDir[0] != 0 || movementDir[1] != 0)
            {
                if(checkForCollisions(movementDir[0],movementDir[1])) return;
                mover.startMove(movementDir[0], -1*movementDir[1]);
                doStep();
            }

            //check for ranged attack
            else if (Input.GetMouseButtonDown(1))
            {
                if (Player.Instance.getAmmo() > 0)
                {
                    tryThrowDagger();
                }
            }
            //check for melee attack
            else if (Input.GetMouseButtonDown(0))
            {
                tryAttack();
            }
		}
	}

    private int[] getMovementDir()
    {
        int xMov = 0, yMov = 0;

        //check for movement up
        if (Input.GetKey(KeyCode.W))
        {
            yMov = 1;
            anim.SetTrigger("iddleN");
        }

        //check for movement left
        else if (Input.GetKey(KeyCode.A))
        {
            xMov = -1;
            anim.SetTrigger("iddleW");
        }

        //check for movement down
        else if (Input.GetKey(KeyCode.S))
        {
            yMov = -1;
            anim.SetTrigger("iddleS");
        }

        //check for movement right
        else if (Input.GetKey(KeyCode.D))
        {
            xMov = 1;
            anim.SetTrigger("iddleE");
        }

        int[] result = { xMov, yMov };
        return result;
    }

    private void tryThrowDagger()
    {
        Vector3 mousePoint = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
        float diffX = mousePoint.x - transform.position.x;
        float diffY = mousePoint.y - transform.position.y;
        float length = Mathf.Sqrt(Mathf.Pow(diffX, 2) + Mathf.Pow(diffY, 2));
        if (length <= longRange / (6 - Player.Instance.getReach()))
        {
            throwDagger(transform.position, mousePoint);
            Player.Instance.throwAmmo();
            doStep();
        }
    }

    private void tryAttack()
    {
        //work out direction of click from player
        Vector3 mousePoint = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
        float diffX = mousePoint.x - transform.position.x;
        float diffY = mousePoint.y - transform.position.y;
        float angle = Mathf.Atan2(diffX, diffY);
        //Debug.Log("Angle: " + (Mathf.Rad2Deg * angle));

        //determine which of the 4 directions it falls in to
        string attackDir = null;
        if (angle > Mathf.Deg2Rad * -45 && angle <= Mathf.Deg2Rad * 45)
            attackDir = "N";
        else if (angle > Mathf.Deg2Rad * 45 && angle <= Mathf.Deg2Rad * 135)
            attackDir = "E";
        else if (Mathf.Abs(angle) > Mathf.Deg2Rad * 135)
            attackDir = "S";
        else if (angle > Mathf.Deg2Rad * -135 && angle <= Mathf.Deg2Rad * -45)
            attackDir = "W";

        //execute attack
        if (attackDir != null)
        {
            anim.SetTrigger("attack" + attackDir);
            attacking = true;
            doAttack(attackDir);
            doStep();
        }
    }

    public void teleport()
    {
        mover.stopMove();
        attacking = false;
        anim.SetTrigger("tele");
    }

	private void doStep(){
		StepManager.Instance.newStep ();
		// notify game mechanic of step performed by player
	}

	private bool checkForCollisions(int xDir, int yDir)
    {
        float offset = 0.6f;
        float lineEnd = 1.4f;
        Vector3 startPoint = new Vector3(transform.position.x + offset*xDir,
                                        transform.position.y + offset*yDir,
                                        transform.position.z);
        Vector3 endPoint = transform.position+new Vector3(lineEnd*xDir, lineEnd*yDir, 0);

        //Debug.DrawLine(startPoint, endPoint);


        RaycastHit2D hit = Physics2D.Linecast(startPoint, endPoint);
        if (hit && !hit.collider.isTrigger)
        {
			if (hit.collider.GetComponent<Dagger>() != null) { return false; }
            if (hit.collider.GetComponent<GoldPickup>() != null) { hit.collider.GetComponent<GoldPickup>().collect(-1); }
			if (hit.collider.GetComponent<HealthPickup>() != null) { hit.collider.GetComponent<HealthPickup>().collect(); }
			if (hit.collider.GetComponent<IncreaserPickup>() != null) { hit.collider.GetComponent<IncreaserPickup>().collect(); }
			if (hit.collider.GetComponent<DecreaserPickup>() != null) { hit.collider.GetComponent<DecreaserPickup>().collect(); }
            if (hit.collider.GetComponent<TeleporterPickup>() != null) { hit.collider.GetComponent<TeleporterPickup>().collect(); }
            if (hit.collider.GetComponent<AmmoPickup>() != null) { hit.collider.GetComponent<AmmoPickup>().collect(); }
            if (hit.collider.GetComponent<OpenTreasure>() != null) { hit.collider.GetComponent<OpenTreasure>().spawnPrize(); }
            //print("Colliding with: "+hit.collider.gameObject.name);
            return true;
        }

        return false;
    }

	private void doAttack(string direction){
        //Debug.Log("Attacking!");
        //print("Remaining steps: " + Player.Instance.getSteps());
		RaycastHit2D hit = Physics2D.Linecast(transform.position+new Vector3(0,0.5f,0), transform.position + new Vector3(0f,shortRange,0f));
		switch (direction) {
		case "N":
			break;
		case "S":
            hit = Physics2D.Linecast(transform.position + new Vector3(0, -0.5f, 0), transform.position - new Vector3(0f, shortRange, 0f));
			break;
		case "W":
            hit = Physics2D.Linecast(transform.position + new Vector3(-0.5f,0, 0), transform.position - new Vector3(shortRange, 0f, 0f));
			break;
		case "E":
            hit = Physics2D.Linecast(transform.position + new Vector3(0.5f, 0, 0), transform.position + new Vector3(shortRange, 0f, 0f));
			break;
		}
		if (hit && !hit.collider.isTrigger && hit.collider.gameObject.CompareTag("Enemy")) {
			hit.collider.gameObject.GetComponent<EnemyBase>().takeDamage(Player.Instance.getDamage()*2);
		}
        else if (hit && !hit.collider.isTrigger && hit.collider.gameObject.CompareTag("Breakable"))
        {
            hit.collider.gameObject.GetComponent<Breakable>().attacked();
        }
	}

	private void throwDagger(Vector3 start, Vector3 end){
		GameObject daggerInstance = Instantiate(dagger, start, Quaternion.identity) as GameObject;
        
		daggerInstance.GetComponent<Dagger> ().throwDagger (start, end);
	}

	private void doKill(){
		if(!anim.GetCurrentAnimatorStateInfo(0).IsName("die")){
			anim.SetTrigger("kill");
		}
		//Player.Instance.death ();
	}

	public void resetSteps(){
		Player.Instance.incrementSteps(10);
	}

	public void destroy(){
		Destroy (gameObject);
	}

    public void disableAttacking()
    {
        attacking = false;
    }
}
