using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrab : MonoBehaviour {

	//hand to parent grab point to if needed
	public Transform hand;
	//nearest grabbable object (or currently grabbed object if grabbed=true)
	public GameObject grabbableObj;
	//is playing holding something?
	public bool grabbed;
	//to enable or disable knife attack	
	public KnifeAttack knife;


	void Start () {
		//begin holding nothing
		grabbableObj = null;
		grabbed = false;
	}

	void TryToPickUpObject(){
		//if object to grab
		if(grabbableObj){
			//grab it
			grabbed=true;
			grabbableObj.gameObject.GetComponent<Collider>().isTrigger=true;
			grabbableObj.transform.position=transform.position;
			grabbableObj.gameObject.GetComponent<Rigidbody>().isKinematic=true;
			grabbableObj.gameObject.GetComponent<Rigidbody>().useGravity=false;
			grabbableObj.transform.SetParent(transform);
			//player can't attack when holding something
			knife.knifeEnabled=false;
		}
	
	}

	void DropCurrentObject(bool throwObject){
		//drop object and reenable for physics
		grabbed=false;
		transform.GetChild(0).gameObject.GetComponent<Rigidbody>().isKinematic=false;
		transform.GetChild(0).gameObject.GetComponent<Rigidbody>().useGravity=true;

		//if object is to be thrown, throw it HARD
		if(throwObject){
			transform.GetChild(0).gameObject.GetComponent<Rigidbody>().AddForce((transform.up*50f)+(transform.forward*1000f));
		}

		//complete dropping of object
		grabbableObj.gameObject.GetComponent<Collider>().isTrigger=false;
		transform.GetChild(0).SetParent(null);
		grabbableObj=null;

		//reenable knife attack
		knife.knifeEnabled=true;
	}



	void Update () {
		//transform.position = hand.position;

		//if player not holding something, check for nearby object for grabbin'
		if (!grabbed) {
			//update grabbableObj (get nearest grabbable obj, i.e. crate)
			Collider[] objNear = Physics.OverlapSphere (transform.position, 1.5f, LayerMask.GetMask ("Crate"));
			if (objNear.Length > 0) {
				grabbableObj = objNear [0].gameObject;
				for (int i = 0; i < objNear.Length; i++) {
					//if nearest crate is not smashed
					if (!objNear [i].gameObject.GetComponent<Crate> ().smashed) {
						if (Vector3.Distance (objNear [i].transform.position, transform.position) < Vector3.Distance (grabbableObj.transform.position, transform.position)) {
							grabbableObj = objNear [i].gameObject;
						}
					}
				}
			
			} else {
				//nothing nearby
				grabbableObj = null;
			}


			//if player wants to grab an objecct, do it (if there's an object to grab)
			if(Input.GetKeyDown(KeyCode.E)){
				TryToPickUpObject();
			}
		//if holding something
		}else{
			//if crate is smashed or player wants to drop object
			if(grabbableObj.GetComponent<Crate> ().smashed||Input.GetKeyDown(KeyCode.E)){
				DropCurrentObject(false);
			//or if mouse clicked, throw object like a badass
			}else if(Input.GetMouseButton(1)){
				DropCurrentObject(true);
			}
		}

	}
}
