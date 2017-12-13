using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlowControl : MonoBehaviour
{
	public GameObject flow;
	public GameObject waterParticlePrefab = null;
	public int x;
	public int y;
	public int z;
	public float length = 1.0f;
	public float speed = 4;
	public bool autoTrigger = true;
	//public bool WaterTrigger = false;
	public bool startFlow = false;
	public bool endFlow = false;

	bool completed = false;

	// Use this for initialization
	void Start()
	{

		if (transform.parent && transform.parent.gameObject.GetComponent<GridBlockControl>())
		{
			x = transform.parent.gameObject.GetComponent<GridBlockControl>().x;
			y = transform.parent.gameObject.GetComponent<GridBlockControl>().y;
			z = transform.parent.gameObject.GetComponent<GridBlockControl>().z;
		}

		gameObject.SetActiveRecursively(false);
		if (autoTrigger)
		{
			Flow();
		}
	}
	
	// Update is called once per frame
	void Update()
	{
		if (completed && !startFlow && !endFlow)
		{
			if (transform.parent && transform.parent.transform.parent)
			{
				transform.parent.transform.parent.gameObject.GetComponent<GridControl>().ExpandFlow(x, y, z);
			}
		}
	}
		
	void FlowComplete()
	{
		if (startFlow)
		{
			//gridManager.GetComponent<GridControl>().StartFlow();
		}
		else
		if (endFlow)
		{
			//Debug.Log("bug");
		}
		else
		{
			completed = true;
			//gridManager.GetComponent<GridControl>().IfEndFlow(x, y, z);
		}
	}

	public void Flow()
	{
		gameObject.SetActiveRecursively(true);

		if (!startFlow && !endFlow)
		{
			GameObject particle = (GameObject)Instantiate(waterParticlePrefab, gameObject.transform);
			particle.transform.localPosition = new Vector3(0, 0.3f, 0);
		}

		flow.transform.localScale = new Vector3(0.01f, flow.transform.localScale.y, flow.transform.localScale.z);
		flow.transform.DOScaleX(length, length / speed).SetEase(Ease.Linear).OnComplete(FlowComplete);
	}

	/*
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Water"))
		{
			x = other.gameObject.GetComponent<GridBlockControl>().x;
			y = other.gameObject.GetComponent<GridBlockControl>().y;
			z = other.gameObject.GetComponent<GridBlockControl>().z;
		}
	}
	*/
}
