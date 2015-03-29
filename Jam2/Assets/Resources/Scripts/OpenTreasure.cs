using UnityEngine;
using System.Collections;

public class OpenTreasure : MonoBehaviour {

    public GameObject[] prizes;
    private bool claimed = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag == "Player")
        {
            spawnPrize();

        }
    }

    public void spawnPrize()
    {
        if (!claimed)
        {
            GetComponent<Animator>().SetTrigger("OpenChest");
            claimed = true;
            print("Spawning prize!");
            GameObject prize = Instantiate(prizes[Random.Range(0, prizes.Length)], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            prize.transform.position = new Vector2(transform.position.x, transform.position.y);
            if (prize.name == "GoldStack")
            {
                prize.GetComponent<GoldPickup>().collect(Random.Range(50, 150));
            }
        }
    }
}
