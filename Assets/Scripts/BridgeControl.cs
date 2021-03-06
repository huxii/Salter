﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BridgeControl : MonoBehaviour
{
	public float fallingSpeed = 0.1f;
	public bool leanDown = false;

	AudioSource audio;
	bool isTriggered;

	// Use this for initialization
	void Start()
	{
		audio = GetComponent<AudioSource>();

		isTriggered = false;
		if (!leanDown)
		{
			transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, -90.0f);
		}
		else
		{
			transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0.0f);
		}
	}
	
	// Update is called once per frame
	void Update()
	{
		if (isTriggered)
		{
			Vector3 r = transform.localEulerAngles;
			transform.DORotate(new Vector3(r.x, r.y, 0f), 1f).SetEase(Ease.OutCubic);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		//Debug.Log("Bridge triggered");
		if (!isTriggered)
		{
			if (other.gameObject.CompareTag("Obstacle"))
			{
				isTriggered = true;
				if (audio)
				{
					audio.Play();
				}
			}
		}
	}
}
