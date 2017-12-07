using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlowControl : MonoBehaviour
{

	// Use this for initialization
	void Start()
	{
		Flow();
	}
	
	// Update is called once per frame
	void Update()
	{
		
	}

	void Flow()
	{
		transform.localScale = new Vector3(0.0f, transform.localScale.y, transform.localScale.z);
		transform.DOScaleX(1.0f, 1.0f);
	}
}
