using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSingleBlockBehavior : MonoBehaviour 
{
	void OnMouseDrag()
	{
		transform.parent.gameObject.GetComponent<PlayerBlocksControl>().Drag(Input.mousePosition, transform.position, transform.localPosition);
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
