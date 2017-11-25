using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTrigger : MonoBehaviour
{
	public GameObject waterUnit;

	// Use this for initialization
	void Start()
	{
		waterUnit = transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update()
	{
		
	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log(other.gameObject.name);
		if (waterUnit)
		{
			waterUnit.GetComponent<WaterControl>().WaterTriggerEnter(other);
		}
	}
	/*
	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Obstacle"))
		{
			if (gameObject.CompareTag("EmptyBlock") && other.gameObject.GetComponent<WaterBehavior>().isAwake)
			{
				gameObject.tag = "WaterBlock";
			}
		}
	}
	*/
}
