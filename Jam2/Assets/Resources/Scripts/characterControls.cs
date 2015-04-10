using UnityEngine;
using System.Collections;

public class characterControls : MonoBehaviour {

	//public GameObject character;

	// stats of character
	public float speed = 1.0f;
	public int shortRange = 1;
	public float longRange = 11f;
    private int movementRange = 1;

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
            //check for movement
            if (tryMove() == true) return;
            //check for ranged attack
            else if (tryThrowDagger() == true) return;
            //check for melee attack
            else if (tryAttack() == true) return;
		}
	}

    private bool tryMove()
    {
        int xMov = 0, yMov = 0;

        //check for movement up
        if (Input.GetKey(KeyCode.W)) { yMov = 1; anim.SetTrigger("iddleN"); }
        //check for movement left
        else if (Input.GetKey(KeyCode.A)) { xMov = -1; anim.SetTrigger("iddleW"); }
        //check for movement down
        else if (Input.GetKey(KeyCode.S)) { yMov = -1; anim.SetTrigger("iddleS"); }
        //check for movement right
        else if (Input.GetKey(KeyCode.D)) { xMov = 1; anim.SetTrigger("iddleE"); }

        if (xMov != 0 || yMov != 0)
        {
            GameObject go = checkForCollisions(xMov,yMov, movementRange);
            if (go != null) { handlePickup(go); return false; }
            mover.startMove(xMov, -1 * yMov);
            doStep();
            return true;
        }

        return false;
    }

    private bool tryThrowDagger()
    {
        if (!Input.GetMouseButtonDown(1) || 
            Player.Instance.getAmmo() > 0) return false;

        Vector3 mousePoint = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
        float diffX = mousePoint.x - transform.position.x;
        float diffY = mousePoint.y - transform.position.y;
        float length = Mathf.Sqrt(Mathf.Pow(diffX, 2) + Mathf.Pow(diffY, 2));
        if (length <= longRange / (6 - Player.Instance.getReach()))
        {
            throwDagger(transform.position, mousePoint);
            Player.Instance.throwAmmo();
            doStep();
            return true;
        }

        return false;
    }

    private bool tryAttack()
    {
        if (!Input.GetMouseButtonDown(0)) return false;

        int x = 0, y = 0;
        //work out direction of click from player
        Vector3 mousePoint = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
        float angle = Mathf.Atan2(mousePoint.x - transform.position.x, mousePoint.y - transform.position.y);

        //determine which of the 4 directions it falls in to
        string attackDir = null;
        if (angle > Mathf.Deg2Rad * -45 && angle <= Mathf.Deg2Rad * 45) { attackDir = "N"; y = 1; }
        else if (angle > Mathf.Deg2Rad * 45 && angle <= Mathf.Deg2Rad * 135) { attackDir = "E"; x = 1; }
        else if (Mathf.Abs(angle) > Mathf.Deg2Rad * 135) { attackDir = "S"; y = -1; }
        else if (angle > Mathf.Deg2Rad * -135 && angle <= Mathf.Deg2Rad * -45) { attackDir = "W"; x = -1; }

        //execute attack
        if (attackDir != null)
        {
            //get the collission object
            GameObject obj = checkForCollisions(x, y, shortRange);
            if (obj != null) 
            {
                if (obj.CompareTag("Enemy")) obj.GetComponent<EnemyBase>().takeDamage(Player.Instance.getDamage() * 2);
                else if(obj.CompareTag("Breakable")) obj.GetComponent<Breakable>().attacked();
            }

            anim.SetTrigger("attack" + attackDir);
            attacking = true;
            doStep();
            return true;
        }
        return false;
    }

    private void handlePickup(GameObject go)
    {
        //this is where all of the collectibles are handled
        if (go.GetComponent<GoldPickup>() != null) { go.GetComponent<GoldPickup>().collect(-1); }
        if (go.GetComponent<HealthPickup>() != null) { go.GetComponent<HealthPickup>().collect(); }
        if (go.GetComponent<IncreaserPickup>() != null) { go.GetComponent<IncreaserPickup>().collect(); }
        if (go.GetComponent<DecreaserPickup>() != null) { go.GetComponent<DecreaserPickup>().collect(); }
        if (go.GetComponent<TeleporterPickup>() != null) { go.GetComponent<TeleporterPickup>().collect(); }
        if (go.GetComponent<AmmoPickup>() != null) { go.GetComponent<AmmoPickup>().collect(); }
        if (go.GetComponent<OpenTreasure>() != null) { go.GetComponent<OpenTreasure>().spawnPrize(); }
		if (go.GetComponent<Web>() != null) { go.GetComponent<Web>().webAction(); }
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

	private GameObject checkForCollisions(int xDir, int yDir, int range)
    {
        //offset from centre of tile where the player is
        float playerOffset = 0.5f;
        //margin from side of tile, as we don't want to scan from beginning of tile to end, just to be safe
        float tileMargin = 0.1f;
        //line length (subtract 1 from range because it's 1 by default)
        float lineLength = 0.8f+(range-1);
        //start and end position of line
        float lineStart = playerOffset + tileMargin;
        float lineEnd = lineStart + lineLength;

        //start and end vectors of collission check
        Vector3 startPoint = new Vector3(transform.position.x + lineStart*xDir,
                                        transform.position.y + lineStart * yDir,
                                        transform.position.z);
        Vector3 endPoint = transform.position+new Vector3(lineEnd*xDir, lineEnd*yDir, 0);

        Debug.DrawLine(startPoint, endPoint);

        RaycastHit2D hit = Physics2D.Linecast(startPoint, endPoint);
        if (hit && !hit.collider.isTrigger)
        {
            return hit.collider.gameObject;
        }

        return null;
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
