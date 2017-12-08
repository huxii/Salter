using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundTrigger : MonoBehaviour {
	private bool isPlaying;
	AudioSource audio;

	// Use this for initialization
	void Start () {
		isPlaying = false;
		audio = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		if(!isPlaying && other.gameObject.CompareTag("Water")){
			//if (other.gameObject.GetComponent<WaterBehavior> ().isAwake) {
				audio.Play ();
				isPlaying = true;
			//}
		}
	}
}
