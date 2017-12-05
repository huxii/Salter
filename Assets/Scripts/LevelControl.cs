using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LevelControl : SceneLoader
{
	public Transform mainCam;
	public Transform levelStartCam;
	public Transform levelEndCam;

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
				LevelComplete(mainCam, levelEndCam, SceneManager.GetActiveScene().buildIndex, 3f, 3f);		
			}
		}
	}

	public void LevelComplete()
	{
		Debug.Log("Level complete");

		levelComplete = true;
	}

	public void LevelFail()
	{
		Debug.Log("Level fail");
		RestartLevel();
	}

	public void RestartLevel()
	{
		LoadLevel(SceneManager.GetActiveScene().buildIndex, 0, 1f);
	}

	public void BackToTitle()
	{
		LoadLevel(0, 0, 1f);
	}
}
