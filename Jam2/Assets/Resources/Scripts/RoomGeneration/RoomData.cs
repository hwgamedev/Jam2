using UnityEngine;
using System.Collections;

public class RoomData : MonoBehaviour {

    public GameObject fogOfWar;

    private System.Collections.Generic.List<int[]> roomBoxes = new System.Collections.Generic.List<int[]>();
    private GameObject[, ,] grid;

    public int[] spawnPoint = new int[2];

    private bool[] playerVisibleRooms;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void addRoomBox(int[] boxSpec)
    {
        roomBoxes.Add(boxSpec);
    }

    public void setGrid(GameObject[, ,] grid)
    {
        this.grid = grid;
    }

    public GameObject[, ,] getGrid() { return grid; }

    public void initObjects()
    {
        playerVisibleRooms = new bool[roomBoxes.Count];

        initSpawn();
    }

    private void initFogOfWar()
    {
        for (int i = 0; i < playerVisibleRooms.Length; i++)
        {
            int spawnX = roomBoxes[i][0];
            int spawnY = roomBoxes[i][1];
            GameObject go = Instantiate(fogOfWar,new Vector3(0,0,0), Quaternion.identity) as GameObject;
            go.transform.parent = transform;
            go.transform.localPosition = new Vector3(spawnX+.5f+.25f, spawnY*-1-.5f-.25f);
            go.transform.localScale = new Vector3(roomBoxes[i][2]-2.5f, roomBoxes[i][3]-2.5f, 0);
        }
    }

    private void initSpawn()
    {
        int spawnRoom = Random.Range(0, roomBoxes.Count);
        int[] roomSpec = roomBoxes[spawnRoom];
        int spawnX = Random.Range(roomSpec[0] + 1, roomSpec[0] + roomSpec[2] - 1);
        int spawnY = Random.Range(roomSpec[1] + 2, roomSpec[1] + roomSpec[3] - 1);
        GameObject playerSpawn = new GameObject("PlayerSpawn");
        playerSpawn.tag = "PlayerSpawn";
        playerSpawn.transform.parent = transform;
        grid[spawnX, spawnY, 1] = playerSpawn;
        spawnPoint[0] = spawnX;
        spawnPoint[1] = spawnY;
        playerVisibleRooms[spawnRoom] = true;
        initFogOfWar();
    }

    public void spawnPlayer(GameObject player)
    {
        player.transform.parent = transform;
        player.transform.localPosition = new Vector3(spawnPoint[0], spawnPoint[1]*-1, 0);
    }
}
