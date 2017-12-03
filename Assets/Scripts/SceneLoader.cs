using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneLoader : MonoBehaviour
{
	void FadeIn(SpriteRenderer blank, float duration = 1.0f)
	{
		blank.DOFade(0f, duration);
	}

	void FadeOut(SpriteRenderer blank, float duration = 1.0f)
	{
		blank.DOFade(1f, duration);
	}

	public void LoadLevel(SpriteRenderer blank, int level, float delayDuration = 2.0f, float fadeDuration = 1.0f)
	{ 
		StartCoroutine(DelayToLoadLevel(level, delayDuration + fadeDuration));
		StartCoroutine(DelayToFade(blank, delayDuration, fadeDuration));
	}

	public void StartLevel(SpriteRenderer blank)
	{
		FadeIn(blank, 3.0f);
	}

	public void TweenCamera(Transform mainCam, Transform tweenCam, float totalDuration = 3.0f)
	{
		mainCam.DOMove(tweenCam.position, totalDuration).SetEase(Ease.InOutCubic);
		mainCam.DORotate(tweenCam.rotation.eulerAngles, totalDuration).SetEase(Ease.InOutCubic);		
	}
		
	public void ExitGame()
	{
		Application.Quit();
	}

	public IEnumerator DelayToLoadLevel(int level, float delaySeconds)
	{
		yield return new WaitForSeconds(delaySeconds);
		Application.LoadLevel(level);
	}

	public IEnumerator DelayToFade(SpriteRenderer blank, float delaySeconds, float fadeDuration)
	{
		yield return new WaitForSeconds(delaySeconds);
		FadeOut(blank, fadeDuration);
	}
}
