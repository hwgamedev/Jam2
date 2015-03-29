using UnityEngine;
using System.Collections;

public class SwitchActive : MonoBehaviour {

    Transform openState;
    Transform closedState;

    bool interference = false;
    bool state = true;

	// Use this for initialization
	void Start () {
        openState = transform.GetChild(0);
        closedState = transform.GetChild(1);

        openState.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay2D(Collider2D col) {
        interference = true;
    }

    void OnTriggerExit2D(Collider2D col) {
        interference = false;
    }

    void OnMouseDown()
    {
        if (!interference)
        {
            Collider2D col = GetComponent<Collider2D>();
            col.isTrigger = state;
            closedState.gameObject.SetActive(!state);
            openState.gameObject.SetActive(state);
            state = !state;
        }
    }
}
