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

		if (Application.platform == RuntimePlatform.WebGLPlayer)
		{
			GameObject exitBtn = GameObject.FindGameObjectWithTag("ExitButton");
			exitBtn.SetActive(false);
		}
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void Load(int level)
	{
		TweenCamera(mainCam, tweenCam);
		LoadLevel(level);
	}

	public void ExitGame()
	{
		Exit();
	}
}
