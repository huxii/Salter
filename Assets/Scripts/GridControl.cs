using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GridControl : MonoBehaviour 
{
	public GameObject gridBlockPrefab;
	public GameObject flowPrefab;
	public int gridWidth = 10;
	public int gridLength = 10;
	public int gridHeight = 1;
	public float unitSize = 1.0f;
	public GameObject[] colliderMap;

	public GameObject nextAction;
	public int startFlowX;
	public int startFlowY;
	public int startFlowZ;
	public int endFlowX;
	public int endFlowY;
	public int endFlowZ;

	List<Vector3Int> dir;
	public List<int> queue;
	public List<Vector3> path;
	public int[] before;
	bool flowStarted;
	bool flowEnded;

	// Use this for initialization
	void Start () 
	{
		flowStarted = false;
		flowEnded = false;

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
					newBlock.transform.localPosition = GridPosition(x, y, z);
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
			int nextGridIdx = CoordsToIdx(nextGridIdx3.x, nextGridIdx3.y, nextGridIdx3.z);
			if (nextGridIdx != -1)
			{
				if (colliderMap[nextGridIdx].GetComponent<GridBlockControl>().moveObstacles > 0)
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
			int nextGridIdx = CoordsToIdx(nextGridIdx3.x, nextGridIdx3.y, nextGridIdx3.z);
			if (nextGridIdx != -1)
			{
				if (colliderMap[nextGridIdx].GetComponent<GridBlockControl>().moveObstacles > 0)
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
			int nextGridIdx = CoordsToIdx(nextGridIdx3.x, nextGridIdx3.y, nextGridIdx3.z);
			if (nextGridIdx != -1)
			{
				if (colliderMap[nextGridIdx].GetComponent<GridBlockControl>().moveObstacles > 0)
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
			int nextGridIdx = CoordsToIdx(nextGridIdx3.x, nextGridIdx3.y, nextGridIdx3.z);
			if (nextGridIdx != -1)
			{
				if (colliderMap[nextGridIdx].GetComponent<GridBlockControl>().moveObstacles > 0)
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
			//Debug.Log(result);
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
		}	
		return false;
	}

	public void MoveToGrid(GameObject block, Vector3Int startGridIdx3, Vector3Int endGridIdx3)
	{
		int startGridIdx = CoordsToIdx(startGridIdx3.x, startGridIdx3.y, startGridIdx3.z);
		int endGridIdx = CoordsToIdx(endGridIdx3.x, endGridIdx3.y, endGridIdx3.z);

		if (startGridIdx == endGridIdx)
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

			block.GetComponent<PlayerBlocksControl>().StartMoving();

			AudioSource audio = block.GetComponent<AudioSource>();
			block.transform.DOPath(waypoints, (path.Count - 1) * 0.18f).SetEase(Ease.InOutCubic)
				.OnWaypointChange((int idx) => {if (idx != 0) {audio.Play();}})
				.OnComplete(() => {block.GetComponent<PlayerBlocksControl>().StopMoving();});
		}
		else
		{
			//Debug.Log("No Path");
		}
	}

	public void StartFlow()
	{
		flowStarted = true;
		GameObject newFlow = Instantiate(flowPrefab, colliderMap[CoordsToIdx(startFlowX, startFlowY, startFlowZ)].transform);
	}

	public void ExpandFlow(int x, int y, int z)
	{
		for (int i = 0; i < 4; ++i)
		{
			int dx = x + dir[i].x;
			int dy = y + dir[i].y;
			int dz = z + dir[i].z;
			int idx = CoordsToIdx(dx, dy, dz);
			if (idx != -1 && colliderMap[idx].GetComponent<GridBlockControl>().flowObstacles == 0)
			{
				GameObject newFlow = Instantiate(flowPrefab, colliderMap[idx].transform);
				++colliderMap[idx].GetComponent<GridBlockControl>().flowObstacles;
				if (i == 0)
				{
					
				}
				else
				if (i == 1)
				{
					newFlow.transform.localEulerAngles = new Vector3(0, -180f, 0);
				}
				else
				if (i == 2)
				{
					newFlow.transform.localEulerAngles = new Vector3(0, -90f, 0);		
				}
				else
				if (i == 3)
				{
					newFlow.transform.localEulerAngles = new Vector3(0, 90f, 0);			
				}
			}
		}
	}

	public void EndFlow()
	{
		flowEnded = true;
		if (nextAction.CompareTag("Grid"))
		{
			nextAction.GetComponent<GridControl>().StartFlow();
		}
		else
		if (nextAction.CompareTag("Water"))
		{
			nextAction.GetComponent<FlowControl>().Flow();
			GameObject.Find("Level Manager").SendMessage("LevelComplete");
		}
	}

	public void TriggerEnter(Collider other, int x, int y, int z)
	{
		if (!flowStarted && other.gameObject.CompareTag("Water") 
			&& CoordsToIdx(x, y, z) == CoordsToIdx(startFlowX, startFlowY, startFlowZ))
		{
			StartFlow();
		}

		if (!flowEnded && other.gameObject.CompareTag("Water")
		    && CoordsToIdx(x, y, z) == CoordsToIdx(endFlowX, endFlowY, endFlowZ))
		{
			EndFlow();
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
		if (x < gridWidth && y < gridHeight && z < gridLength &&
		    x >= 0 && y >= 0 && z >= 0)
		{
			return x * gridLength + z;
		}

		return -1;
	}
		
	public IEnumerator DelayToMove(float delaySeconds)
	{
		yield return new WaitForSeconds(delaySeconds);
	}
}
