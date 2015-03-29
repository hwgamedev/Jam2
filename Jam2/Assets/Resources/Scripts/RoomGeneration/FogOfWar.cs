using UnityEngine;
using System.Collections;

public class FogOfWar : MonoBehaviour {

    bool initialCleansing = false;
    float newAlpha;
    bool changedAlpha = false;

	// Use this for initialization
	void Start () {
	   
	}
	
	// Update is called once per frame
	void Update () {
        if (changedAlpha)
        {
            SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
            float newVal = Mathf.Lerp(sr.color.a, newAlpha, Time.deltaTime);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, newVal);
            if (newVal == newAlpha)
                changedAlpha = false;

        }
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        print("triggered!");
        if (initialCleansing == false && col.tag == "Player")
        {
            print("Cleansing!");
            initialCleansing = true;
            changedAlpha = true;
            SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
            newAlpha = sr.color.a * .3f;
        }
    }
}
