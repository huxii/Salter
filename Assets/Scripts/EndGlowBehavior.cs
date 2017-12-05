using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGlowBehavior : MonoBehaviour {
	public float glowSpeed;
	Material m_Material;
	Color glowColor;
	float opacity;
	bool isGlowing;
	// Use this for initialization
	void Start () {
		isGlowing = false;
		m_Material = GetComponent<Renderer>().material;
		glowColor = m_Material.color;
		opacity = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (isGlowing){
			if (opacity < 1) {
				opacity += Time.deltaTime*glowSpeed/100;
			}
			glowColor.a = opacity;
			m_Material.color = glowColor;
		}
	}

	public void Glow(){
		isGlowing = true;
	}

}
