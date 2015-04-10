using UnityEngine;
using System.Collections;

public class Collected : MonoBehaviour {

	bool activate = false;
    float timer = 1f;
    float scaleFactor = .02f;
	//if item should shrink or increase scale
	public int scaleModifier = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
        if(activate) {
			Destroy(GetComponent("BoxCollider2D"));
            timer -= Time.deltaTime;
            if (timer < 0)
                Destroy(gameObject);
            else
            {
				transform.localScale = new Vector2(transform.localScale.x + (scaleModifier * scaleFactor), transform.localScale.y + (scaleModifier *scaleFactor));
            }
        }
        
	}

    public void playPickup() {
        if (!activate)
        {
            activate = true;
            timer = 1f;
        }
    }
}
