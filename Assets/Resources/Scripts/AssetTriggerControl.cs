using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetTriggerControl : MonoBehaviour
{
	public bool isActive = true;
	public bool randomSpeed = false;
	public bool shouldBeTriggered = true;

	Animator animator;
	bool watered;
	float speed;

	// Use this for initialization
	void Start()
	{
		animator = GetComponent<Animator>();
		watered = false;
		animator.SetBool("Watered", watered);

		if (randomSpeed)
		{
			speed = Random.Range(0.5f, 0.7f);
			animator.speed = speed;
		}
	}

	// Update is called once per frame
	void Update()
	{
		//if (watered != shouldBeTriggered)
	}

	void OnTriggerEnter(Collider other)
	{
		if (isActive)
		{
			if (other.CompareTag("Water"))
			{
				if (other.gameObject.GetComponent<WaterBehavior>() && other.gameObject.GetComponent<WaterBehavior>().isAwake)
				{
					if (!watered)
					{
						watered = true;
						animator.SetBool("Watered", watered);
					}
				}
			}
		}
	}
}
