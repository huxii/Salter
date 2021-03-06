﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerBlocksControl : MonoBehaviour
{
	public GameObject dragStoneParticle;
	public int width;
	public int length;
	public int height;
	public bool movable;
	public Color hoverColor;
	public Color DragColor;
	public GameObject haloPrefab;

	List<GameObject> blocks;
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

		StopMoving();
	}
		
	public void Hover()
	{
		if (!Input.GetMouseButton(0))
		{
			foreach (GameObject block in blocks)
			{
				block.GetComponent<PlayerSingleBlockBehavior>().TweenEmission(hoverColor);
			}
		}
	}

	public void Leave()
	{
		foreach (GameObject block in blocks)
		{
			block.GetComponent<PlayerSingleBlockBehavior>().TweenEmission(new Color(0, 0, 0, 1));
		}
	}

	public void Click(Vector3 mousePos)
	{
		/*
		GameObject newParticle = Instantiate(
			clickStoneParticle, 
			Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, gameObject.transform.position.z)),
			new Quaternion()
		);
		*/

		GameObject halo = Instantiate(haloPrefab, transform);
		halo.transform.localPosition = new Vector3((width - 1) * 0.5f, height * 0.5f, (length - 1) * 0.5f);
		halo.GetComponent<HaloControl>().Hit(0.5f);
	}

	public void Drag(Vector3 mousePos, Vector3 worldPos, Vector3Int offset)
	{
		if (!movable)
		{
			return;
		}

		foreach (GameObject block in blocks)
		{
			block.GetComponent<PlayerSingleBlockBehavior>().TweenEmission(DragColor);
		}

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
		gridManager.GetComponent<GridControl>().MoveToGrid(gameObject, startGridIdx - offset, hitGridIdx - offset);
	}

	public void StartMoving()
	{
		movable = false;
		dragStoneParticle.GetComponent<ParticleSystem>().Play();
	}

	public void StopMoving()
	{
		movable = true;
		dragStoneParticle.GetComponent<ParticleSystem>().Stop();
	}
}
