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
	void FixedUpdate () {

        //Debug.Log("Updated in " + Time.deltaTime);
        //if (Player.Instance.getHealth() <= 0) {
        //    doKill();
        //    return;
        //}
		//if (Player.Instance.getSteps() == 0) {
		//	doJump();
		//	return;
		//} else {

        /*if (anim.GetCurrentAnimatorStateInfo(0).IsName("iddleN") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("iddleS") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("iddleE") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("iddleW"))
        {
            attacking = false;
        }*/

		if (!mover.moving && !attacking) {
        	if (Input.GetKey(KeyCode.W))
	        {
                anim.SetTrigger("iddleN");
	            if (checkForCollisions(new Vector3(transform.position.x, transform.position.y + 0.9f, transform.position.z), transform.position + new Vector3(0, 1.1f, 0)))
	                return;
				startTime = Time.time;
				
				endPosition = startPosition;
				endPosition += new Vector3 (0, 1, 0);
                journeyLength = Vector3.Distance(startPosition, endPosition);
                doStep();

                mover.startMove(0, -1);
			}
	        else if (Input.GetKey(KeyCode.A))
	        {
                anim.SetTrigger("iddleW");
	            if (checkForCollisions(new Vector3(transform.position.x-0.75f, transform.position.y, transform.position.z),transform.position - new Vector3(1.25f, 0, 0)))
	                return;
				startTime = Time.time;
				startPosition = transform.position;
				endPosition = startPosition;
				endPosition -= new Vector3 (1, 0, 0);
                journeyLength = Vector3.Distance(startPosition, endPosition);
				doStep();

                mover.startMove(-1, 0);
			}
	        else if (Input.GetKey(KeyCode.S))
	        {
                anim.SetTrigger("iddleS");
	            if (checkForCollisions(new Vector3(transform.position.x, transform.position.y-.75f, transform.position.z),transform.position - new Vector3(0, 1.25f, 0)))
	                return;
				startTime = Time.time;
                startPosition = transform.position;
				endPosition = startPosition;
				endPosition -= new Vector3 (0, 1, 0);
                journeyLength = Vector3.Distance(startPosition, endPosition);
                doStep();

                mover.startMove(0,1);
			}
	        else if (Input.GetKey(KeyCode.D))
	        {
                anim.SetTrigger("iddleE");
	            if (checkForCollisions(new Vector3(transform.position.x+0.75f, transform.position.y, transform.position.z),transform.position + new Vector3(1.25f, 0, 0)))
	                return;
				startTime = Time.time;
				startPosition = transform.position;
				endPosition = startPosition;
				endPosition += new Vector3 (1, 0, 0);
                journeyLength = Vector3.Distance(startPosition, endPosition);
				doStep();

                mover.startMove(1, 0);
			}
            else if (Input.GetMouseButtonDown(1))
            {
                if (Player.Instance.getAmmo() > 0)
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
            }
            else if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePoint = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
                float diffX = mousePoint.x - transform.position.x;
                float diffY = mousePoint.y - transform.position.y;
                float angle = Mathf.Atan2(diffX, diffY);
                Debug.Log("Angle: "+(Mathf.Rad2Deg*angle));
                if (angle > Mathf.Deg2Rad*-45 && angle <= Mathf.Deg2Rad*45)
                {
                    anim.SetTrigger("attackN");
                    doStep();
                    //trace.transform.rotation = Quaternion.Euler(0,0,89f);
                    //trace.Play();
                    attacking = true;
                    doAttack("N");
                }
                else if(angle > Mathf.Deg2Rad*45 && angle <= Mathf.Deg2Rad*135)
                {
                    anim.SetTrigger("attackE");
                    doStep();
                    //trace.transform.rotation = Quaternion.Euler(0,0,-1f);
                    //trace.Play();
                    attacking = true;
                    doAttack("E");
                }
                else if (Mathf.Abs(angle) > Mathf.Deg2Rad * 135)
                {
                    anim.SetTrigger("attackS");
                    doStep();
                    //trace.transform.rotation = Quaternion.Euler(0,0,-91f);
                    //trace.Play();
                    attacking = true;
                    doAttack("S");
                }
                else if(angle > Mathf.Deg2Rad * -135 && angle <= Mathf.Deg2Rad * -45)
                {
                    anim.SetTrigger("attackW");
                    doStep();
                    //trace.transform.rotation = Quaternion.Euler(0,0,-179f);
                    //trace.Play();
                    attacking = true;
                    doAttack("W");
                }
                /*if (diffX > -0.5 && diffX < 0.5)
                {
                    if (diffY > 0.5 && diffY < 1.5)
                    {
                    }
                    else if (diffY < -.5 && diffY > -1.5)
                    {
                    }
                }
                else if (diffY < .5 && diffY > -.5)
                {
                    if (diffX < -0.5 && diffX > -1.5)
                    {
                    }
                    else if (diffX > 0.5 && diffX < 1.5)
                    {
                    }
                }*/
            }
		}
		//}
        if (mover.moving)
        {
            mover.moveSine();
			//doLerp ();
		}
	}

    public void teleport()
    {
        attacking = false;
        anim.SetTrigger("tele");
    }

	private void doStep(){
		StepManager.Instance.newStep ();
		// notify game mechanic of step performed by player
	}

	private bool checkForCollisions(Vector3 startPoint, Vector3 endPoint)
    {
        //Debug.DrawLine(startPoint, endPoint);
        int layer = LayerMask.NameToLayer("RaycastLayer");
        //print("Layer : "+layer);
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

	private void doLerp() {
		float distCovered = (Time.time - startTime) * speed;
		float fracJourney = distCovered / journeyLength;
		transform.position = Vector3.Lerp (startPosition, endPosition, fracJourney);
		if (transform.position == endPosition) {
			//moving = false;
		}
	}
    

	private void doAttack(string direction){
        Debug.Log("Attacking!");
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

	private void doJump(){
        //print ("teleport !!");
	}

	private void doKill(){
		if(!anim.GetCurrentAnimatorStateInfo(0).IsName("die")){
			anim.SetTrigger("kill");
		}
		//Player.Instance.death ();
	}

	/*public void takeDammages(int dammages) {
		int health = Player.Instance.getHealth ();
		health -= dammages;
		Player.Instance.setHealth (health);
	}*/

	public void resetSteps(){
		Player.Instance.incrementSteps(10);
	}

	public void destroy(){
		Destroy (gameObject);
	}

    public void setMoving(bool mov)
    {
        //mover.moving = mov;
    }
    //public void takeDammages(int dammages) {
    //    int health = Player.Instance.getHealth ();
    //    health -= dammages;
    //    Player.Instance.setHealth (health);
    //}

    public void disableAttacking()
    {
        attacking = false;
    }
}
