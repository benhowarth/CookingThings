using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BzKovSoft.ObjectSlicerSamples;

public class toggleSlicer : MonoBehaviour {
	private SampleMouseSlicer ms;
	// Use this for initialization
	void Start () {
		ms = transform.GetComponent<SampleMouseSlicer>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("q")) {
			ms = transform.GetComponent<SampleMouseSlicer>();
			if(ms.enabled){
				ms.enabled=false;
			}else{
				ms.enabled=true;
			}
		}
	}
	void OnGUI()
	{
		string drawText="Knife: Disabled";
		if (ms.enabled) {
			drawText="Knife: Enabled";
			GUI.
		}
		GUI.Label(new Rect(Screen.width-100, 10, 100, 100), drawText);
	}
}
