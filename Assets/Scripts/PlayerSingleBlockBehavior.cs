using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerSingleBlockBehavior : MonoBehaviour 
{
	public int x;
	public int y;
	public int z;

	void OnMouseOver()
	{
		transform.parent.gameObject.GetComponent<PlayerBlocksControl>().Hover();
	}

	void OnMouseExit()
	{
		transform.parent.gameObject.GetComponent<PlayerBlocksControl>().Leave();
	}

	void OnMouseDrag()
	{
		transform.parent.gameObject.GetComponent<PlayerBlocksControl>().Drag(Input.mousePosition, transform.position, new Vector3Int(x, y, z));
	}

	void OnMouseDown()
	{
		transform.parent.gameObject.GetComponent<PlayerBlocksControl>().Click(Input.mousePosition);
		GetComponent<AudioSource>().Play();
	}

	void OnMouseUp()
	{
		transform.parent.gameObject.GetComponent<PlayerBlocksControl>().Leave();
	}

	public void TweenEmission(Color color)
	{
		Material mat = gameObject.GetComponent<MeshRenderer>().material;
		mat.DOColor(color, "_EmissionColor", 1f);		
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
