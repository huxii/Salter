using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBehavior : MonoBehaviour
{
	public float speed;
	public bool isFlowing = false;
	public bool isInitial = false;
	public bool isAwake = false;
	public bool canCrossEdge = false;
	public bool canBeStoppedByWater = true;

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

			if (gameObject.transform.localScale.x >= 300.0f)
			{
				Stop();
			}
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

		if (other.gameObject.CompareTag("Goal"))
		{
			GameObject.Find("Level Manager").SendMessage("LevelComplete");
		}
	}

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("ObstacleBlock") || other.gameObject.CompareTag("Obstacle"))
		{
			//Debug.Log("wall " + other.gameObject.name + " " + gameObject.name);
			Pause();
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("ObstacleBlock") || other.gameObject.CompareTag("Obstacle"))
		{
			Resume();
		}
	}
}
