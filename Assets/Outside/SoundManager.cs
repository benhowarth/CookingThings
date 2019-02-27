using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
	public GameObject soundObj;
	// Use this for initialization
	void Start () {
	}

	
	// Update is called once per frame
	void Update () {
	}

	public void newSound(Transform soundTransform){
		GameObject newSound=Instantiate(soundObj,soundTransform.position,Quaternion.Euler (0,0,0));
	}

	public void newSound(Transform soundTransform,float maxRad,float maxTimer){
		GameObject newSound=Instantiate(soundObj,soundTransform.position,Quaternion.Euler (0,0,0));
		newSound.GetComponent<Sound>().radiusMax=maxRad;
		newSound.GetComponent<Sound>().timerMax=maxTimer;
	}
}
