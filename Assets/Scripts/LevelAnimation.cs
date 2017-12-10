using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelAnimation : MonoBehaviour
{
	public GameObject[] levelBtn;

	// Use this for initialization
	void Start()
	{
		
	}
	
	// Update is called once per frame
	void Update()
	{
		
	}

	public void ButtonEnter()
	{
	}

	public void ButtonExit()
	{
	}

	public void ButtonDown()
	{
		foreach (GameObject btn in levelBtn)
		{
			btn.GetComponent<ButtonAnimation>().interactable = false;
		}
	}
}
