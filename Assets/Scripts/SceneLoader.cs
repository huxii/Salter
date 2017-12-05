using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneLoader : MonoBehaviour
{
	void FadeIn(float duration = 1.0f)
	{
		GameObject blank = GameObject.FindGameObjectWithTag("Blank");
		Color tmpColor = blank.GetComponent<SpriteRenderer>().color;
		tmpColor.a = 1.0f;
		blank.GetComponent<SpriteRenderer>().color = tmpColor;
		blank.GetComponent<SpriteRenderer>().DOFade(0f, duration);

		GameObject [] audios = GameObject.FindGameObjectsWithTag("Sound");
		for (int i = 0; i < audios.Length; ++i)
		{
			AudioSource audio = audios[i].GetComponent<AudioSource>();
			if (audio && audio.playOnAwake)
			{
				audio.volume = 0f;
				audio.DOFade(1f, duration);
			}
		}
	}

	void FadeOut(float duration = 1.0f)
	{
		GameObject blank = GameObject.FindGameObjectWithTag("Blank");
		Color tmpColor = blank.GetComponent<SpriteRenderer>().color;
		tmpColor.a = 0.0f;
		blank.GetComponent<SpriteRenderer>().color = tmpColor;
		blank.GetComponent<SpriteRenderer>().DOFade(1f, duration);

		GameObject [] audios = GameObject.FindGameObjectsWithTag("Sound");
		for (int i = 0; i < audios.Length; ++i)
		{
			audios[i].GetComponent<AudioSource>().DOFade(0f, duration);
		}
	}

	public void LoadLevel(int level, float delayDuration = 2.0f, float fadeDuration = 1.0f)
	{ 
		StartCoroutine(DelayToLoadLevel(level, delayDuration + fadeDuration));
		StartCoroutine(DelayToFade(delayDuration, fadeDuration));
	}

	public void LevelComplete(Transform mainCam, Transform tweenCam, int level, float delayDuration = 2.0f, float fadeDuration = 1.0f)
	{ 
		++level;
		if (level >= 3)
		{
			level -= 3;
		}
		
		StartCoroutine(DelayToLoadLevel(level, delayDuration + fadeDuration));
		StartCoroutine(DelayToTween(mainCam, tweenCam, delayDuration - 1, fadeDuration));
		StartCoroutine(DelayToFade(delayDuration, fadeDuration));
	}

	public void StartLevel(float duration = 3.0f)
	{
		FadeIn(duration);
	}

	public void TweenCamera(Transform mainCam, Transform tweenCam, float totalDuration = 3.0f)
	{
		mainCam.DOMove(tweenCam.position, totalDuration).SetEase(Ease.InOutCubic);
		mainCam.DORotateQuaternion(tweenCam.rotation, totalDuration).SetEase(Ease.InOutCubic);		
	}
		
	public void Exit()
	{
		FadeOut();
		StartCoroutine(DelayToQuit(1f));
	}

	public IEnumerator DelayToLoadLevel(int level, float delaySeconds)
	{
		yield return new WaitForSeconds(delaySeconds);
		Application.LoadLevel(level);
	}

	public IEnumerator DelayToTween(Transform mainCam, Transform tweenCam, float delaySeconds, float fadeDuration)
	{
		yield return new WaitForSeconds(delaySeconds);
		TweenCamera(mainCam, tweenCam, fadeDuration);
	}

	public IEnumerator DelayToFade(float delaySeconds, float fadeDuration)
	{
		yield return new WaitForSeconds(delaySeconds);
		FadeOut(fadeDuration);
	}

	public IEnumerator DelayToQuit(float delaySeconds)
	{
		yield return new WaitForSeconds(delaySeconds);
		Application.Quit();
	}
}
