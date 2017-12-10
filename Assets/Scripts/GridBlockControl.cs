using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBlockControl : MonoBehaviour
{
	public int flowObstacles;
	public int moveObstacles;
	public int x;
	public int y;
	public int z;

	// Use this for initialization
	void Start()
	{
	}

	void OnTriggerEnter(Collider other)
	{
		transform.parent.GetComponent<GridControl>().TriggerEnter(other, x, y, z);

		if (!other.CompareTag("Untagged") && !other.CompareTag("Sound") && !other.CompareTag("Bridge") && !other.CompareTag("Grid")
			&& !other.CompareTag("GridBlock") && !other.CompareTag("AssetTrigger"))
		{
			++flowObstacles;
		}

		if (!other.CompareTag("Untagged") && !other.CompareTag("Sound") && !other.CompareTag("Bridge") && !other.CompareTag("Grid")
			&& !other.CompareTag("GridBlock"))
		{
			++moveObstacles;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (!other.CompareTag("Untagged") && !other.CompareTag("Sound") && !other.CompareTag("Bridge") && !other.CompareTag("Grid")
			&& !other.CompareTag("GridBlock") && !other.CompareTag("AssetTrigger"))
		{
			--flowObstacles;
		}

		if (!other.CompareTag("Untagged") && !other.CompareTag("Sound") && !other.CompareTag("Bridge") && !other.CompareTag("Grid")
			&& !other.CompareTag("GridBlock"))
		{
			--moveObstacles;
		}		
	}

	public void InitCollision()
	{
		flowObstacles = 0;
		moveObstacles = 0;	

		Collider[] others = Physics.OverlapBox(transform.parent.GetComponent<GridControl>().GridPosition(x, y, z), new Vector3(0.45f, 0.45f, 0.45f));
		/*
		foreach (Collider other in others)
		{
			if (other.CompareTag("WallBlock"))
			{
				++flowObstacles;
				++moveObstacles;
			}	
		}
		*/
	}
}
