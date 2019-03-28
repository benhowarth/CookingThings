using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour {

	public GameObject food;
	public bool smashed;
	public GameObject SoundManager;
	public PickupManager pm;
	//how quickly crate opens
	public float openSpeed=0.1f;


	void Start () {
		pm = GameObject.Find ("PickupManager").GetComponent<PickupManager> ();
		smashed = false;
	}

	//if hit against anything hard enough, smash based on magnitude of hit
	void OnCollisionEnter(Collision col){
		if (col.impulse.magnitude>10f) {
			Smash(col.gameObject.transform.position,col.impulse.magnitude);
		}
	}

	//if hit with player's knife, smash impressively
	void OnTriggerEnter(Collider col){
		if (col.gameObject.name=="Knife") {
			Smash(col.gameObject.transform.position,20f);
		}
	}

	//smash with parameters for explosion physics effect
	public void Smash(Vector3 explosionCentre,float force){
		//set crate to smashed
		smashed = true;
		//stop parent collider weirdness as box is now in pieces
		GetComponent<Collider> ().enabled = false;
		GetComponent<Rigidbody>().useGravity=false;

		
		//loop through children
		int children=transform.childCount;
		for(int i=0;i<children;i++){
			//move pieces away from centre of explosion with given force
			Transform piece=transform.GetChild(i);
			Vector3 knifeHit=piece.position-explosionCentre;
			piece.GetComponent<Collider>().enabled=true;
			piece.GetComponent<Rigidbody>().useGravity=true;
			piece.GetComponent<Rigidbody>().isKinematic=false;
			piece.GetComponent<Rigidbody>().AddForce(knifeHit*force);
			//set child to be destroyed after a period of time
			piece.GetComponent<DestroyAfter>().ActivateDestructionTimer();
		}

		//remove all children from crate parent
		foreach (Transform child in transform)
		{
			child.parent = null;
		}

		//spawn foodpickup object and initialise it with random ingredientinfo
		GameObject foodInstance=Instantiate (food,transform);
		foodInstance.GetComponent<FoodPickup> ().info = pm.GetRandomIngredient ();
		foodInstance.GetComponent<FoodPickup> ().LoadFromInfo ();
		//also separate foodpickup from crate parent
		foodInstance.transform.SetParent (null);

		//spawn sound for crate smashing
		SoundManager.GetComponent<SoundManager>().newSound(transform,force*0.1f,force*0.2f);
	}

	//by default smash from centre of crate
	public void Smash(float force){
		Smash (transform.position,force);
	}

}
