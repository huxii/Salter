using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
	public GameObject manager;
	public bool interactable;

	Image image;
	RectTransform rect;
	float originalScale;

	// Use this for initialization
	void Start()
	{
		interactable = true;
		image = gameObject.GetComponent<Image>();
		rect = gameObject.GetComponent<RectTransform>();
		originalScale = rect.localScale.x;
	}
	
	// Update is called once per frame
	void Update()
	{
		
	}
		
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (interactable)
		{
			image.DOColor(new Color(1.0f, 1.0f, 1.0f, 1.0f), 0.5f).SetEase(Ease.InOutCubic);
			rect.DOScale(originalScale * 1.2f, 0.5f).SetEase(Ease.InOutCubic);

			if (transform.parent && transform.parent.CompareTag("Buttons"))
			{
				manager.SendMessage("ButtonEnter");
			}
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (interactable)
		{
			image.DOColor(new Color(0.8f, 0.8f, 0.8f, 0.8f), 0.5f).SetEase(Ease.InOutCubic);
			rect.DOScale(originalScale, 0.5f).SetEase(Ease.InOutCubic);

			if (transform.parent && transform.parent.CompareTag("Buttons"))
			{
				manager.SendMessage("ButtonExit");
			}
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (interactable)
		{
			image.DOFade(0f, 1.5f).SetEase(Ease.InOutCubic);
			rect.DOScale(originalScale * 5.0f, 1.5f).SetEase(Ease.InOutCubic);

			manager.SendMessage("ButtonDown");
		}
	}
}
