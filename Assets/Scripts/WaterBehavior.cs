﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBehavior : MonoBehaviour
{
	public float speed;
	public bool isFlowing = false;
	public bool isInitial = false;
	public bool isAwake = false;
	public bool canCrossEdge = false;
	Renderer render;

	// Use this for initialization
	void Start()
	{
		render = GetComponentInChildren<MeshRenderer>();
		render.enabled = false;
		isFlowing = false;
		isAwake = false;
		if (isInitial)
		{
			Run();
		}
	}
	
	// Update is called once per frame
	void Update()
	{
		if (isAwake && isFlowing)
		{
			gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x + speed * Time.deltaTime * 50,
				gameObject.transform.localScale.y,
				gameObject.transform.localScale.z);
		}

	}

	public void Run()
	{
		gameObject.transform.localScale = new Vector3(0.05f, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
		render.enabled = true;
		isFlowing = true;
		isAwake = true;
	}

	public void Stop()
	{
		isAwake = false;
	}

	public void Pause()
	{
		isFlowing = false;
	}

	public void Resume()
	{
		isFlowing = true;
	}
		
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("WallBlock"))
		{
			//Debug.Log("wall " + other.gameObject.name + " " + gameObject.name);
			Stop();
		}
	}
}
