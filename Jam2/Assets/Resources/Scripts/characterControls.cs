using UnityEngine;
using System.Collections;

public class characterControls : MonoBehaviour {

	//public GameObject character;

	public float speed = 1.0f;
	private float startTime;
	private float journeyLength;
	private Vector3 startPosition;
	private Vector3 endPosition;
	private bool moving = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!moving) {
			if (Input.GetKey (KeyCode.W)) {
				startTime = Time.time;
				startPosition = transform.position;
				endPosition = startPosition;
				endPosition += new Vector3 (0, 1, 0);
				journeyLength = Vector3.Distance (startPosition, endPosition);
				moving = true;
			}
			if (Input.GetKey (KeyCode.A)) {
				startTime = Time.time;
				startPosition = transform.position;
				endPosition = startPosition;
				endPosition -= new Vector3 (1, 0, 0);
				journeyLength = Vector3.Distance (startPosition, endPosition);
				moving = true;
			}
			if (Input.GetKey (KeyCode.S)) {
				startTime = Time.time;
				startPosition = transform.position;
				endPosition = startPosition;
				endPosition -= new Vector3 (0, 1, 0);
				journeyLength = Vector3.Distance (startPosition, endPosition);
				moving = true;
			}
			if (Input.GetKey (KeyCode.D)) {
				startTime = Time.time;
				startPosition = transform.position;
				endPosition = startPosition;
				endPosition += new Vector3 (1, 0, 0);
				journeyLength = Vector3.Distance (startPosition, endPosition);
				moving = true;
			}
		}
		if (moving) {
			doLerp ();
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
}
