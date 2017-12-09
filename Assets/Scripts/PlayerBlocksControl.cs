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

	GameObject gridManager;
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
		gridManager = GameObject.Find("GridManager");
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
			if (hitObj.CompareTag("GridBlock") && hitObj.transform.parent.gameObject == gridManager)
			{
				hitGrid = hitObj;
				break;
			}
		}

		if (hitGrid == null)
		{
			return;
		}

		GameObject startGrid = gridManager.GetComponent<GridControl>().GetGrid(worldPos);	
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
		gridManager.GetComponent<GridControl>().MoveToGrid(gameObject, startGridIdx - offset, hitGridIdx - offset, audio);
	}
}
