using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pan : MonoBehaviour {
	
	public bool heatOn=false;
	public Material heatOffMat;
	public Material heatOnMat;
	public float cookSpeed=0.0f;
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (heatOn) {
			GetComponent<Renderer> ().material = heatOnMat;
			
		} else {
			GetComponent<Renderer> ().material = heatOffMat;
		}
	}
	
	void OnMouseDown(){
				
	}
	void OnCollisionEnter(Collision col){
		if (heatOn) {
			if (col.gameObject.GetComponent<Cookable> ()) {
				col.gameObject.GetComponent<Cookable> ().beingCookedAmount = cookSpeed;
			}
		} else {
			
			if (col.gameObject.GetComponent<Hotplate> ()) {
				if(col.gameObject.GetComponent<Hotplate> ().heatOn){
					cookSpeed=col.gameObject.GetComponent<Hotplate> ().cookSpeed;
					heatOn=true;
				}else{
					heatOn=false;
					cookSpeed=0.0f;
				}
			}
		}
	}
	void OnCollisionStay(Collision col){
		if (heatOn) {
			if (col.gameObject.GetComponent<Cookable> ()) {
				col.gameObject.GetComponent<Cookable> ().beingCookedAmount = cookSpeed;
			}
		} else {
			
			if (col.gameObject.GetComponent<Hotplate> ()) {
				if(col.gameObject.GetComponent<Hotplate> ().heatOn){
					cookSpeed=col.gameObject.GetComponent<Hotplate> ().cookSpeed;
					heatOn=true;
				}else{
					heatOn=false;
					cookSpeed=0.0f;

				}
			}
		}
	}
	
	void OnCollisionExit(Collision col){
		if (col.gameObject.GetComponent<Cookable> ()) {
			col.gameObject.GetComponent<Cookable> ().beingCookedAmount = 0.0f;
		}
		if (col.gameObject.GetComponent<Hotplate> ()) {
			heatOn=false;
			cookSpeed=0.0f;
		}
	}
}
