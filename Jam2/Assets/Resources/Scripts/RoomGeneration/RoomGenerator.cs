using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomGenerator : MonoBehaviour {

    public int noRoomsToGenerateHorizontal = 4;
    public int noRoomsToGenerateVertical = 5;

    //room spec
    public int noOfSubrooms = 2;
    public int subRoomsVariation = 2;
    public int minSubRoomSize = 10;
    public int maxSubRoomSize = 15;
    public int maxSizeVariation = 3;
    public int minSizeVariation = 3;

    
    //tiles
    public GameObject tileTemplate;
    public GameObject cornerTileTop;
    public GameObject cornerTileBottom;
    public GameObject topTile;
    public GameObject bottomTile;
    public GameObject sideTile;
    public GameObject wallTile;
    public GameObject doorCornerTop;
    public GameObject doorCornerBottom;

    //doors
    public GameObject doorHorizontal;
    public GameObject doorVertical;

    //room data template
    public GameObject roomDataTemplate;

    //derive grid size
    private int roomSize;

    //this is a grid for a room
    private GameObject[,,] grid;

    //this is the offset of the room on the map
    private Transform roomObject;
    private RoomData data;

    //template for player
    public GameObject player;

    public List<GameObject> rooms;


	// Use this for initialization
    void Start()
    {
        roomSize = (noOfSubrooms+subRoomsVariation) * maxSubRoomSize;
        rooms = new List<GameObject>();
        int roomCounter = 0;
        int roomOffsetX = 0;
        int roomOffsetY = 0;
        for (int i = 0; i < noRoomsToGenerateHorizontal; i++)
        {
            roomOffsetX = (roomSize + maxSubRoomSize) * i;
            for (int j = 0; j < noRoomsToGenerateVertical; j++)
            {
                roomOffsetY = (roomSize + maxSubRoomSize) * j;
                GameObject room = Instantiate(roomDataTemplate, new Vector3(0,0,0), Quaternion.identity) as GameObject;
                room.name = "Room" + roomCounter;
                data = room.GetComponent<RoomData>();
                room.transform.parent = transform;
                Vector3 roomPos = room.transform.localPosition;
                roomPos.x = roomOffsetX;
                roomPos.y = roomOffsetY;
                room.transform.localPosition = roomPos;
                roomObject = room.transform;

                GameObject[, ,] curGrid = generateRoom();
                data.setGrid(curGrid);
                rooms.Add(room);
                data.initObjects();
                roomCounter++;
            }
        }

        int startRoom = Random.Range(0, rooms.Count);
        GameObject playerInstance = Instantiate(player, new Vector3(0,0,0), Quaternion.identity) as GameObject;
        rooms[startRoom].GetComponent<RoomData>().spawnPlayer(playerInstance);
	}
	
	// Update is called once per frame
	void Update () {
	}

    public GameObject[,,] generateRoom()
    {
        //initialise a grid for the room
        grid = new GameObject[roomSize, roomSize, 10];
        int roomsGenerated = 0;

        //spice things up
        int subRoomsToGenerate = noOfSubrooms + Random.Range(0, subRoomsVariation);

        //generate the subrooms
        while (roomsGenerated != subRoomsToGenerate)
        {
            //conjure up a subroom size
            int roomSizeRangeMinX = minSubRoomSize - Random.Range(0, minSizeVariation);
            int roomSizeRangeMinY = minSubRoomSize - Random.Range(0, minSizeVariation);
            int roomSizeRangeMaxX = maxSubRoomSize - Random.Range(0, maxSizeVariation);
            int roomSizeRangeMaxY = maxSubRoomSize - Random.Range(0, maxSizeVariation);

            int roomX = Random.Range(minSubRoomSize, maxSubRoomSize);
            int roomY = Random.Range(minSubRoomSize, maxSubRoomSize);

            int[] offsetsAndSizes = calculateOffset(roomX, roomY, roomsGenerated);

            //generate the subroom based off the specifications
            generateSubRoom(offsetsAndSizes[2],offsetsAndSizes[3],offsetsAndSizes[0],offsetsAndSizes[1]);

            data.addRoomBox(offsetsAndSizes);

            //increment counter
            roomsGenerated++;
        }

        return grid;
    }

    private int[] calculateOffset(int roomX, int roomY, int roomNumber)
    {
        //initialise the offset from 0,0 for where the new subroom would be put
        int offsetX = 0;
        int offsetY = 0;
        bool offsetCorrect = false;

        //keep track of how many times we fail until success
        int failureCount = 0;
        bool offsetTooBig = false;
        //keep trying until successfully placed new subroom
        while (!offsetCorrect)
        {
            //at each attempt, reinit the offsets
            offsetX = 0;
            offsetY = 0;

            //set the chance for the search to move right and down
            float speedX = Random.Range(0.5f, 1f);
            float speedY = Random.Range(0.5f, 1f);
            print("Speeds x: " + speedX + ", y: " + speedY);

            //check if the room would collide with other rooms
            bool hasCollisions = true;
            while (hasCollisions)
            {
                //go through the whole grid area of the new subroom starting at the current offsets
                bool collided = false;
                int x = 0;
                int y = 0;
                for (x = offsetX; x < roomX + offsetX; x++)
                {
                    for (y = offsetY; y < roomY + offsetY; y++)
                    {
                        //sanity check - don't wander off the room boundaries
                        if (x >= roomSize || y >= roomSize)
                        {
                            collided = true;
                            break;
                        }

                        //check if we'd overlap with another room by placing this subroom here
                        if (grid[x, y, 0] != null)
                        {
                            collided = true;
                            break;
                        }
                    }
                    //efficiency optimisation
                    if (collided) break;
                }

                //die if we're out of the room boundaries
                if (x > roomSize || y > roomSize)
                {
                    offsetTooBig = true;
                    break;
                }
                //if collisions detected, move the offsets at their respective speeds
                if (collided)
                {
                    if (Random.Range(0f, 1f) > speedX)
                        offsetX++;
                    if (Random.Range(0f, 1f) > speedY)
                        offsetY++;
                }
                //if no collisions detected, declare victory
                else
                    hasCollisions = false;
            } //end first while

            //retry from beginning if we're out of the room boundaries
            if (offsetTooBig) continue;

            //if it's the first subroom in a room, don't bother with other stuff
            if (offsetX == 0 && offsetY == 0)
            {
                offsetCorrect = true;
                break;
            }

            //otherwise, count if there's enough contact between our new subroom and any of the existing subrooms for a door to be placed
            int counter = 0;
            bool countStarted = false;
            int firstCounterInstance = 0;
            for (int x = offsetX+1; x < offsetX + roomX -1; x++)
            {
                int y = offsetY - 1;
                if (y < 0 || y -1 < 0) break;
                if (grid[x, y, 0] != null && grid[x, y, 0].tag == "Wall" && grid[x, y-1, 0] != null && grid[x, y-1, 0].tag != "Corner")
                {
                    counter++;
                    if (countStarted == false)
                        firstCounterInstance = x;
                    countStarted = true;
                }
                if (grid[x, y, 0] == null && countStarted)
                    break;
            }
            if (counter >= 5)
            {
                int doorPosition = firstCounterInstance + Random.Range(3, counter-2);
                createDoorHorizontal(doorPosition, offsetY-1);
                offsetCorrect = true;
            }

            //do the same on the other axis, since we're not sure where the contact might come from
            counter = 0;
            countStarted = false;
            firstCounterInstance = 0;
            for (int y = offsetY+2; y < offsetY + roomY-1; y++)
            {
                int x = offsetX - 1;

                if (x < 0 || x - 1 < 0) break;

                if (grid[x, y, 0] != null && grid[x,y,0].tag == "Wall" && grid[x-1,y,0] != null && grid[x-1,y,0].tag != "Corner")
                {
                    counter++;
                    if(countStarted == false)
                        firstCounterInstance = y;
                    countStarted = true;
                }
                if (grid[x, y, 0] == null && countStarted)
                    break;
            }
            if (counter >= 5)
            {
                int doorPosition = firstCounterInstance + Random.Range(3, counter-2);
                createDoorVertical(offsetX - 1, doorPosition);
                offsetCorrect = true;
            }

            if (offsetCorrect) break;

            failureCount++;
            print("Failed " + failureCount + " times");
        }

        print("New subroom position. offsets: " + offsetX + ", " + offsetY + ". size: " + roomX + ", " + roomY);

        int[] result = { offsetX, offsetY, roomX, roomY , roomNumber};

        return result;
    }

    private void createDoorVertical(int xPos, int yPos)
    {
        //delete the wall previously here
        Destroy(grid[xPos, yPos, 0]);
        Destroy(grid[xPos, yPos - 1, 0]);
        Destroy(grid[xPos, yPos - 2, 0]);
        Destroy(grid[xPos, yPos + 1, 0]);
        //and create a passage instead
        GameObject temp = Instantiate(tileTemplate, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        //parent passage to parent object
        temp.transform.parent = roomObject;
        temp.transform.localPosition = new Vector3(xPos, (yPos) * -1, 0);
        //add to grid
        grid[xPos, yPos, 0] = temp;
        //wall tile
        temp = Instantiate(wallTile, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        //parent passage to parent object
        temp.transform.parent = roomObject;
        temp.transform.localPosition = new Vector3(xPos, (yPos-1) * -1, 0);
        //add to grid
        grid[xPos, yPos-1, 0] = temp;
        temp = Instantiate(doorCornerTop, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        //parent passage to parent object
        temp.transform.parent = roomObject;
        temp.transform.localPosition = new Vector3(xPos, (yPos - 2) * -1, 0);
        //add to grid
        grid[xPos, yPos - 2, 0] = temp;
        temp = Instantiate(doorCornerBottom, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        //parent passage to parent object
        temp.transform.parent = roomObject;
        temp.transform.localPosition = new Vector3(xPos, (yPos + 1) * -1, 0);
        //add to grid
        grid[xPos, yPos + 1, 0] = temp;

        //add door
        temp = Instantiate(doorVertical, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        //parent passage to parent object
        temp.transform.parent = roomObject;
        temp.transform.localPosition = new Vector3(xPos, (yPos-1.4f) * -1, 0);
        //add to grid
        grid[xPos, yPos-1, 1] = temp;

        //create the passage for the other subroom as well
        temp = Instantiate(tileTemplate, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        //parent it to the room object
        temp.transform.parent = roomObject;
        temp.transform.localPosition = new Vector3(xPos+1, (yPos) * -1, 0);
        //add to grid
        grid[xPos + 1, yPos, 0] = temp;
        temp = Instantiate(wallTile, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        //parent it to the room object
        temp.transform.parent = roomObject;
        temp.transform.localPosition = new Vector3(xPos + 1, (yPos-1) * -1, 0);
        //add to grid
        grid[xPos + 1, yPos-1, 0] = temp;
        temp = Instantiate(doorCornerTop, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        temp.transform.localScale = new Vector3(-1, 1, 0);
        //parent passage to parent object
        temp.transform.parent = roomObject;
        temp.transform.localPosition = new Vector3(xPos + 1, (yPos - 2) * -1, 0);
        //add to grid
        grid[xPos+1, yPos - 2, 0] = temp;
        temp = Instantiate(doorCornerBottom, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        temp.transform.localScale = new Vector3(-1, 1, 0);
        //parent passage to parent object
        temp.transform.parent = roomObject;
        temp.transform.localPosition = new Vector3(xPos + 1, (yPos + 1) * -1, 0);
        //add to grid
        grid[xPos+1, yPos + 1, 0] = temp;
    }

    private void createDoorHorizontal(int xPos, int yPos)
    {
        //delete the wall previously here
        Destroy(grid[xPos, yPos, 0]);
        Destroy(grid[xPos - 1, yPos, 0]);
        Destroy(grid[xPos -2, yPos, 0]);
        Destroy(grid[xPos+1, yPos, 0]);
        //and create a passage instead
        GameObject temp = Instantiate(tileTemplate, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        //parent passage to parent object
        temp.transform.parent = roomObject;
        temp.transform.localPosition = new Vector3(xPos, (yPos) * -1, 0);
        //add to grid
        grid[xPos, yPos, 0] = temp;
        temp = Instantiate(tileTemplate, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        //parent passage to parent object
        temp.transform.parent = roomObject;
        temp.transform.localPosition = new Vector3(xPos-1, (yPos) * -1, 0);
        //add to grid
        grid[xPos-1, yPos, 0] = temp;
        //wall tile
        temp = Instantiate(doorCornerBottom, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        temp.transform.localScale = new Vector3(-1, 1, 0);
        //parent passage to parent object
        temp.transform.parent = roomObject;
        temp.transform.localPosition = new Vector3(xPos-2, (yPos) * -1, 0);
        //add to grid
        grid[xPos-2, yPos, 0] = temp;
        temp = Instantiate(doorCornerBottom, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        //parent passage to parent object
        temp.transform.parent = roomObject;
        temp.transform.localPosition = new Vector3(xPos+1, (yPos) * -1, 0);
        //add to grid
        grid[xPos+1, yPos, 0] = temp;

        temp = Instantiate(tileTemplate, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        //parent passage to parent object
        temp.transform.parent = roomObject;
        temp.transform.localPosition = new Vector3(xPos, (yPos+1) * -1, 0);
        //add to grid
        grid[xPos, yPos+1, 0] = temp;
        temp = Instantiate(tileTemplate, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        //parent passage to parent object
        temp.transform.parent = roomObject;
        temp.transform.localPosition = new Vector3(xPos-1, (yPos + 1) * -1, 0);
        //add to grid
        grid[xPos-1, yPos + 1, 0] = temp;
        //wall tile
        temp = Instantiate(doorCornerTop, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        temp.transform.localScale = new Vector3(-1, 1, 0);
        //parent passage to parent object
        temp.transform.parent = roomObject;
        temp.transform.localPosition = new Vector3(xPos - 2, (yPos+1) * -1, 0);
        //add to grid
        grid[xPos-2, yPos + 1, 0] = temp;
        temp = Instantiate(doorCornerTop, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        //parent passage to parent object
        temp.transform.parent = roomObject;
        temp.transform.localPosition = new Vector3(xPos + 1, (yPos+1) * -1, 0);
        //add to grid
        grid[xPos+1, yPos +1, 0] = temp;

        temp = Instantiate(tileTemplate, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        //parent passage to parent object
        temp.transform.parent = roomObject;
        temp.transform.localPosition = new Vector3(xPos, (yPos + 2) * -1, 0);
        //add to grid
        grid[xPos, yPos+2, 0] = temp;
        temp = Instantiate(tileTemplate, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        //parent passage to parent object
        temp.transform.parent = roomObject;
        temp.transform.localPosition = new Vector3(xPos-1, (yPos + 2) * -1, 0);
        //add to grid
        grid[xPos-1, yPos + 2, 0] = temp;

        //add door
        temp = Instantiate(doorHorizontal, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        //parent it to the room object
        temp.transform.parent = roomObject;
        temp.transform.localPosition = new Vector3(xPos-1.5f, (yPos) * -1, 0);
        //add to grid
        grid[xPos-1, yPos, 1] = temp;
        
    }

    private void generateSubRoom(int roomX, int roomY, int offsetX, int offsetY)
    {
        //generate the corner tiles
        genCornerTile(cornerTileTop, offsetX, offsetY, 1, 1);
        genCornerTile(cornerTileTop, roomX-1 + offsetX, offsetY, -1, 1);
        genCornerTile(cornerTileBottom, roomX-1 + offsetX, roomY-1 + offsetY, -1, 1);
        genCornerTile(cornerTileBottom, offsetX, roomY-1 + offsetY, 1, 1);

        //generate walls
        genWall(topTile, roomX-1, offsetX, offsetY, true, false);
        genWall(wallTile, roomX - 1, offsetX, offsetY + 1, true, false);
        genWall(bottomTile, roomX-1, offsetX, roomY-1 + offsetY, true, true);
        genWall(sideTile, roomY-1, offsetX, offsetY, false, false);
        genWall(sideTile, roomY-1, offsetX + roomX-1, offsetY, false, true);

        //fill the middle bit
        for (int x = 1; x < roomX-1; x++)
        {
            for (int y = 2; y < roomY-1; y++)
            {
                int xPos = x + offsetX;
                int yPos = y + offsetY;
                createTile(tileTemplate, xPos, yPos);
            }
        }
    }

    private GameObject createTile(GameObject template, int xPos, int yPos)
    {
        if (grid[xPos, yPos, 0] != null)
            return null;
        GameObject temp = Instantiate(template, new Vector3(0,0, 0), Quaternion.identity) as GameObject;
        temp.transform.parent = roomObject.transform;
        temp.transform.localPosition = new Vector3(xPos, (yPos) * -1, 0);
        grid[xPos, yPos, 0] = temp;
        return temp;
    }

    private void genCornerTile(GameObject cornerTile, int xPos, int yPos, int xScale, int yScale)
    {
        GameObject temp = createTile(cornerTile, xPos, yPos);
        if (temp == null)
        {
            Debug.Log("Couldn't create corner tile!");
        }
        Vector3 theScale = temp.transform.localScale;
        theScale.x *= xScale;
        theScale.y *= yScale;
        temp.transform.localScale = theScale;
    }

    private void genWall(GameObject tile, int noOfTiles, int xPos, int yPos, bool xAxis, bool flip)
    {
        for (int i = 1; i < noOfTiles; i++)
        {
            GameObject temp;
            if (xAxis)
            {
                temp = createTile(tile, xPos + i, yPos);
            }
            else
            {
                temp = createTile(tile, xPos, yPos + i);
            }
            if (flip && temp != null && !xAxis)
            {
                Vector3 theScale = temp.transform.localScale;
               // if (xAxis)
               //     theScale.y *= -1;
               // else
                    theScale.x *= -1;
                temp.transform.localScale = theScale;
            }
        }
    }
}
