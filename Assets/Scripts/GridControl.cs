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
	public GameObject[] colliderMap;

	List<Vector3Int> dir;
	public List<int> queue;
	public List<Vector3> path;
	public int[] before;
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

		queue = new List<int>();
		before = new int[gridWidth * gridHeight * gridLength];
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
		colliderMap = new GameObject[gridWidth * gridLength * gridHeight];
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
					colliderMap[CoordsToIdx(x, y, z)] = newBlock;
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

	public bool FindPath(int startGridIdx, int endGridIdx)
	{
		int head = 0;
		int tail = 0;

		queue.Clear();
		queue.Add(startGridIdx);
		++tail;

		bool[] visited = new bool[gridWidth * gridHeight * gridLength];
		for (int i = 0; i < gridWidth * gridHeight * gridLength; ++i)
		{
			visited[i] = false;
		}
			
		while (head < tail)
		{
			int curGridIdx = queue[head++];
			visited[curGridIdx] = true;
			Vector3Int curGridIdx3 = new Vector3Int(
				colliderMap[curGridIdx].GetComponent<GridBlockControl>().x,
				colliderMap[curGridIdx].GetComponent<GridBlockControl>().y,
				colliderMap[curGridIdx].GetComponent<GridBlockControl>().z
			);
			/*
			Debug.Log(
				curGridIdx + " " +
				colliderMap[curGridIdx].GetComponent<GridBlockControl>().x + " " +
				colliderMap[curGridIdx].GetComponent<GridBlockControl>().y + " " +
				colliderMap[curGridIdx].GetComponent<GridBlockControl>().z
			);
			*/
			for (int i = 0; i < dir.Count; ++i)
			{
				Vector3Int nextGridIdx3 = curGridIdx3 + dir[i];
				Debug.Log(curGridIdx3.x + " " + curGridIdx3.y + " " + curGridIdx3.z);

				if (nextGridIdx3.x < gridWidth && nextGridIdx3.y < gridHeight && nextGridIdx3.z < gridLength &&
					nextGridIdx3.x >= 0 && nextGridIdx3.y >= 0 && nextGridIdx3.z >= 0)
				{
					int nextGridIdx = CoordsToIdx(nextGridIdx3.x, nextGridIdx3.y, nextGridIdx3.z);
					GameObject nextGrid = colliderMap[nextGridIdx];
					if (!visited[nextGridIdx] && !nextGrid.GetComponent<GridBlockControl>().collided)
					{
						before[nextGridIdx] = curGridIdx;
						queue.Add(nextGridIdx);
						++tail;
						if (nextGridIdx == endGridIdx)
						{
							return true;
						}
					}
				}
			}
		}	
		return false;
	}

	public void MoveToGrid(GameObject block, Vector3Int startGridIdx3, Vector3Int endGridIdx3)
	{
		if (!movable)
		{
			return;
		}

		int startGridIdx = CoordsToIdx(startGridIdx3.x, startGridIdx3.y, startGridIdx3.z);
		int endGridIdx = CoordsToIdx(endGridIdx3.x, endGridIdx3.y, endGridIdx3.z);

		if (FindPath(startGridIdx, endGridIdx))
		{
			path.Clear();
			int gridIdx = endGridIdx;
			while (gridIdx != startGridIdx)
			{
				path.Add(colliderMap[gridIdx].transform.position);
				gridIdx = before[gridIdx];
			}
			//path.Add(colliderMap[startGridIdx].transform.position);

			Vector3[] waypoints = new Vector3[path.Count];
			int j = 0;
			for (int i = path.Count - 1; i >= 0; --i)
			{
				waypoints[j++] = path[i];
			}

			movable = false;
			//block.transform.position = waypoints[path.Count - 1];
			block.transform.DOPath(waypoints, path.Count * 0.2f).SetEase(Ease.InCubic).OnComplete(CanMove);
			//StartCoroutine(DelayToMove(path.Count * 0.12f));
		}
		else
		{
			Debug.Log("No Path");
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
		int x = (int)(pos.x / unitSize + gridWidth / 2);
		int y = (int)(pos.y / unitSize + gridHeight / 2);
		int z = (int)(pos.z/ unitSize + gridLength / 2);

		return colliderMap[CoordsToIdx(x, y, z)];
	}

	public int CoordsToIdx(int x, int y, int z)
	{
		return x * gridLength + z;
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
