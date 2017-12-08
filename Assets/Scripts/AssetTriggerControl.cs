using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetTriggerControl : MonoBehaviour
{
	public bool isActive = true;
	public bool randomSpeed = false;
	public bool shouldBeTriggered = true;
	public bool watered;

	Animator animator;
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
	}

	void OnTriggerEnter(Collider other)
	{
		if (isActive)
		{
			if (other.CompareTag("Water"))
			{
				if (!watered)
				{
					watered = true;
					animator.SetBool("Watered", watered);

					if (!shouldBeTriggered)
					{
						GameObject.Find("Level Manager").SendMessage("LevelFail");
					}
				}
			}
		}
	}
}
