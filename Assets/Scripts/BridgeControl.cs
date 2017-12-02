using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeControl : MonoBehaviour
{
	public float fallingSpeed = 0.1f;

	bool isTriggered;

	// Use this for initialization
	void Start()
	{
		isTriggered = false;
		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0.0f);
	}
	
	// Update is called once per frame
	void Update()
	{
		if (isTriggered)
		{
			float rz = transform.localEulerAngles.z;
			if (rz < 90.0f)
			{
				rz += fallingSpeed * Time.deltaTime * 100.0f;
				transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, rz);
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		//Debug.Log("Bridge triggered");
		if (!isTriggered)
		{
			if (other.gameObject.CompareTag("Obstacle"))
			{
				isTriggered = true;
			}
		}
	}
}
