using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSingleBlockBehavior : MonoBehaviour 
{
	public int x;
	public int y;
	public int z;

	void OnMouseDrag()
	{
		transform.parent.gameObject.GetComponent<PlayerBlocksControl>().Drag(Input.mousePosition, transform.position, new Vector3Int(x, y, z));
	}

	void OnMouseDown()
	{
		GetComponent<AudioSource>().Play();
	}
	/*
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag != "EmptyBlock")
		{
			Debug.Log("Collide");
			transform.parent.gameObject.GetComponent<PlayerBlocksControl>().Resolve();
		}
	}
	*/
}
