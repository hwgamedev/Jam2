using UnityEngine;
using System.Collections;

public class FigureMovement : MonoBehaviour {

    //sine wave properties - wave length and frequency
    float moveAmplitude = Mathf.PI / 2;
    float vfxAmplitude = Mathf.PI / 16;
    float moveSpeed = 2.0f;
    float vfxSpeed = 8.0f;
    float index;

    //directions
    private int movementX = 0;
    private int movementY = 0;

    //starting points
    private Vector3 startScale;
    private Vector3 startPosition;

    //big old movement bool
    private bool moving = false;

    //template and instance variables for placing movement tokens
    public GameObject tokenTemplate;
    private GameObject moveToken;

    //for the rotatey-two-sidey effect
    private int rotationDir = 1;

	// Use this for initialization
	void Start () {
        startScale = transform.localScale;
        moveToken = Instantiate(tokenTemplate, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

        //identify the player movement token in a special way
        if (gameObject.tag.Equals("Player"))
            moveToken.tag = "PlayerMover";
	}

    void FixedUpdate()
    {
        if (moving)
            moveSine();
    }

    private void moveSine()
    {
        index += Time.fixedDeltaTime;
        float move = moveAmplitude * Mathf.Sin(moveSpeed * index);
        float vfx = vfxAmplitude * Mathf.Sin(vfxSpeed * index);
        if (movementX != 0)
        {
            if (Mathf.Abs(move * movementX) >= 1.0f)
            {
                endMove();
            }
            else
            {
                transform.position = new Vector3(startPosition.x + move * movementX, startPosition.y + vfx, 0);
                transform.Rotate(new Vector3(0, 0, vfx*rotationDir));
            }
        }
        else if (movementY != 0)
        {
            if (Mathf.Abs(move * movementY) >= 1.0f)
            {
                transform.localScale = startScale;
                endMove();
            }
            else
            {
                transform.position = new Vector3(startPosition.x, startPosition.y - move * movementY, 0);
                transform.Rotate(new Vector3(0, 0, vfx * rotationDir));
                transform.localScale = new Vector3(startScale.x + vfx, startScale.y + vfx, 1);
            }
        }
    }

    public void startMove(int x, int y)
    {
        movementX = x;
        movementY = y;
        moving = true;
        startPosition = transform.position;
        moveToken.SetActive(true);
        moveToken.transform.position = new Vector3(startPosition.x + x, startPosition.y - y, 1);
    }

    private void endMove()
    {
        transform.rotation = Quaternion.identity;
        rotationDir *= -1;
        transform.position = new Vector3(startPosition.x + movementX, startPosition.y - movementY, 0);
        stopMove();
    }

    public void stopMove() {
        moving = false;
        index = 0;
        movementY = 0;
        movementX = 0;
        moveToken.SetActive(false);
    }

    public bool isMoving() { return moving; }

    public int getMovementX() { return movementX; }
    public int getMovementY() { return movementY; }
}
