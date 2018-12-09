using UnityEngine;
using System.Collections;

public class Hotplate : MonoBehaviour {

	public bool heatOn=false;
	public bool turnOnWithTouch = false;
	public Material heatOffMat;
	public Material heatOnMat;
	public float cookSpeed=0.4f;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown(){
		if (turnOnWithTouch) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit) && hit.transform == transform) {
				if (heatOn) {
					heatOn = false;
					GetComponent<Renderer> ().material = heatOffMat;

				} else {
					heatOn = true;
					GetComponent<Renderer> ().material = heatOnMat;
				}
			}
		}
	}
	void OnCollisionEnter(Collision col){
		if (heatOn) {
			if (col.gameObject.GetComponent<Cookable> ()) {
				col.gameObject.GetComponent<Cookable> ().beingCookedAmount = cookSpeed;
			}
		}
	}
	void OnCollisionStay(Collision col){
		if (heatOn) {
			if (col.gameObject.GetComponent<Cookable> ()) {
				col.gameObject.GetComponent<Cookable> ().beingCookedAmount = cookSpeed;
			}
		}
	}

	void OnCollisionExit(Collision col){
		if (col.gameObject.GetComponent<Cookable> ()) {
			col.gameObject.GetComponent<Cookable> ().beingCookedAmount = 0.0f;
		}
	}
}
