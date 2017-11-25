using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTrigger : MonoBehaviour {
	private bool isTriggered;
	public GameObject[] waters;
	// Use this for initialization
	void Start () {
		isTriggered = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		foreach (GameObject water in waters) {
			if (water == other.gameObject) {
				return;
			}
		}
		if (!isTriggered && other.gameObject.CompareTag ("Water")) {
			isTriggered = true;
			Debug.Log (other.gameObject.name + " reaches " + gameObject.name);
			//run next water
			foreach (GameObject water in waters) {
				if (water != null) {
					water.GetComponent<WaterBehavior> ().Run ();
				}
			}
			//stop previous water
			other.gameObject.GetComponent<WaterBehavior>().Stop();
		}
	}
}
