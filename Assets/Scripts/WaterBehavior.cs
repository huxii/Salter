using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBehavior : MonoBehaviour
{
	public float speed;
	public bool isFlowing;
	public bool isInitial;
	public bool isAwake;
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
		//gameObject.transform.localScale = new Vector3(0, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
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
			Stop();
		}
	}
}
