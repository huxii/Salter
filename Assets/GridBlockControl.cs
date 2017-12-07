using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBlockControl : MonoBehaviour
{
	public bool collided;
	public int x;
	public int y;
	public int z;

	// Use this for initialization
	void Start()
	{
		collided = false;
	}
	
	// Update is called once per frame
	void FixedUpdate()
	{
		//Debug.Log("FixUpdate");
		collided = false;
	}
		
	void OnTriggerStay(Collider other)
	{
		//Debug.Log("OnTriggerStay");
		//yield return new WaitForFixedUpdate();
		if (!other.CompareTag("Untagged") && !other.CompareTag("Sound"))
		{
			collided = true;
			//Debug.Log(other.gameObject.name);
		}
	}
	/*
	void OnTriggerExit(Collider other)
	{
		if (!other.CompareTag("Untagged"))
		{
			--collidedObj;
		}		
	}
	*/
}
