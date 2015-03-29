using UnityEngine;
using System.Collections;

public class Dagger : MonoBehaviour {

	// movement helpers
	private float startTime;
	private float journeyLength;
	private Vector3 startPosition;
	private Vector3 endPosition;

	private float speed = 12.0f;

	private bool initiated = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (initiated) {
			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / journeyLength;
			if(!checkCollision(transform.position, Vector3.Lerp (startPosition, endPosition, fracJourney))){
				transform.position = Vector3.Lerp (startPosition, endPosition, fracJourney);
				transform.Rotate(0f, 0f, 20f);
				if (transform.position == endPosition) {
					Destroy (gameObject);
				}
			} else {
				Destroy (gameObject);
			}
		}
	}

	public void throwDagger(Vector3 start, Vector3 end){
		transform.position = start;
		startTime = Time.time;
		startPosition = start;
		endPosition = end;
		journeyLength = Vector3.Distance (startPosition, endPosition);
		initiated = true;
	}

	private bool checkCollision(Vector3 start, Vector3 end) {
		RaycastHit2D hit = Physics2D.Linecast(start, end);
		if (hit && !hit.collider.isTrigger)
		{
			print("Colliding with: "+hit.collider.gameObject.name);
			if(hit.collider.gameObject.CompareTag("Enemy"){
				hit.collider.GetComponent<EnemyBase>().takeDamage(Player.Instance.getDamage());
			}
			return true;
		}
		
		return false;
	}
}
