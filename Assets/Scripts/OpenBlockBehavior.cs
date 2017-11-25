using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenBlockBehavior : MonoBehaviour {
	public bool isEmpty;
	GameObject blockingWaterGO;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (isEmpty && blockingWaterGO!=null) {
			blockingWaterGO.GetComponent<WaterBehavior> ().Resume ();
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Obstacle")) {
			gameObject.tag = "ObstacleBlock";
			isEmpty = false;
		}

		if (!isEmpty && other.gameObject.CompareTag ("Water")) {
			if (other.gameObject.GetComponent<WaterBehavior> ().isAwake) {
				Debug.Log (gameObject.name + " is blocking " + other.gameObject.name);
				other.gameObject.GetComponent<WaterBehavior> ().Pause ();
				blockingWaterGO = other.gameObject;
			}
		}


	}

	void OnTriggerStay(Collider other){ //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!Not working
		if (isEmpty && other.gameObject.CompareTag ("Water")) {
			if (other.gameObject.GetComponent<WaterBehavior> ().isAwake) {
				gameObject.tag = "WaterBlock";
			}
		}
	}

	void OnTriggerExit(Collider other){
		if (other.gameObject.CompareTag ("Obstacle")) {
			gameObject.tag = "EmptyBlock";
			isEmpty = true;
		}

	}
}
