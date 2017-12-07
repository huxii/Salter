using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GridControl : MonoBehaviour 
{
	public GameObject gridBlockPrefab;
	public GameObject[] models;
	public int gridWidth = 10;
	public int gridLength = 10;
	public int gridHeight = 1;
	public float unitSize = 1.0f;
	public GameObject[, , ] colliderMap;

	List<Vector3Int> dir;
	public List<GameObject> queue;
	public List<Vector3> path;
	public GameObject[] before;
	bool movable;

	// Use this for initialization
	void Start () 
	{
		movable = true; 

		dir = new List<Vector3Int>();
		dir.Add(new Vector3Int(1, 0, 0));
		dir.Add(new Vector3Int(-1, 0, 0));
		dir.Add(new Vector3Int(0, 0, 1));
		dir.Add(new Vector3Int(0, 0, -1));

		queue = new List<GameObject>();
		before = new GameObject[gridWidth * gridHeight * gridLength];
		path = new List<Vector3>();

		InitGrid();
		//UpdateGrid();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void InitGrid()
	{
		colliderMap = new GameObject[gridWidth, gridLength, gridHeight];
		for (int x = 0; x < gridWidth; ++x)
		{
			for (int y = 0; y < gridHeight; ++y)
			{
				for (int z = 0; z < gridLength; ++z)
				{
					GameObject newBlock = Instantiate(gridBlockPrefab, gameObject.transform);
					newBlock.GetComponent<GridBlockControl>().x = x;
					newBlock.GetComponent<GridBlockControl>().y = y;
					newBlock.GetComponent<GridBlockControl>().z = z;
					newBlock.transform.position = GridPosition(x, y, z);
					colliderMap[x, z, y] = newBlock;
				}
			}
		}
	}
	/*
	public void UpdateGrid()
	{
		if (models.Length <= 0)
		{
			return;
		}

		int startX = -gridWidth / 2;
		int endX = gridWidth - gridWidth / 2;
		int startY = 0;
		int endY = gridHeight;
		int startZ = -gridLength / 2;
		int endZ = gridLength - gridLength / 2;

		for (int x = startX; x < endX; ++x)
		{
			float X = x * unitSize + unitSize / 2;
			for (int y = startY; y < endY; ++y)
			{
				float Y = y * unitSize + unitSize / 2;
				for (int z = startZ; z < endZ; ++z)
				{
					float Z = z * unitSize + unitSize / 2;
					for (int i = 0; i < models.Length; ++i)
					{
						if (models[i].GetComponent<BoxCollider>().bounds.Contains(new Vector3(X, Y, Z)) == false)
						{
							colliderMap[x - startX, z - startZ, y - startY] = false;
						}
						else
						{
							Debug.Log("x: " + x + " y: " + y + " z: " + z + " X:" + X + " Y:" + Y + " Z:" + Z);
						}
					}
				}
			}
		}
	}
	*/

	public bool FindPath(GameObject startGrid, GameObject endGrid)
	{
		int head = 0;
		int tail = 0;

		queue.Clear();
		queue.Add(startGrid);
		++tail;

		bool[] visited = new bool[gridWidth * gridHeight * gridLength];
		for (int i = 0; i < gridWidth * gridHeight * gridLength; ++i)
		{
			visited[i] = false;
			before[i] = null;
		}

		while (head < tail)
		{
			GameObject curGrid = queue[head++];
			Vector3Int curGridIdx = new Vector3Int(
				curGrid.GetComponent<GridBlockControl>().x,
				curGrid.GetComponent<GridBlockControl>().y,
				curGrid.GetComponent<GridBlockControl>().z
			);

			int curIdx = curGridIdx.x * gridLength + curGridIdx.z;
			visited[curIdx] = true;

			for (int i = 0; i < dir.Count; ++i)
			{
				Vector3Int nextGridIdx = curGridIdx + dir[i];
				if (nextGridIdx.x < gridWidth && nextGridIdx.y < gridHeight && nextGridIdx.z < gridLength &&
				    nextGridIdx.x >= 0 && nextGridIdx.y >= 0 && nextGridIdx.z >= 0)
				{
					GameObject nextGrid = colliderMap[nextGridIdx.x, nextGridIdx.z, nextGridIdx.y];
					int nextIdx = nextGridIdx.x * gridLength + nextGridIdx.z;

					if (!visited[nextIdx] && !nextGrid.GetComponent<GridBlockControl>().collided)
					{
						before[nextIdx] = curGrid;
						queue.Add(nextGrid);
						++tail;
						if (nextGrid == endGrid)
						{
							return true;
						}
					}
				}
			}
		}	
		return false;
	}

	public void MoveToGrid(GameObject block, GameObject startGrid, GameObject endGrid)
	{
		if (!movable)
		{
			return;
		}
			
		if (FindPath(startGrid, endGrid))
		{
			path.Clear();
			GameObject grid = endGrid;
			while (grid != startGrid)
			{
				int idx = grid.GetComponent<GridBlockControl>().x * gridLength + grid.GetComponent<GridBlockControl>().z;
				path.Add(grid.transform.position);
				grid = before[idx];
			}

			Vector3[] waypoints = new Vector3[path.Count + 1];
			int j = 0;
			waypoints[j++] = startGrid.transform.position;
			for (int i = path.Count - 1; i >= 0; --i)
			{
				waypoints[j++] = path[i];
			}

			movable = false;
			block.transform.DOPath(waypoints, path.Count * 0.12f).OnComplete(CanMove);
			//StartCoroutine(DelayToMove(path.Count * 0.12f));
		}
	}

	public Vector3 GridPosition(int x, int y, int z)
	{
		Vector3 pos = new Vector3();
		pos.x = (x - gridWidth / 2) * unitSize + 0.5f;
		pos.y = (y - gridHeight / 2) * unitSize;
		pos.z = (z - gridLength / 2) * unitSize + 0.5f;

		return pos;
	}
		
	public GameObject GetGrid(Vector3 pos)
	{
		int x = (int)((pos.x - 0.5f) / unitSize + gridWidth / 2);
		int y = (int)(pos.y / unitSize + gridHeight / 2);
		int z = (int)((pos.z - 0.5f) / unitSize + gridLength / 2);

		return colliderMap[x, z, y];
	}

	public void CanMove()
	{
		movable = true;
	}

	public IEnumerator DelayToMove(float delaySeconds)
	{
		yield return new WaitForSeconds(delaySeconds);
		movable = true;
	}
}
