using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
	private Vector3 screenPoint;
	private Vector3 offset;

	void OnMouseDown()
	{
	}

	void OnMouseDrag()
	{
		RaycastHit hitInfo;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hitInfo))
		{
			if (hitInfo.collider.gameObject.tag == "EmptyBlock")
			{
				Debug.Log(true);
				transform.position = hitInfo.collider.gameObject.transform.position;
			}
			else
			{
				Debug.Log(false);
			}
		}		
	}

	/*
	void OnMouseDown()
	{
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
 
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
 
	}

	void OnMouseDrag()
	{
		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
 
		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
		transform.position = curPosition;
 
	}
	*/

	/*
	// Use this for initialization
	void Start()
	{
		
	}
	
	// Update is called once per frame
	void Update()
	{

	}

	void OnMouseDrag()
	{
		Vector3 mousePos = Input.mousePosition;
		Vector3 targetPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 5.0f));
		transform.position = targetPos;
	}
	*/
}
