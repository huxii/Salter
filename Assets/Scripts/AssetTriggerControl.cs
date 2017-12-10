using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetTriggerControl : MonoBehaviour
{
	public bool isActive = true;
	public bool randomSpeed = false;
	public bool shouldBeTriggered = true;
	public bool watered;
	public GameObject haloPrefab;

	Animator animator;
	float speed;

	// Use this for initialization
	void Start()
	{
		watered = false;

		animator = GetComponent<Animator>();
		if (animator)
		{
			animator.SetBool("Watered", watered);

			if (randomSpeed)
			{
				speed = Random.Range(0.5f, 0.7f);
				animator.speed = speed;
			}
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

						GameObject halo = Instantiate(haloPrefab, transform);
						halo.GetComponent<HaloControl>().Hit(10f);
					}
				}
			}
		}
	}
}
