using UnityEngine;
using System.Collections;

public class RoomGenerator : MonoBehaviour {

    //room spec
    public int noOfSubrooms = 1;
    public int minSubRoomSize = 5;
    public int maxSubRoomSize = 10;
    
    //tiles
    public GameObject tileTemplate;
    public GameObject cornerTile;
    public GameObject topTile;
    public GameObject sideTile;

    //derive grid size
    private int roomSize;

    //this is a grid for a room
    private GameObject[,,] grid;

    //this is the offset of the room on the map
    private int roomOffsetX;
    private int roomOffsetY;

    //this marks the full bounding box of a single room
    private int xStart, xEnd, yStart, yEnd;


	// Use this for initialization
    void Start()
    {
        roomSize = noOfSubrooms * maxSubRoomSize;
        roomOffsetX = roomOffsetY = 0;
        generateRoom();
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void generateRoom()
    {
        //initialise a grid for the room
        grid = new GameObject[roomSize, roomSize, 10];
        int roomsGenerated = 0;

        //generate the subrooms
        while (roomsGenerated != noOfSubrooms)
        {
            //conjure up a subroom size
            int roomX = Random.Range(minSubRoomSize, maxSubRoomSize);
            int roomY = Random.Range(minSubRoomSize, maxSubRoomSize);

            int[] offsetsAndSizes = calculateOffset(roomX, roomY);

            //generate the subroom based off the specifications
            generateSubRoom(offsetsAndSizes[2],offsetsAndSizes[3],offsetsAndSizes[0],offsetsAndSizes[1]);

            //increment counter
            roomsGenerated++;
        }
    }

    private int[] calculateOffset(int roomX, int roomY)
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
            for (int x = offsetX+1; x < offsetX + roomX -1; x++)
            {
                int y = offsetY - 1;
                if (y < 0) break;
                if (grid[x, y, 0] != null && grid[x, y, 0].tag == "Wall")
                {
                    counter++;
                    countStarted = true;
                }
                if (grid[x, y, 0] == null && countStarted)
                    break;
            }
            if (counter >= 1)
            {
                offsetCorrect = true;
            }

            //do the same on the other axis, since we're not sure where the contact might come from
            counter = 0; 
            countStarted = false;
            for (int y = offsetY+1; y < offsetY + roomY-1; y++)
            {
                int x = offsetX - 1;

                if (x < 0) break;

                if (grid[x, y, 0] != null && grid[x,y,0].tag == "Wall")
                {
                    counter++;
                    countStarted = true;
                }
                if (grid[x, y, 0] == null && countStarted)
                    break;
            }
            if (counter >= 1)
            {
                offsetCorrect = true;
            }

            if (offsetCorrect) break;

            failureCount++;
            print("Failed " + failureCount + " times");
        }

        print("New subroom position. offsets: " + offsetX + ", " + offsetY + ". size: " + roomX + ", " + roomY);

        int[] result = { offsetX, offsetY, roomX, roomY };

        return result;
    }

    private void createDoor(int xPos, int yPos, int xAdjacent, int yAdjacent)
    {
        Destroy(grid[xPos,yPos,0]);
        GameObject temp = Instantiate(tileTemplate, new Vector3(xPos + roomOffsetX, (yPos + roomOffsetY) * -1, 0), Quaternion.identity) as GameObject;
        grid[xPos, yPos, 0] = temp;
        temp = Instantiate(tileTemplate, new Vector3(xPos + xAdjacent + roomOffsetX, (yPos + yAdjacent + roomOffsetY) * -1, 0), Quaternion.identity) as GameObject;
        grid[xPos+xAdjacent, yPos+yAdjacent, 0] = temp;
    }

    private int[] fitRoom(int xSubOffset, int ySubOffset, int xSpaceNeeded, int ySpaceNeeded)
    {
        //we'll be checking if we can fit the subroom of specified size where we want
        int xSpaceGiven = xSpaceNeeded;
        int ySpaceGiven = ySpaceNeeded;

        //check if there's enough space there to generate the subroom of the given size
        for (int xSpace = 0; xSpace < xSpaceNeeded; xSpace++)
        {
            //if there's not enough space on the x axis, reduce subroom width
            if (xSubOffset + xSpace == roomSize - 1)
            {
                xSpaceGiven = xSpace - 1;
                break;
            }
            else if (grid[(xSubOffset + xSpace), ySubOffset, 0] != null)
            {
                xSpaceGiven = xSpace - 1;
                break;
            }
        }
        //check if there's enough space there to generate the subroom of the given size
        for (int ySpace = 0; ySpace < ySpaceNeeded; ySpace++)
        {
            //if there's not enough space on the y axis, reduce the subroom height
            if (xSubOffset + ySpace == roomSize - 1)
            {
                xSpaceGiven = ySpace - 1;
                break;
            }
            else if (grid[xSubOffset, (ySubOffset + ySpace), 0] != null)
            {
                ySpaceGiven = ySpace - 1;
                break;
            }
        }

        //check if the subroom is realistically big enough to navigate (1 cell for walls after all)
        if (xSpaceGiven < 3 || ySpaceGiven < 3)
        {
            return null;
        }

        //expand the whole room bounding box
        if (xStart > xSubOffset)
            xStart = xSubOffset;
        if (xEnd < xSubOffset + xSpaceGiven)
            xEnd = xSubOffset + xSpaceGiven;
        if (yStart > ySubOffset)
            yStart = ySubOffset;
        if (yEnd < ySubOffset + xSpaceGiven)
            yEnd = ySubOffset + ySpaceGiven;

        print("Room bounding box: x: " + xStart + ", " + xEnd + ". y: " + yStart + ", " + yEnd);

        //return the spec of the new room to be generated
        int[] result = { xSubOffset, ySubOffset, xSpaceGiven, ySpaceGiven };
        return result;
    }

    private void generateSubRoom(int roomX, int roomY, int offsetX, int offsetY)
    {
        //generate the corner tiles
        genCornerTile(offsetX, offsetY, 1, 1);
        genCornerTile(roomX-1 + offsetX, offsetY, -1, 1);
        genCornerTile(roomX-1 + offsetX, roomY-1 + offsetY, -1, -1);
        genCornerTile(offsetX, roomY-1 + offsetY, 1, -1);

        //generate walls
        genWall(roomX-1, offsetX, offsetY, true, false);
        genWall(roomX-1, offsetX, roomY-1 + offsetY, true, true);
        genWall(roomY-1, offsetX, offsetY, false, false);
        genWall(roomY-1, offsetX + roomX-1, offsetY, false, true);

        //fill the middle bit
        for (int x = 1; x < roomX-1; x++)
        {
            for (int y = 1; y < roomY-1; y++)
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
        GameObject temp = Instantiate(template, new Vector3(xPos + roomOffsetX, (yPos + roomOffsetY) * -1, 0), Quaternion.identity) as GameObject;
        grid[xPos, yPos, 0] = temp;
        return temp;
    }

    private void genCornerTile(int xPos, int yPos, int xScale, int yScale)
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

    private void genWall(int noOfTiles, int xPos, int yPos, bool xAxis, bool flip)
    {
        for (int i = 1; i < noOfTiles; i++)
        {
            GameObject temp;
            if (xAxis)
            {
                temp = createTile(topTile, xPos + i, yPos);
            }
            else
            {
                temp = createTile(sideTile, xPos, yPos + i);
            }
            if (flip && temp != null)
            {
                Vector3 theScale = temp.transform.localScale;
                if (xAxis)
                    theScale.y *= -1;
                else
                    theScale.x *= -1;
                temp.transform.localScale = theScale;
            }
        }
    }
}
