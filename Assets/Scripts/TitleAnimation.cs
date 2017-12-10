using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TitleAnimation : MonoBehaviour
{
	public GameObject[] levelBtn;
	public GameObject exitBtn;
	public GameObject leftDash;
	public GameObject rightDash;

	// Use this for initialization
	void Start()
	{
		if (Application.platform == RuntimePlatform.WebGLPlayer)
		{
			exitBtn.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update()
	{
		
	}

	public void ButtonEnter()
	{
		leftDash.GetComponent<RectTransform>().DOScaleX(0.8f, 0.5f).SetEase(Ease.InOutCubic);
		rightDash.GetComponent<RectTransform>().DOScaleX(0.8f, 0.5f).SetEase(Ease.InOutCubic);
	}

	public void ButtonExit()
	{
		leftDash.GetComponent<RectTransform>().DOScaleX(1f, 0.5f).SetEase(Ease.InOutCubic);
		rightDash.GetComponent<RectTransform>().DOScaleX(1f, 0.5f).SetEase(Ease.InOutCubic);
	}

	public void ButtonDown()
	{
		exitBtn.GetComponent<ButtonAnimation>().interactable = false;
		foreach (GameObject btn in levelBtn)
		{
			btn.GetComponent<ButtonAnimation>().interactable = false;
		}

		leftDash.GetComponent<RectTransform>().DOSizeDelta(new Vector2(150f, 0f), 1.5f).SetEase(Ease.InOutCubic);
		rightDash.GetComponent<RectTransform>().DOSizeDelta(new Vector2(150f, 0f), 1.5f).SetEase(Ease.InOutCubic);

		StartCoroutine(DelayToFade(0.5f));
	}

	public IEnumerator DelayToFade(float delaySeconds)
	{
		yield return new WaitForSeconds(delaySeconds);

		exitBtn.GetComponent<Image>().DOFade(0f, 1f).SetEase(Ease.InOutCubic);
		foreach (GameObject btn in levelBtn)
		{
			btn.GetComponent<Image>().DOFade(0f, 1f).SetEase(Ease.InOutCubic);
		}
		leftDash.GetComponent<Image>().DOFade(0f, 1f).SetEase(Ease.InOutCubic);
		rightDash.GetComponent<Image>().DOFade(0f, 1f).SetEase(Ease.InOutCubic);
	}
}
