using UnityEngine;
using System.Collections;

public class characterControls : MonoBehaviour {

	//public GameObject character;

	// stats of character
	public float speed = 1.0f;
	public float shortRange = 1.5f;
	public float longRange = 11f;

	private Animator anim;

	// movement helpers
	private float startTime;
	private float journeyLength;
	private Vector3 startPosition;
	private Vector3 endPosition;

	//states
	private bool moving = false;
	private bool attacking = false;

	//if need be to add a particle system when attacking
	//public ParticleSystem trace;
	public GameObject dagger;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		//synchronise with player data
		Player.Instance.incrementSteps (50);
	}
	
	// Update is called once per frame
	void Update () {
		if (Player.Instance.getHealth() <= 0) {
			doKill();
			return;
		}
		if (Player.Instance.getSteps() == 0) {
			doJump();
			return;
		} else {
			if (!moving && !attacking) {
        		if (Input.GetKey(KeyCode.W))
	            {
	                if (checkForCollisions(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.position + new Vector3(0, 1, 0)))
	                    return;
	                anim.SetTrigger("iddleN");
					startTime = Time.time;
					startPosition = transform.position;
					endPosition = startPosition;
					endPosition += new Vector3 (0, 1, 0);
					journeyLength = Vector3.Distance (startPosition, endPosition);
					doStep();
					moving = true;
				}
	            if (Input.GetKey(KeyCode.A))
	            {
	                if (checkForCollisions(new Vector3(transform.position.x-0.5f, transform.position.y, transform.position.z),transform.position - new Vector3(1, 0, 0)))
	                    return;
	                anim.SetTrigger("iddleW");
					startTime = Time.time;
					startPosition = transform.position;
					endPosition = startPosition;
					endPosition -= new Vector3 (1, 0, 0);
					journeyLength = Vector3.Distance (startPosition, endPosition);
					doStep();
					moving = true;
				}
	            if (Input.GetKey(KeyCode.S))
	            {
	                if (checkForCollisions(new Vector3(transform.position.x, transform.position.y-.5f, transform.position.z),transform.position - new Vector3(0, 1, 0)))
	                    return;
	                anim.SetTrigger("iddleS");
					startTime = Time.time;
					startPosition = transform.position;
					endPosition = startPosition;
					endPosition -= new Vector3 (0, 1, 0);
					journeyLength = Vector3.Distance (startPosition, endPosition);
					doStep();
					moving = true;
				}
	            if (Input.GetKey(KeyCode.D))
	            {
	                if (checkForCollisions(new Vector3(transform.position.x+0.5f, transform.position.y, transform.position.z),transform.position + new Vector3(1, 0, 0)))
	                    return;
					anim.SetTrigger("iddleE");
					startTime = Time.time;
					startPosition = transform.position;
					endPosition = startPosition;
					endPosition += new Vector3 (1, 0, 0);
					journeyLength = Vector3.Distance (startPosition, endPosition);
					doStep();
					moving = true;
				}
				if (Input.GetMouseButtonDown (0)) {
					Vector3 mousePoint = Camera.main.ScreenPointToRay (Input.mousePosition).origin;
					float diffX = mousePoint.x - transform.position.x;
					float diffY = mousePoint.y - transform.position.y;
					if (diffX > -0.7 && diffX < 0.7) {
						if (diffY > 1 && diffY < 2) {
							anim.SetTrigger ("attackN");
							doStep();
							//trace.transform.rotation = Quaternion.Euler(0,0,89f);
							//trace.Play();
							attacking = true;
							doAttack ("N");
						} else if (diffY < 0 && diffY > -1) {
							anim.SetTrigger ("attackS");
							doStep();
							//trace.transform.rotation = Quaternion.Euler(0,0,-91f);
							//trace.Play();
							attacking = true;
							doAttack ("S");
						}
					} else if (diffY < 1 && diffY > 0) {
						if (diffX < -0.7 && diffX > -1.7) {
							anim.SetTrigger ("attackW");
							doStep();
							//trace.transform.rotation = Quaternion.Euler(0,0,-179f);
							//trace.Play();
							attacking = true;
							doAttack ("W");
						} else if (diffX > 0.7 && diffX < 1.7) {
							anim.SetTrigger ("attackE");
							doStep();
							//trace.transform.rotation = Quaternion.Euler(0,0,-1f);
							//trace.Play();
							attacking = true;
							doAttack ("E");
						}
					} 
					if (!attacking) {

						float length = Mathf.Sqrt (Mathf.Pow (diffX,2) + Mathf.Pow (diffY,2));
						print (longRange/(6-Player.Instance.getReach()));
						print (length);
						if(length <= longRange/(6-Player.Instance.getReach()) && length > shortRange) {
							print ("yo");
							throwDagger(transform.position, mousePoint);
							doStep ();
						}
					}
				}
			}
		}
		if (moving) {
			doLerp ();
		}
	}

	private void doStep(){
		StepManager.Instance.newStep ();
		// notify game mechanic of step performed by player
	}

	private bool checkForCollisions(Vector3 startPoint, Vector3 endPoint)
    {
        Debug.DrawLine(transform.position, endPoint);
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
            if (hit.collider.GetComponent<OpenTreasure>() != null) { hit.collider.GetComponent<OpenTreasure>().spawnPrize(); }
            print("Colliding with: "+hit.collider.gameObject.name);
            return true;
        }

        return false;
    }

	private void doLerp() {
		float distCovered = (Time.time - startTime) * speed;
		float fracJourney = distCovered / journeyLength;
		transform.position = Vector3.Lerp (startPosition, endPosition, fracJourney);
		if (transform.position == endPosition) {
			moving = false;
		}
	}

	private void doAttack(string direction){
		RaycastHit2D hit = Physics2D.Linecast(transform.position, transform.position + new Vector3(0f,shortRange,0f));
		switch (direction) {
		case "N":
			break;
		case "S":
			hit = Physics2D.Linecast(transform.position, transform.position - new Vector3(0f,shortRange,0f));
			break;
		case "W":
			hit = Physics2D.Linecast(transform.position, transform.position - new Vector3(shortRange,0f,0f));
			break;
		case "E":
			hit = Physics2D.Linecast(transform.position, transform.position + new Vector3(shortRange,0f,0f));
			break;
		}
		if (hit && !hit.collider.isTrigger && hit.collider.gameObject.CompareTag("Enemy")) {
			hit.collider.gameObject.GetComponent<EnemyBase>().takeDamage(Player.Instance.getDamage()*2);
		}
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("iddleN") ||
			anim.GetCurrentAnimatorStateInfo (0).IsName ("iddleS") ||
			anim.GetCurrentAnimatorStateInfo (0).IsName ("iddleE") ||
			anim.GetCurrentAnimatorStateInfo (0).IsName ("iddleW")) {
			attacking = false;
		}
	}

	private void throwDagger(Vector3 start, Vector3 end){
		GameObject daggerInstance = Instantiate(dagger, start, Quaternion.identity) as GameObject;
		daggerInstance.GetComponent<Dagger> ().throwDagger (start, end);
	}

	private void doJump(){
		print ("teleport !!");
	}

	private void doKill(){
		if(!anim.GetCurrentAnimatorStateInfo(0).IsName("die")){
			anim.SetTrigger ("kill");
		}
		Player.Instance.death ();
	}

	public void takeDammages(int dammages) {
		int health = Player.Instance.getHealth ();
		health -= dammages;
		Player.Instance.setHealth (health);
	}
}
