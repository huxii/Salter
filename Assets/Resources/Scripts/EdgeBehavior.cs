using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Water")) 
		{
			if (!other.gameObject.GetComponent<WaterBehavior>().canCrossEdge)
			{
				other.gameObject.GetComponent<WaterBehavior>().Stop();
			}
		}
	}
}
