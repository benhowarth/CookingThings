using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleDestroyAfterPlay : MonoBehaviour {

	void DestroyParticle(){
		Destroy (gameObject);
	}
	// Use this for initialization
	void Start () {
		Invoke ("DestroyParticle", GetComponent<ParticleSystem> ().main.duration);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
