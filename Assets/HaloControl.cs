using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HaloControl : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void Hit(float scale)
	{
		transform.localScale = new Vector3(0, 0, 0);
		transform.DOScale(scale, 1f);
		GetComponent<MeshRenderer>().material.DOFade(0.0f, 1f);	

		StartCoroutine(DelayToDestroy(1f));
	}

	public IEnumerator DelayToDestroy(float delaySeconds)
	{
		yield return new WaitForSeconds(delaySeconds);
		Destroy(gameObject);
	}
}
