using UnityEngine;
using System.Collections;

public class FigureMovement : MonoBehaviour {

    float amplitudeX = Mathf.PI / 2;
    float amplitudeY = Mathf.PI / 8;
    float omegaX = 2.0f;
    float omegaY = 8.0f;
    float index;

    private int movementX = 0;
    private int movementY = 0;
    private Vector3 startScale;
    private Vector3 startPosition;

    public bool moving = false;
    public GameObject tokenTemplate;
    private GameObject moveToken;

    private int rotationDir = 1;

	// Use this for initialization
	void Start () {
        startScale = transform.localScale;
        moveToken = Instantiate(tokenTemplate, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
	}

    public void moveSine()
    {
        index += Time.fixedDeltaTime;
        if (movementX != 0)
        {
            float x = amplitudeX * Mathf.Sin(omegaX * index);
            float y = amplitudeY * Mathf.Sin(omegaY * index);
            if (Mathf.Abs(x * movementX) >= 1.0f)
            {
                transform.rotation = Quaternion.identity;
                rotationDir *= -1;
                endMove();
            }
            else
            {
                transform.position = new Vector3(startPosition.x + x * movementX, startPosition.y + y, 0);
                transform.Rotate(new Vector3(0, 0, y*rotationDir));
            }
        }
        else if (movementY != 0)
        {
            float y = amplitudeX * Mathf.Sin(omegaX * index);
            float x = amplitudeY * Mathf.Sin(omegaY * index);
            if (Mathf.Abs(y * movementY) >= 1.0f)
            {
                transform.localScale = startScale;
                transform.rotation = Quaternion.identity;
                rotationDir *= -1;
                endMove();
            }
            else
            {
                transform.position = new Vector3(startPosition.x, startPosition.y - y * movementY, 0);
                transform.Rotate(new Vector3(0, 0, x * rotationDir));
                transform.localScale = new Vector3(startScale.x + x, startScale.y + x, 1);
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

    public int getMovementX() { return movementX; }
    public int getMovementY() { return movementY; }
}
