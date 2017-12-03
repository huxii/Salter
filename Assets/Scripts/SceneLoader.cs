using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	public void LoadScene(int level)
	{ 
		Application.LoadLevel(level);
	}

	public void RestartGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name); // loads current scene
	}

	public void BackToTitle()
	{
		SceneManager.LoadScene(0);
	}

	public void ExitGame()
	{
		Application.Quit();
	}
}
