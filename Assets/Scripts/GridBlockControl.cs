using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBlockControl : MonoBehaviour
{
	public bool cantFlow;
	public bool cantMove;
	public int x;
	public int y;
	public int z;

	// Use this for initialization
	void Start()
	{
		cantFlow = false;
		cantMove = false;
	}
	
	// Update is called once per frame
	void FixedUpdate()
	{
		//Debug.Log("FixUpdate");
		cantFlow = false;
		cantMove = false;
	}
		
	void OnTriggerStay(Collider other)
	{
		//Debug.Log("OnTriggerStay");
		//yield return new WaitForFixedUpdate();
		if (!other.CompareTag("Untagged") && !other.CompareTag("Sound") && !other.CompareTag("Bridge") && !other.CompareTag("Grid")
			&& !other.CompareTag("GridBlock") && !other.CompareTag("AssetTrigger"))
		{
			cantFlow = true;
			/*
			if (x == 0 && y == 0 && z == 5)
			{
				Debug.Log(other.gameObject.name);
			}
			*/
			//Debug.Log(other.gameObject.name);
		}

		if (!other.CompareTag("Untagged") && !other.CompareTag("Sound") && !other.CompareTag("Bridge") && !other.CompareTag("Grid")
			&& !other.CompareTag("GridBlock"))
		{
			cantMove = true;
			/*
			if (x == 0 && y == 0 && z == 5)
			{
				Debug.Log(other.gameObject.name);
			}
			*/
			//Debug.Log(other.gameObject.name);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		transform.parent.GetComponent<GridControl>().TriggerEnter(other, x, y, z);
	}
}
