using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LevelControl : SceneLoader
{
	public SpriteRenderer blank;
	public Transform mainCam;
	public Transform levelStartCam;
	public Transform levelEndCam;

	// Use this for initialization
	void Start()
	{
		StartLevel(blank);
		TweenCamera(mainCam, levelStartCam);
	}
	
	// Update is called once per frame
	void Update()
	{
		
	}

	public void LevelComplete()
	{
		Debug.Log("Level complete");
		LevelComplete(mainCam, levelEndCam, blank, SceneManager.GetActiveScene().buildIndex, 3f, 3f);
	}

	public void RestartLevel()
	{
		LoadLevel(blank, SceneManager.GetActiveScene().buildIndex, 0, 1f);
	}

	public void BackToTitle()
	{
		LoadLevel(blank, 0, 0, 1f);
	}
}
