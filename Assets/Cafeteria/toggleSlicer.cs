using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BzKovSoft.ObjectSlicerSamples;

public class toggleSlicer : MonoBehaviour {
	private SampleMouseSlicer ms;
	public GameObject knifePlane;
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
				knifePlane.GetComponent<MeshRenderer>().enabled=false;
			}else{
				ms.enabled=true;
				knifePlane.GetComponent<MeshRenderer>().enabled=true;
			}
		}

		if (ms.enabled) {
			Vector3 mPos=Input.mousePosition;
			Ray camRay=Camera.main.ScreenPointToRay(mPos);
			RaycastHit hit;
			if(Physics.Raycast(camRay,out hit)){
				knifePlane.transform.position=new Vector3(hit.point.x,3.5f,hit.point.z);
			}
			if(Input.GetMouseButton(0)){
				//particle
			}
		}
	}
	void OnGUI()
	{
		string drawText="Knife: Disabled";
		if (ms.enabled) {
			drawText="Knife: Enabled";
			//GUI.
		}
		GUI.Label(new Rect(Screen.width-100, 10, 100, 100), drawText);
	}
}
