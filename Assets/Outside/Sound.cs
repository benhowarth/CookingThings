using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour {
	
	
	public float radiusMax=10.0f;
	public float timerMax=3.0f;
	public float timer;
	public float radius;

	// Use this for initialization
	void Start () {
		timer = 0.0f;
		radius = 0.0f;
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (timer >= timerMax) {
			Destroy (gameObject);
		}else{
			radius = (timer / timerMax) * radiusMax;
			transform.localScale=new Vector3(radius,radius,radius);
			timer=timer+Time.deltaTime;
		}
	}
}
