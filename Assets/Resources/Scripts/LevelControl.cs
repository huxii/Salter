using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LevelControl : SceneLoader
{
	public SpriteRenderer blank;
	public Transform mainCam;
	public Transform tweenCam;

	GameObject [] audios;

	// Use this for initialization
	void Start()
	{
		StartLevel(blank);
		TweenCamera(mainCam, tweenCam);

		audios = GameObject.FindGameObjectsWithTag("Sound");
	}
	
	// Update is called once per frame
	void Update()
	{
		
	}

	public void RestartLevel()
	{
		LoadLevel(blank, SceneManager.GetActiveScene().buildIndex, 0, 1f);
		for (int i = 0; i < audios.Length; ++i)
		{
			audios[i].GetComponent<AudioSource>().DOFade(0f, 1f);
		}
	}

	public void BackToTitle()
	{
		LoadLevel(blank, 0, 0, 1f);
		for (int i = 0; i < audios.Length; ++i)
		{
			audios[i].GetComponent<AudioSource>().DOFade(0f, 1f);
		}
	}
}
