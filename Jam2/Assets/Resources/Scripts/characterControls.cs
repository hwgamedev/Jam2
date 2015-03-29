using UnityEngine;
using System.Collections;

public class characterControls : MonoBehaviour {

	//public GameObject character;

	private Animator anim;
	public float speed = 1.0f;
	private float startTime;
	private float journeyLength;
	private Vector3 startPosition;
	private Vector3 endPosition;
	private bool moving = false;
	private bool attacking = false;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!moving && !attacking) {
			if (Input.GetKey (KeyCode.W)) {
				anim.SetTrigger("iddleN");
				startTime = Time.time;
				startPosition = transform.position;
				endPosition = startPosition;
				endPosition += new Vector3 (0, 1, 0);
				journeyLength = Vector3.Distance (startPosition, endPosition);
				moving = true;
			}
			if (Input.GetKey (KeyCode.A)) {
				anim.SetTrigger("iddleE");
				startTime = Time.time;
				startPosition = transform.position;
				endPosition = startPosition;
				endPosition -= new Vector3 (1, 0, 0);
				journeyLength = Vector3.Distance (startPosition, endPosition);
				moving = true;
			}
			if (Input.GetKey (KeyCode.S)) {
				anim.SetTrigger("iddleS");
				startTime = Time.time;
				startPosition = transform.position;
				endPosition = startPosition;
				endPosition -= new Vector3 (0, 1, 0);
				journeyLength = Vector3.Distance (startPosition, endPosition);
				moving = true;
			}
			if (Input.GetKey (KeyCode.D)) {
				anim.SetTrigger("iddleW");
				startTime = Time.time;
				startPosition = transform.position;
				endPosition = startPosition;
				endPosition += new Vector3 (1, 0, 0);
				journeyLength = Vector3.Distance (startPosition, endPosition);
				moving = true;
			}
			if(Input.GetMouseButtonDown(0)){
				Vector3 mousePoint = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
				float diffX = mousePoint.x - transform.position.x;
				float diffY = mousePoint.y - transform.position.y;
				if(diffX > -0.7 && diffX < 0.7){
					if(diffY > 1 && diffY < 2){
						anim.SetTrigger("attackN");
						attacking = true;
					}
					else if(diffY < 0 && diffY > -1){
						anim.SetTrigger("attackS");
						attacking = true;
					}
				}
				else if(diffY < 1 && diffY > 0){
					if(diffX < -0.7 && diffX > -1.7){
						anim.SetTrigger("attackE");
						attacking = true;
					}
					else if(diffX > 0.7 && diffX < 1.7){
						anim.SetTrigger("attackW");
						attacking = true;
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

	private void doLerp() {
		float distCovered = (Time.time - startTime) * speed;
		float fracJourney = distCovered / journeyLength;
		transform.position = Vector3.Lerp (startPosition, endPosition, fracJourney);
		if (transform.position == endPosition) {
			moving = false;
		}
	}

	private void doAttack(){
		attacking = false;
	}
}
