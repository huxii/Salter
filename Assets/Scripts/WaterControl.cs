using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterControl : MonoBehaviour
{
	private bool isTriggered;
	public List<GameObject> waters;

	// Use this for initialization
	void Start()
	{
		isTriggered = false;
		waters.Clear();
		foreach (Transform child in transform)
		{
			if (child.gameObject.CompareTag("Water"))
			{
				waters.Add(child.gameObject);
			}
		}
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void WaterTriggerEnter(Collider other)
	{
		foreach (GameObject water in waters) {
			if (water == other.gameObject) {
				return;
			}
		}
		if (!isTriggered && other.gameObject.CompareTag ("Water")) {
			isTriggered = true;
			Debug.Log (other.gameObject.name + " reaches " + gameObject.name);
			//run next water
			foreach (GameObject water in waters) {
				if (water != null) {
					water.GetComponent<WaterBehavior> ().Run ();
				}
			}
			//stop previous water
			other.gameObject.GetComponent<WaterBehavior>().Stop();
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
