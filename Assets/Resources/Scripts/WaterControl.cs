using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterControl : MonoBehaviour
{
	public bool goForward = true;
	public bool goBack = true;
	public bool goLeft = true;
	public bool goRight = true;

	private bool isTriggered;
	private List<GameObject> waters;

	// Use this for initialization
	void Start()
	{
		isTriggered = false;

		waters = new List<GameObject>();
		foreach (Transform child in transform)
		{
			if (child.gameObject.CompareTag("Water"))
			{
				float ry = child.gameObject.transform.localRotation.eulerAngles.y;
				//Debug.Log(ry);
				if (Mathf.Abs(ry - 0.0f) < 0.01f)
				{
					if (goForward)
					{
						waters.Add(child.gameObject);
					}
					else
					{
						child.gameObject.SetActive(false);
					}
				}
				else
				if (Mathf.Abs(ry - 270.0f) < 0.01f)
				{
					if (goLeft)
					{
						waters.Add(child.gameObject);
					}
					else
					{
						child.gameObject.SetActive(false);
					}
				}
				else
				if (Mathf.Abs(ry - 90.0f) < 0.01f)
				{
					if (goRight)
					{
						waters.Add(child.gameObject);
					}
					else
					{
						child.gameObject.SetActive(false);
					}
				}
				else
				if (Mathf.Abs(ry - 180.0f) < 0.01f)
				{
					if (goBack)
					{
						waters.Add(child.gameObject);
					}
					else
					{
						child.gameObject.SetActive(false);
					}
				}

			}
		}
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void WaterTriggerEnter(Collider other)
	{
		foreach (GameObject water in waters)
		{
			if (water == other.gameObject)
			{
				return;
			}
		}

		if (other.gameObject.CompareTag("Water"))
		{
			if (!isTriggered)
			{
				isTriggered = true;
				Debug.Log(other.gameObject.name + " reaches " + gameObject.name);
				//run next water
				foreach (GameObject water in waters)
				{
					if (water != null)
					{
						water.GetComponent<WaterBehavior>().Run();
					}
				}				
			}
			Debug.Log("Stop other water " + other.gameObject.name);
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
