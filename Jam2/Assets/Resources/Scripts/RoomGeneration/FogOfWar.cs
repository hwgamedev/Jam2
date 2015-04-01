using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
            if (newVal <= newAlpha)
                changedAlpha = false;

        }
	}

    public void changeAlpha(float nAlpha)
    {
        newAlpha = newAlpha - nAlpha;
        //print("Newalpha: " + newAlpha);
        changedAlpha = true;
    }

    public void initialise()
    {
        //print("triggered!");
        if (initialCleansing == false)
        {
            //print("Cleansing!");
            initialCleansing = true;
            changedAlpha = true;
            SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
            newAlpha = sr.color.a * .3f;
        }
    }
}
