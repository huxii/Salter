﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LevelControl : SceneLoader
{
	public Transform mainCam;
	public Transform levelStartCam;
	public Transform levelEndCam;
	public GameObject endGlow;

	bool levelComplete;
	bool levelCompleted;

	// Use this for initialization
	void Start()
	{
		StartLevel();
		TweenCamera(mainCam, levelStartCam);

		levelComplete = false;
		levelCompleted = false;
	}
	
	// Update is called once per frame
	void Update()
	{
		if (levelComplete && !levelCompleted)
		{
			bool canComplete = true;
			GameObject[] assetTriggers = GameObject.FindGameObjectsWithTag("AssetTrigger");
			for (int i = 0; i < assetTriggers.Length; ++i)
			{
				if (assetTriggers[i].GetComponent<AssetTriggerControl>().watered !=
				    assetTriggers[i].GetComponent<AssetTriggerControl>().shouldBeTriggered)
				{
					canComplete = false;
					break;
				}
			}

			if (canComplete)
			{
				levelCompleted = true;
				endGlow.GetComponent<EndGlowBehavior>().Glow();
				GameObject.FindGameObjectWithTag("ClearMusic").GetComponent<AudioSource>().Play();
				LevelComplete(mainCam, levelEndCam, SceneManager.GetActiveScene().buildIndex, 3f, 3f);		
			}
		}
	}

	public void LevelComplete()
	{
		//Debug.Log("Level complete");

		levelComplete = true;
	}

	public void LevelFail()
	{
		//Debug.Log("Level fail");

		mainCam.DOShakePosition(0.5f, 0.2f, 50);
		mainCam.DOShakeRotation(0.5f, 2, 50);
		GameObject.FindGameObjectWithTag("FailMusic").GetComponent<AudioSource>().Play();
		StartCoroutine(DelayToRestart(2f));
	}

	public void RestartLevel()
	{
		LoadLevel(SceneManager.GetActiveScene().buildIndex, 0, 1f);
	}

	public void BackToTitle()
	{
		LoadLevel(0, 0, 1f);
	}

	public IEnumerator DelayToRestart(float delaySeconds)
	{
		yield return new WaitForSeconds(delaySeconds);
		RestartLevel();
	}
}
