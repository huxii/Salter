using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerBlocksControl : MonoBehaviour
{
	private List<GameObject> blocks;
	public int width;
	public int length;
	public int height;

	GameObject gameManager;
	AudioSource audio;

	void Start()
	{
		blocks = new List<GameObject>();
		foreach (Transform child in transform)
		{
			if (child.gameObject.CompareTag("Obstacle"))
			{
				blocks.Add(child.gameObject);
			}
		}
		audio = GetComponent<AudioSource>();
		gameManager = GameObject.Find("Game Manager");
	}

	bool CollisionTest(Vector3 newPos)
	{		
		RaycastHit[] hits;
		hits = Physics.RaycastAll(transform.position, newPos - transform.position, (newPos - transform.position).magnitude);

		foreach (RaycastHit hit in hits)
		{
			GameObject hitObj = hit.collider.gameObject;
			if (!hitObj.CompareTag("GridBlock") && !hitObj.CompareTag("Sound"))
			{
				return true;
			}
		}

		return false;

		/*
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

				if (!(hitObj.tag == "EmptyBlock" || hitObj.tag == "Water" || hitObj.tag == "WaterTrigger" || hitObj.tag == "ObstacleBlock" ||
				    hitObj.tag == "Bridge" || hitObj.tag == "Sound" ||
				    (hitObj.tag == "Obstacle" && hitObj.transform.parent == block.transform.parent)))
				{
					Debug.Log("Collide" + hitObj.name + " " + block.name + " " + hitObj.transform.parent.name + " " + block.transform.parent.name);
					return true;
				}
			}
		}
		return false;
		*/
	}
		
	public void Drag(Vector3 mousePos, Vector3 worldPos, Vector3Int offset)
	{
		Ray ray = Camera.main.ScreenPointToRay(mousePos);
		RaycastHit[] hits;
		hits = Physics.RaycastAll(ray);

		GameObject hitGrid = null;
		foreach (RaycastHit hit in hits)
		{
			GameObject hitObj = hit.collider.gameObject;
			if (hitObj.CompareTag("GridBlock"))
			{
				hitGrid = hitObj;
				break;
			}
		}

		if (hitGrid == null)
		{
			return;
		}

		GameObject startGrid = gameManager.GetComponent<GridControl>().GetGrid(worldPos);	
		Vector3Int startGridIdx = new Vector3Int(
			startGrid.GetComponent<GridBlockControl>().x,
			startGrid.GetComponent<GridBlockControl>().y,
			startGrid.GetComponent<GridBlockControl>().z
		);

		Vector3Int hitGridIdx = new Vector3Int(
			hitGrid.GetComponent<GridBlockControl>().x,
			hitGrid.GetComponent<GridBlockControl>().y,
			hitGrid.GetComponent<GridBlockControl>().z
		);
		//Debug.Log(startGridIdx + hitGridIdx);
		gameManager.GetComponent<GridControl>().MoveToGrid(gameObject, startGridIdx - offset, hitGridIdx - offset);


		/*
		RaycastHit hitInfo;
		Ray ray = Camera.main.ScreenPointToRay(mousePos);
		Vector3 newPos = transform.position;

		if (Physics.Raycast(ray, out hitInfo))
		{
			//if (hitInfo.collider.gameObject.tag == "GridBlock")
			//{
				Debug.Log("...");
				//if (hitInfo.collider.gameObject.GetComponent<GridBlockControl>().collidedObj == 0)
				//{
					newPos = hitInfo.collider.gameObject.transform.position;

					if (!CollisionTest(newPos))
					{
						Debug.Log("Moved");

						transform.DOMove(newPos, 0.2f).SetEase(Ease.InOutCubic);
						movable = false;
						StartCoroutine(DelayToMove(0.2f));
					}
				//}
			//}
			/*
			//if (hitInfo.collider.gameObject.tag == "EmptyBlock")
			//{
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
			else
			if (tmpDir.magnitude < 0.1f)
			{
				return;
			}

			if (!CollisionTest(new Vector3(newPos.x, 0.0f, newPos.z)))
			{
				Debug.Log("Moved");
					
				//transform.position = newPos;
				audio.Play();
				transform.DOMove(newPos, 0.1f).SetEase(Ease.InOutCubic);

				movable = false;
				StartCoroutine(DelayToMove(0.1f));
			}
			
		}*/
	}
}
