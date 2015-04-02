using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomData : MonoBehaviour {

    public GameObject fogOfWar;

    private System.Collections.Generic.List<int[]> roomBoxes = new System.Collections.Generic.List<int[]>();
    private GameObject[, ,] grid;

    public int[] spawnPoint = new int[2];

    private bool[] playerVisibleRooms;
    private int roomSize;

    public GameObject[] obstacles;
    public GameObject goldTemplate;
    public GameObject treasureTemplate;
    public GameObject[] enemies;
    public GameObject[] fogOfWars;
    public GameObject[] accents;

    private int enemyCount = 0;
    private List<GameObject> enemyArray;
    private float decrement;
    private bool setupComplete = false;

    //variable obstacle/reward densities
    public float sparseObjects = Random.Range(0.05f, 0.10f);
    public float clumps = Random.Range(0.05f, 0.08f);
    public float treasure = 0.01f;
    public float coins = Random.Range(0.02f, 0.10f);

    //enemy spawn formation chances
    public float singleEnemy = Random.Range(0.2f, 0.3f);
    public float enemyLine = Random.Range(0.05f, 0.1f);
    public float cornerFormation = Random.Range(0.05f, 0.1f);

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

    public void initObjects(int size)
    {
        playerVisibleRooms = new bool[roomBoxes.Count];

        roomSize = size;

        initSpawn();
        initFogOfWar();
        initObstacles();
        initEnemies();
        putOnAccents();

        setupComplete = true;
    }

    public void putOnAccents()
    {
        for (int z = 0; z < roomBoxes.Count; z++)
        {
            int[] roomBox = roomBoxes[z];
            int minX = roomBox[0] + 1;
            int maxX = roomBox[0] + roomBox[2] - 1;
            int y = roomBox[1] +1;
            int wallLength = maxX - minX;
            //pick an accent from a list
            int accentIndex = Random.Range(0,accents.Length);
            if (grid[minX+2, y, 0].tag != "Floor")
            {
                //TODO: put on the selected accent
                GameObject temp = Instantiate(accents[accentIndex], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                temp.transform.parent = transform;
                temp.transform.localPosition = new Vector3(minX+2, y * -1);
            }

            if (grid[maxX-3, y, 0].tag != "Floor")
            {
                //TODO: put on the selected accent
                GameObject temp = Instantiate(accents[accentIndex], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                temp.transform.parent = transform;
                temp.transform.localPosition = new Vector3(maxX-3, y * -1);
            }
        }
    }

    public void initObstacles()
    {
        for (int z = 0; z < roomBoxes.Count; z++)
        {
            int[] roomBox = roomBoxes[z];
            int minX = roomBox[0] + 1;
            int maxX = roomBox[0]+roomBox[2]-1;
            int minY = roomBox[1]+2;
            int maxY = roomBox[1] + roomBox[3] - 1;

            for (int x = minX; x < maxX; x++)
            {
                for (int y = minY; y < maxY; y++)
                {
                    //avoid doorways
                    if((x == minX && grid[x-1,y,0].tag == "Floor") ||
                        (x == maxX && grid[x+1,y,0].tag == "Floor") ||
                        (y == minY && grid[x,y-1,0].tag == "Floor") ||
                        (y == maxY && grid[x,y+1,0].tag == "Floor"))
                        continue;

                    //if it's not a doorway, try to place some clutter
                    if (grid[x, y, 0] != null && grid[x, y, 0].tag == "Floor" && grid[x, y, 1] == null)
                    {
                        if (Random.Range(0f, 1f) < sparseObjects)
                        {
                            GameObject temp = Instantiate(obstacles[Random.Range(0, obstacles.Length)], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                            temp.transform.parent = transform;
                            temp.transform.localPosition = new Vector3(x, y * -1);
                            grid[x, y, 1] = temp;
                        }
                        else if (Random.Range(0f, 1f) < clumps)
                        {
                            for (int i = x; i < x + Random.Range(0,3); i++)
                            {
                                for (int j = y; j < y + Random.Range(0,3); j++)
                                {
                                    if (i >= maxX || j >= maxY)
                                        continue;
                                    if (grid[i, j, 1] == null)
                                    {
                                        GameObject temp = Instantiate(obstacles[Random.Range(0, obstacles.Length)], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                                        temp.transform.parent = transform;
                                        temp.transform.localPosition = new Vector3(i, j * -1);
                                        grid[i, j, 1] = temp;
                                    }
                                }
                            }

                        }
                        else if (Random.Range(0f, 1f) < treasure)
                        {
                            GameObject temp = Instantiate(treasureTemplate, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                            temp.transform.parent = transform;
                            temp.transform.localPosition = new Vector3(x, y * -1);
                            grid[x, y, 1] = temp;
                        }
                        else if (Random.Range(0f, 1f) < coins)
                        {
                            GameObject temp = Instantiate(goldTemplate, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                            temp.transform.parent = transform;
                            temp.transform.localPosition = new Vector3(x, y * -1);
                            grid[x, y, 1] = temp;
                        }
                    }
                }
            }

        }
        

        foreach(Transform child in transform) {
            if(child.tag == "Door") {
                int xPos = (int)child.localPosition.x;
                int yPos = (int)child.localPosition.y;

                cleanupDoor(xPos, yPos*-1);
            }
        }
    }

    private void initEnemies()
    {
        enemyCount = 0;
        enemyArray = new List<GameObject>();
        
        for (int z = 0; z < roomBoxes.Count; z++)
        {
            int[] roomBox = roomBoxes[z];
            int minX = roomBox[0] + 1;
            int maxX = roomBox[0] + roomBox[2] - 1;
            int minY = roomBox[1] + 2;
            int maxY = roomBox[1] + roomBox[3] - 1;
            int mandatoryX = (roomBox[0]+((maxX - minX) / 2));
            int mandatoryY = (roomBox[1]+((maxY - minY)) / 2);
            //print("Mandatory enemy x: " + mandatoryX + " y: " + mandatoryY + " minX: " + minX + ",maxX: " + maxX + ", minY: " + minY + ", maxY: " + maxY);


            for (int x = minX; x < maxX; x++)
            {
                for (int y = minY; y < maxY; y++)
                {
                    //print("x: " + x + " y: " + y);
                    //mandatory enemy in centre of each room
                    if (x == mandatoryX && y == mandatoryY)
                    {
                        //Debug.Log("mandatory spawn!");
                        //Debug.Log("Love me!");
                        if (grid[x, y, 1] != null && grid[x, y, 1].tag == "PlayerSpawn")
                        {
                            x += 1;
                        }
                        if (grid[x, y, 1] != null)
                            Destroy(grid[x, y, 1]);

                        GameObject temp = Instantiate(enemies[Random.Range(0, enemies.Length)], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                        temp.transform.parent = transform;
                        temp.transform.localPosition = new Vector3(x, y * -1);
                        addEnemy(temp);
                        continue;
                    }

                    //avoid doorways
                    if ((x == minX && grid[x - 1, y, 0].tag == "Floor") ||
                        (x == maxX && grid[x + 1, y, 0].tag == "Floor") ||
                        (y == minY && grid[x, y - 1, 0].tag == "Floor") ||
                        (y == maxY && grid[x, y + 1, 0].tag == "Floor"))
                        continue;


                    //if it's not a doorway, try to place a line of enemies
                    if (grid[x, y, 0] != null && grid[x, y, 0].tag == "Floor" && grid[x, y, 1] == null)
                    {
                        if (Random.Range(0f, 1f) < enemyLine)
                        {
                            for (int i = x; i < x + Random.Range(0, 3); i++)
                            {
                                    if (i >= maxX)
                                        break;
                                    if (grid[i, y, 1] == null)
                                    {
                                        GameObject temp = Instantiate(enemies[Random.Range(0, enemies.Length)], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                                        temp.transform.parent = transform;
                                        temp.transform.localPosition = new Vector3(x, y * -1);
                                        addEnemy(temp);
                                    }
                            }
                        }
                        else if (Random.Range(0f, 1f) < singleEnemy)
                        {
                            GameObject temp = Instantiate(enemies[Random.Range(0, enemies.Length)], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                            temp.transform.parent = transform;
                            temp.transform.localPosition = new Vector3(x, y * -1);
                            addEnemy(temp);
                        }
                    }
                }
            }

            if (Random.Range(0f, 1f) < cornerFormation)
            {
                if (grid[minX, minX, 0] != null && grid[minX, minY, 0].tag == "Floor" && grid[minX, minY, 1] == null)
                {
                    GameObject temp = Instantiate(enemies[Random.Range(0, enemies.Length)], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                    temp.transform.parent = transform;
                    temp.transform.localPosition = new Vector3(minX, minY * -1);
                    addEnemy(temp);
                }

                if (grid[maxX, minY, 0] != null && grid[maxX, minY, 0].tag == "Floor" && grid[maxX, minY, 1] == null)
                {
                    GameObject temp = Instantiate(enemies[Random.Range(0, enemies.Length)], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                    temp.transform.parent = transform;
                    temp.transform.localPosition = new Vector3(maxX, minY * -1);
                    addEnemy(temp);
                }

                if (grid[minX, maxY, 0] != null && grid[minX, maxY, 0].tag == "Floor" && grid[minX, maxY, 1] == null)
                {
                    GameObject temp = Instantiate(enemies[Random.Range(0, enemies.Length)], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                    temp.transform.parent = transform;
                    temp.transform.localPosition = new Vector3(minX, maxY * -1);
                    addEnemy(temp);
                }

                if (grid[maxX, maxY, 0] != null && grid[maxX, maxY, 0].tag == "Floor" && grid[maxX, maxY, 1] == null)
                {
                    GameObject temp = Instantiate(enemies[Random.Range(0, enemies.Length)], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                    temp.transform.parent = transform;
                    temp.transform.localPosition = new Vector3(maxX, maxY * -1);
                    addEnemy(temp);
                }
            }


        }

        //print(gameObject.name+" EnemyCount: " + enemyCount+ " NoSubrooms: "+roomBoxes.Count);
        decrement = .4f / (enemyCount);
        //print("decrement: " + decrement);
        
        
    }

    private void addEnemy(GameObject temp)
    {
        enemyArray.Add(temp);
        enemyCount++;
        temp.GetComponent<EnemyBase>().setRoom(gameObject.name);
    }

    private void cleanupDoor(int xPos, int yPos)
    {
        //print("Door at x: " + xPos + ", y: " + yPos);
        for (int i = xPos - 3; i < xPos + 4; i++)
        {
            for (int j = yPos - 3; j < yPos + 4; j++)
            {
                if (i < roomSize && i < roomSize && i >= 0 && j >= 0 && grid[i, j, 1] != null && grid[i, j, 1].tag == "Obstacle")
                {
                    Destroy(grid[i, j, 1]);
                }
            }
        }
    }

    private void initFogOfWar()
    {
        fogOfWars = new GameObject[roomBoxes.Count];
        for (int i = 0; i < roomBoxes.Count; i++)
        {
            int spawnX = roomBoxes[i][0];
            int spawnY = roomBoxes[i][1];
            GameObject go = Instantiate(fogOfWar,new Vector3(0,0,0), Quaternion.identity) as GameObject;
            go.transform.parent = transform;
            go.transform.localPosition = new Vector3(spawnX+.5f+.25f, spawnY*-1-.5f-.25f);
            go.transform.localScale = new Vector3(roomBoxes[i][2]-2.5f, roomBoxes[i][3]-2.5f, 0);
            fogOfWars[i] = go;
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
    }

    public void spawnPlayer(GameObject player, GameObject camera)
    {
		//print (player.transform.position);
        player.transform.parent = transform;
        camera.transform.parent = transform;
		//print (player.transform.position);
        player.transform.localPosition = new Vector3(spawnPoint[0], spawnPoint[1]*-1, 0);
        camera.transform.localPosition = new Vector3(spawnPoint[0], spawnPoint[1] * -1, -10);
		//print (player.transform.position);
        Player.Instance.setCurrentRoom(gameObject.name);
        Player.Instance.setRoomEnemies(enemyArray.Count);

        for (int i = 0; i < roomBoxes.Count; i++)
        {
            fogOfWars[i].GetComponent<FogOfWar>().initialise();

        }

        for (int i = 0; i < enemyArray.Count; i++)
        {
            enemyArray[i].SetActive(true);
        }
    }

    public void removePlayer()
    {
        for (int i = 0; i < enemyArray.Count; i++)
        {
            enemyArray[i].SetActive(false);
        }
    }

    public int getRoomSize()
    {
        return roomSize;
    }

    public void setEnemyAsDead(GameObject enemy)
    {
        enemyArray.Remove(enemy);
        Player.Instance.killRoomEnemy();
        foreach(GameObject fow in fogOfWars) {
            fow.GetComponent<FogOfWar>().changeAlpha(decrement);
        }
    }
}
