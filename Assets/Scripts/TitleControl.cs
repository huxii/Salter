using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleControl : SceneLoader
{
	public Transform mainCam;
	public Transform tweenCam;

	// Use this for initialization
	void Start()
	{
		StartLevel();
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void LoadLevel(int level)
	{
		TweenCamera(mainCam, tweenCam);
		LoadLevel(level);
	}

	public void ExitGame()
	{
		Exit();
	}
}
