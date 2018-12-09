using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPickup : MonoBehaviour {
	private bool toolTipOn=false;
	private Camera cam;
	public IngredientInfo info;
	// Use this for initialization
	void Start () {
		cam = Camera.main;
		toolTipOn=false;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 mPos = Input.mousePosition;
		RaycastHit hit;
		Ray camRay = cam.ScreenPointToRay (mPos);
		if(Physics.Raycast(camRay, out hit,Mathf.Infinity)){
			if(hit.transform==transform){
				toolTipOn=true;
			}else{
				toolTipOn=false;
			}
		}
	}

	public void Pickup(){
		Debug.Log ("Picked Up "+info.name+"!");
		GameObject pm=GameObject.Find ("PickupManager");
		pm.GetComponent<PickupManager>().Pickup(info.id);
		Destroy (transform.gameObject);
	}

	void onCollisionEnter(Collision col){
		/*Debug.Log (col);
		if (col.gameObject.name == "Player") {
			Destroy (gameObject);
		}*/
	}
	void OnGUI(){
		if (toolTipOn) {
			Vector3 mousePos=Input.mousePosition;
			GUI.Label (new Rect(mousePos.x,mousePos.y,300,100),info.name+"\nEnergy:"+info.energy+"\nVitamins:"+info.vitamins+"\nDisease:"+info.vitamins);
		}
	}
}
