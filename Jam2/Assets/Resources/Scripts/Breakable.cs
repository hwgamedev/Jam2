using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Breakable : MonoBehaviour {
    public List<GameObject> drop = new List<GameObject>();
    [Range(0,10)]
    public int dropRate;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void attacked()
    {
        if (Random.Range(0, 10) <= dropRate)
        {
            dropItem();
        }
        Destroy(gameObject);
    }

    public void dropItem()
    {
        GameObject item = Instantiate(drop[Random.Range(0, drop.Count)]);
        item.transform.position = transform.position;
    }
}
