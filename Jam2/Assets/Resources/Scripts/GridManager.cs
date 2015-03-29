using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridManager : MonoBehaviour {

	public RoomGenerator rg;
	public int currentRoom;
	public GameObject[,,] grid;
	public int [,] distanceSteps;
	public Queue <KeyValuePair<int,int>> nodes;
	KeyValuePair<int,int> playerTile;

	public static GridManager Instance;
	void Awake()
	{
		if (Instance == null) Instance = this;
	}
	void Start () {
		rg = (RoomGenerator)FindObjectOfType(typeof(RoomGenerator));
		currentRoom = 0;
		nodes = new Queue<KeyValuePair<int, int>>();
	}
	
	public void pathMove()
	{
		Debug.Log("PATHMOVE");
		grid = rg.rooms.First.Value;
		//Debug.Log(grid.GetLength(1));
		distanceSteps = new int[grid.GetLength(0), grid.GetLength(1)] ;
		for(int i = 0; i < (distanceSteps.GetLength(0));i++)
			for(int j = 0; j < (distanceSteps.GetLength(1));j++)  
		{
				distanceSteps[i,j] = -1;
				
		}
		GameObject player = GameObject.FindWithTag("Player");
		playerTile = findPlayer(player);
		if (playerTile.Key!= -1){
			//Debug.Log("FOUIND");
			setDistanceArray(playerTile);
		}

	}

	public KeyValuePair<int, int> findPlayer(GameObject o){
		for(int i = 0; i < (grid.GetLength(0));i++)
			for(int j = 0; j < (grid.GetLength(1));j++)
				if (grid[i,j,3] !=null && grid[i,j,3].CompareTag("Player"))
			{
				//Debug.Log(i);
				return new KeyValuePair<int,int>(i,j);
			}
		return new KeyValuePair<int,int>(-1,-1);
	}

	public KeyValuePair<int, int> findObject(GameObject o){
		for(int i = 0; i < (grid.GetLength(0));i++)
			for(int j = 0; j < (grid.GetLength(1));j++)
				if (grid[i,j,3] !=null && grid[i,j,3]==o)
			{
				//Debug.Log(i);
					return new KeyValuePair<int,int>(i,j);
			}
		return new KeyValuePair<int,int>(-1,-1);
	}

	public void changePlayerTile(Vector2 a, GameObject o){
		grid[playerTile.Key, playerTile.Value, 3] = null;
		playerTile= new KeyValuePair<int, int>((int)(playerTile.Key + a.x), (int)(playerTile.Value - a.y));
		grid[playerTile.Key, playerTile.Value, 3] = o;
	}
	

	public void setDistanceArray(KeyValuePair<int,int>temp)
	{
		KeyValuePair<int,int> origin = temp;
		distanceSteps[origin.Key, origin.Value] = 0;
		distancesAroundNode(origin.Key,origin.Value);
		while(nodes.Count > 0){
			origin = nodes.Dequeue();
			distancesAroundNode(origin.Key,origin.Value);
		}


	}

	public void distancesAroundNode(int x, int y){
		int distance = distanceSteps[x,y] + 1;
		setNode (x+1, y, distance);
		setNode (x-1, y, distance);
		setNode (x, y+1, distance);
		setNode (x, y-1, distance);
	}

	public void setNode(int x, int y, int value)
	{
		if(x < distanceSteps.GetLength(0) && y < distanceSteps.GetLength(1) && x>=0 && y>=0 && walkable (x,y)){
			if(distanceSteps[x,y] < 0){
				distanceSteps[x,y] = value;
				nodes.Enqueue(new KeyValuePair<int,int >(x,y));
			}else{
				if(distanceSteps[x,y] > value){
					distanceSteps[x,y] = value;
					nodes.Enqueue(new KeyValuePair<int,int >(x,y));
				}
			}
		}
		//Debug.Log (value);
	}

	public bool walkable(int x, int y)
	{
		if(grid[x,y,0] == null)
			return false;
		if(((GameObject)grid[x,y,0]).CompareTag("Wall"))
			return false;
		if(((GameObject)grid[x,y,0]).CompareTag("Enemy"))
			return false;
		return true;

	}

	public Vector2 getMoveDirection(GameObject o){
		KeyValuePair<int, int> tile = findObject(o);
		KeyValuePair<int, int> previous = tile;
		tile = getClosestAdjecent(tile.Key, tile.Value);
		grid[previous.Key + tile.Key, previous.Value - tile.Value, 3] = o;
		grid[previous.Key, previous.Value, 3] = null;
		return new Vector2( tile.Key, tile.Value);

	}



	public KeyValuePair<int, int> getClosestAdjecent(int x, int y){
		int min = 900;
		KeyValuePair<int, int> temp = new KeyValuePair<int,int >();
		if (x > 1 && distanceSteps[x-1,y] >= 0 && distanceSteps[x-1,y] < min && walkable(x-1,y)){
			min = distanceSteps[x-1,y];
			temp = new KeyValuePair<int,int >(-1, 0);
		}
		if (x < distanceSteps.GetLength(0)-1 && distanceSteps[x+1,y] >= 0 && distanceSteps[x+1,y] < min && walkable(x+1,y)){
			min = distanceSteps[x+1,y];
			temp = new KeyValuePair<int,int >(1, 0);
		}
		if (y > 1 && distanceSteps[x,y-1] >= 0 && distanceSteps[x,y-1] < min && walkable(x,y-1)){
			min = distanceSteps[x,y-1];
			temp = new KeyValuePair<int,int >(0, 1);
		}
		if (y < distanceSteps.GetLength(1)-1 && distanceSteps[x,y+1] >= 0 && distanceSteps[x,y+1] < min && walkable(x,y+1)){
			min = distanceSteps[x,y+1];
			temp = new KeyValuePair<int,int >(0, -1);
		}
		Debug.Log (min);
		return temp;


	}
}
