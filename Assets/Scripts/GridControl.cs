using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridControl : MonoBehaviour 
{
	public GameObject[] models;
	public int gridWidth = 10;
	public int gridLength = 10;
	public int gridHeight = 1;
	public float unitSize = 1.0f;
	public bool[, , ] colliderMap;

	// Use this for initialization
	void Start () 
	{
		InitGrid();
		UpdateGrid();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void InitGrid()
	{
		colliderMap = new bool[gridWidth, gridLength, gridHeight];
		for (int x = 0; x < gridWidth; ++x)
		{
			for (int y = 0; y < gridHeight; ++y)
			{
				for (int z = 0; z < gridLength; ++z)
				{
					colliderMap[x, z, y] = true;
				}
			}
		}
	}

	void UpdateGrid()
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
}
