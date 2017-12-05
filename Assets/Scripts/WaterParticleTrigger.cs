using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterParticleTrigger : MonoBehaviour{
	public GameObject waterPariticlePF;
	private bool isParticleTriggered;

	void Start(){
		isParticleTriggered = false;
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag("Water")){
			if ((gameObject.CompareTag("EmptyBlock") || gameObject.CompareTag("BridgeWater")) && other.gameObject.GetComponent<WaterBehavior>().isAwake){
				if (!isParticleTriggered) {
					isParticleTriggered = true;
					GameObject particle = (GameObject)Instantiate(waterPariticlePF);
					particle.transform.parent = gameObject.transform;
					particle.transform.localPosition = new Vector3 (0, 0.3f, 0);
				}
			}
		}
	}

}

