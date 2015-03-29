using UnityEngine;
using System.Collections;

public class characterControls : MonoBehaviour {

	//public GameObject character;

	private Animator anim;
	public float speed = 1.0f;
	private int health = 20;
	private int steps = 10;
	private float startTime;
	private float journeyLength;
	private Vector3 startPosition;
	private Vector3 endPosition;
	private bool moving = false;
	private bool attacking = false;
	public ParticleSystem trace;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (health <= 0) {
			doKill();
		}
		if (steps == 0) {
			doJump();
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
					moving = true;
				}
	            if (Input.GetKey(KeyCode.A))
	            {
	                if (checkForCollisions(new Vector3(transform.position.x-0.5f, transform.position.y, transform.position.z),transform.position - new Vector3(1, 0, 0)))
	                    return;
	                anim.SetTrigger("iddleE");
					startTime = Time.time;
					startPosition = transform.position;
					endPosition = startPosition;
					endPosition -= new Vector3 (1, 0, 0);
					journeyLength = Vector3.Distance (startPosition, endPosition);
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
					moving = true;
				}
	            if (Input.GetKey(KeyCode.D))
	            {
	                if (checkForCollisions(new Vector3(transform.position.x+0.5f, transform.position.y, transform.position.z),transform.position + new Vector3(1, 0, 0)))
	                    return;
					anim.SetTrigger("iddleW");
					startTime = Time.time;
					startPosition = transform.position;
					endPosition = startPosition;
					endPosition += new Vector3 (1, 0, 0);
					journeyLength = Vector3.Distance (startPosition, endPosition);
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
						} else if (diffY < 0 && diffY > -1) {
							anim.SetTrigger ("attackS");
							doStep();
							//trace.transform.rotation = Quaternion.Euler(0,0,-91f);
							//trace.Play();
							attacking = true;
						}
					} else if (diffY < 1 && diffY > 0) {
						if (diffX < -0.7 && diffX > -1.7) {
							anim.SetTrigger ("attackE");
							doStep();
							//trace.transform.rotation = Quaternion.Euler(0,0,-179f);
							//trace.Play();
							attacking = true;
						} else if (diffX > 0.7 && diffX < 1.7) {
							anim.SetTrigger ("attackW");
							doStep();
							//trace.transform.rotation = Quaternion.Euler(0,0,-1f);
							//trace.Play();
							attacking = true;
						}
					}
				}
			}
		}
		if (moving) {
			doLerp ();
		}
		if (attacking) {
			doAttack();
		}
	}

	private void doStep(){
		steps --;
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

	private void doAttack(){
		// TODO proper collision detection with enemies
		//RaycastHit2D hit = Physics2D.Raycast (transform.position, Vector2.right, 5, <enemy layer>);
		//if (hit.collider != null) {
		//	
		//}
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("iddleN") ||
			anim.GetCurrentAnimatorStateInfo (0).IsName ("iddleS") ||
			anim.GetCurrentAnimatorStateInfo (0).IsName ("iddleE") ||
			anim.GetCurrentAnimatorStateInfo (0).IsName ("iddleW")) {
			attacking = false;
		}
	}

	private void doJump(){

	}

	private void doKill(){
		if(!anim.GetCurrentAnimatorStateInfo(0).IsName("die")){
			anim.SetTrigger ("kill");
		}
	}

	public void doDammages(int dammages) {
		health -= dammages;
	}
}
