using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlocksControl : MonoBehaviour
{
	public List<GameObject> blocks;

	void Start()
	{
		blocks.Clear();
		foreach (Transform child in transform)
		{
			if (child.gameObject.CompareTag("Obstacle"))
			{
				blocks.Add(child.gameObject);
			}
		}

		RaycastHit[] hits;
		hits = Physics.RaycastAll(new Vector3(-0.5f, 0.0f, 0.0f) - transform.forward, transform.forward, 1.0f);
		foreach (RaycastHit hit in hits)
		{
			Debug.Log(hit.collider.gameObject.name);
		}
	
	}
		
	bool CollisionTest(Vector3 newPos)
	{		
		foreach (GameObject block in blocks)
		{
			Vector3 pos = newPos + block.transform.localPosition;

			RaycastHit[] hits;
			hits = Physics.RaycastAll(pos - transform.forward, transform.forward, 1.0f);
			//Debug.Log(hits.Length + " " + pos);
			foreach (RaycastHit hit in hits)
			{
				GameObject hitObj = hit.collider.gameObject;
				//Debug.Log(hitObj.name);
				//hitObj.transform.position = new Vector3(hitObj.transform.position.x, 3.0f, hitObj.transform.position.z);

				if (! (hitObj.tag == "EmptyBlock" || hitObj.tag == "Water" || hitObj.tag == "WaterTrigger" ||
					(hitObj.tag == "Obstacle" && hitObj.transform.parent == block.transform.parent)) )
				{
					Debug.Log("Collide" + hitObj.name + " " + hitObj.transform.parent.name + " " + block.transform.parent.name);
					return true;
				}
			}
		}
		return false;
	}

	public void Drag(Vector3 mousePos, Vector3 worldPos, Vector3 localPos)
	{
		RaycastHit hitInfo;
		Ray ray = Camera.main.ScreenPointToRay(mousePos);
		Vector3 newPos = transform.position;
		if (Physics.Raycast(ray, out hitInfo))
		{
			if (hitInfo.collider.gameObject.tag == "EmptyBlock")
			{
				//prePos = transform.position;

				Vector3 tmpPos = hitInfo.collider.gameObject.transform.position - localPos;
				newPos = new Vector3(
					tmpPos.x,
					transform.position.y,
					tmpPos.z
				);	


				Vector3 tmpDir = newPos - transform.position;
				if (tmpDir.magnitude > 1.1f)
				{
					if (Mathf.Abs(transform.position.x - newPos.x) < 0.01f || Mathf.Abs(transform.position.z - newPos.z) < 0.01f)
					{
						tmpDir.Normalize();
						newPos = transform.position + tmpDir;
					}
					else
					{
						return;
					}
				}

				if (!CollisionTest(new Vector3(newPos.x, 0.0f, newPos.z)))
				{
					Debug.Log("Moved");
					transform.position = newPos;
				}
			}
		}


		/*
		Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane));
		Vector3 dir = worldMousePos - worldPos;
		bool collided = false;

		foreach (GameObject block in blocks)
		{
			Vector3 curWorldPos = block.transform.position;
			Vector3 curWorldMousePos = curWorldPos + dir;
			Vector3 curMousePos = Camera.main.WorldToScreenPoint(curWorldMousePos);

			RaycastHit hitInfo;
			Ray ray = Camera.main.ScreenPointToRay(curMousePos);
			if (Physics.Raycast(ray, out hitInfo))
			{
				if (hitInfo.collider.gameObject.tag != "EmptyBlock")
				{
					Debug.Log(block.gameObject.name + " Colliding "+hitInfo.collider.gameObject.name);

					collided = true;
					break;
				}
			}
		}

		if (!collided)
		{
			RaycastHit hitInfo;
			Ray ray = Camera.main.ScreenPointToRay(mousePos);
			if (Physics.Raycast(ray, out hitInfo))
			{
				Vector3 tmpPos = hitInfo.collider.gameObject.transform.position + localPos;
				transform.position = new Vector3(
					tmpPos.x,
					transform.position.y,
					tmpPos.z
				);	
			}
		}
		*/
		/*
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hitInfo))
		{
			if (hitInfo.collider.gameObject.tag == "EmptyBlock")
			{
				Debug.Log("Moving "+hitInfo.collider.gameObject.name);
				transform.position = new Vector3(
					hitInfo.collider.gameObject.transform.position.x,
					transform.position.y,
					hitInfo.collider.gameObject.transform.position.z
				);
			}
		}
		*/
	}
	/*
	public void Resolve()
	{
		transform.position = prePos;
	}
	*/
}
