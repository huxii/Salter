using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenBlockBehavior : MonoBehaviour
{
	public List<GameObject> blockingWaterGO;

	// Use this for initialization
	void Start()
	{
		
	}
	
	// Update is called once per frame
	void Update()
	{
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Obstacle"))
		{
			gameObject.tag = "ObstacleBlock";
		}

		/*
		if (other.gameObject.CompareTag("Water"))
		{
			if (gameObject.CompareTag("ObstacleBlock") && other.gameObject.GetComponent<WaterBehavior>().isAwake)
			{
				Debug.Log(gameObject.name + " is blocking " + other.gameObject.name);
				other.gameObject.GetComponent<WaterBehavior>().Pause();
				blockingWaterGO = other.gameObject;
			}
		}
		*/

	}

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Water"))
		{
			if (other.gameObject.GetComponent<WaterBehavior>().isAwake)
			{
				if (gameObject.CompareTag("ObstacleBlock"))
				{
					//Debug.Log(gameObject.name + " is blocking " + other.gameObject.name);
					other.gameObject.GetComponent<WaterBehavior>().Pause();
					blockingWaterGO.Add(other.gameObject);
				}
				else
				if (gameObject.CompareTag("EmptyBlock"))
				{
					gameObject.tag = "WaterBlock";
				}
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Obstacle"))
		{
			gameObject.tag = "EmptyBlock";

			if (blockingWaterGO.Count > 0)
			{
				foreach (GameObject blockingWater in blockingWaterGO)
				{
					blockingWater.GetComponent<WaterBehavior>().Resume();
				}
			}
		}

	}
}
