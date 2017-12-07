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

	int CollisionTest(Vector3Int curGridIdx3, int blockWidth, int blockLength, int blockHeight)
	{
		int result = 0;

		Vector3Int nextGridIdx3;
		bool collided = false;

		// +x
		for (int i = 0; i < blockLength; ++i)
		{
			Vector3Int offset = new Vector3Int(blockWidth - 1, 0, i);
			nextGridIdx3 = curGridIdx3 + offset + dir[0];
			if (nextGridIdx3.x < gridWidth && nextGridIdx3.y < gridHeight && nextGridIdx3.z < gridLength &&
			    nextGridIdx3.x >= 0 && nextGridIdx3.y >= 0 && nextGridIdx3.z >= 0)
			{
				int nextGridIdx = CoordsToIdx(nextGridIdx3.x, nextGridIdx3.y, nextGridIdx3.z);
				if (colliderMap[nextGridIdx].GetComponent<GridBlockControl>().collided)
				{
					collided = true;
					break;
				}
			}
			else
			{
				collided = true;
				break;
			}
		}

		if (collided)
		{
			result |= (1 << 0);
			collided = false;
		}

		// -x
		for (int i = 0; i < blockLength; ++i)
		{
			Vector3Int offset = new Vector3Int(0, 0, i);
			nextGridIdx3 = curGridIdx3 + offset + dir[1];
			if (nextGridIdx3.x < gridWidth && nextGridIdx3.y < gridHeight && nextGridIdx3.z < gridLength &&
				nextGridIdx3.x >= 0 && nextGridIdx3.y >= 0 && nextGridIdx3.z >= 0)
			{
				int nextGridIdx = CoordsToIdx(nextGridIdx3.x, nextGridIdx3.y, nextGridIdx3.z);
				if (colliderMap[nextGridIdx].GetComponent<GridBlockControl>().collided)
				{
					collided = true;
					break;
				}
			}
			else
			{
				collided = true;
				break;
			}
		}
		if (collided)
		{
			result |= (1 << 1);
			collided = false;
		}

		// +z
		for (int i = 0; i < blockWidth; ++i)
		{
			Vector3Int offset = new Vector3Int(i, 0, blockLength - 1);
			nextGridIdx3 = curGridIdx3 + offset + dir[2];
			if (nextGridIdx3.x < gridWidth && nextGridIdx3.y < gridHeight && nextGridIdx3.z < gridLength &&
				nextGridIdx3.x >= 0 && nextGridIdx3.y >= 0 && nextGridIdx3.z >= 0)
			{
				int nextGridIdx = CoordsToIdx(nextGridIdx3.x, nextGridIdx3.y, nextGridIdx3.z);
				if (colliderMap[nextGridIdx].GetComponent<GridBlockControl>().collided)
				{
					collided = true;
					break;
				}
			}
			else
			{
				collided = true;
				break;
			}
		}

		if (collided)
		{
			result |= (1 << 2);
			collided = false;
		}
			
		// -z
		for (int i = 0; i < blockWidth; ++i)
		{
			Vector3Int offset = new Vector3Int(i, 0, 0);
			nextGridIdx3 = curGridIdx3 + offset + dir[3];
			//Debug.Log(nextGridIdx3);
			if (nextGridIdx3.x < gridWidth && nextGridIdx3.y < gridHeight && nextGridIdx3.z < gridLength &&
				nextGridIdx3.x >= 0 && nextGridIdx3.y >= 0 && nextGridIdx3.z >= 0)
			{
				int nextGridIdx = CoordsToIdx(nextGridIdx3.x, nextGridIdx3.y, nextGridIdx3.z);
				if (colliderMap[nextGridIdx].GetComponent<GridBlockControl>().collided)
				{
					collided = true;
					break;
				}
			}
			else
			{
				collided = true;
				break;
			}
		}

		if (collided)
		{
			result |= (1 << 3);
			collided = false;
		}

		return result;
	}

	public bool FindPath(int startGridIdx, int endGridIdx, int blockWidth, int blockLength, int blockHeight)
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

			int result = CollisionTest(curGridIdx3, blockWidth, blockLength, blockHeight);
			Debug.Log(result);
			for (int i = 0; i < dir.Count; ++i)
			{
				int collided = (result & (1 << i));
				if (collided == 0)
				{
					Vector3Int nextGridIdx3 = curGridIdx3 + dir[i];
					int nextGridIdx = CoordsToIdx(nextGridIdx3.x, nextGridIdx3.y, nextGridIdx3.z);
					if (!visited[nextGridIdx])
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
			/*
			Debug.Log(
				curGridIdx + " " +
				colliderMap[curGridIdx].GetComponent<GridBlockControl>().x + " " +
				colliderMap[curGridIdx].GetComponent<GridBlockControl>().y + " " +
				colliderMap[curGridIdx].GetComponent<GridBlockControl>().z
			);
			*/
			/*
			for (int i = 0; i < dir.Count; ++i)
			{
				Vector3Int nextGridIdx3 = curGridIdx3 + dir[i];
				//Debug.Log(curGridIdx3.x + " " + curGridIdx3.y + " " + curGridIdx3.z);

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
			*/
		}	
		return false;
	}

	public void MoveToGrid(GameObject block, Vector3Int startGridIdx3, Vector3Int endGridIdx3)
	{
		int startGridIdx = CoordsToIdx(startGridIdx3.x, startGridIdx3.y, startGridIdx3.z);
		int endGridIdx = CoordsToIdx(endGridIdx3.x, endGridIdx3.y, endGridIdx3.z);

		if (!movable || startGridIdx == endGridIdx)
		{
			return;
		}

		if (FindPath(startGridIdx, endGridIdx, 
			block.GetComponent<PlayerBlocksControl>().width, block.GetComponent<PlayerBlocksControl>().length, block.GetComponent<PlayerBlocksControl>().height))
		{
			path.Clear();
			int gridIdx = endGridIdx;
			while (gridIdx != startGridIdx)
			{
				path.Add(colliderMap[gridIdx].transform.position);
				gridIdx = before[gridIdx];
			}
			path.Add(colliderMap[startGridIdx].transform.position);

			Vector3[] waypoints = new Vector3[path.Count];
			int j = 0;
			for (int i = path.Count - 1; i >= 0; --i)
			{
				waypoints[j++] = path[i];
			}


			movable = false;
			//block.transform.position = waypoints[path.Count - 1];
			block.transform.DOPath(waypoints, (path.Count - 1) * 0.25f).SetEase(Ease.InOutCubic).OnComplete(CanMove);
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
	}
}
