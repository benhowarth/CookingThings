using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour {

	public GameObject food;
	public GameObject SoundManager;
	public PickupManager pm;
	public float openSpeed=0.1f;

	// Use this for initialization
	void Start () {
		pm = GameObject.Find ("PickupManager").GetComponent<PickupManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider col){
		//Debug.Log (col);
		if (col.gameObject.name=="Knife") {
			Smash(col.gameObject.transform.position,20f);
		}
	}
	public void Smash(Vector3 explosionCentre,float force){
		GetComponent<Collider> ().enabled = false;
		GetComponent<Rigidbody>().useGravity=false;
		int children=transform.childCount;
		for(int i=0;i<children;i++){
			Transform piece=transform.GetChild(i);
			Vector3 knifeHit=piece.position-explosionCentre;
			piece.GetComponent<Collider>().enabled=true;
			piece.GetComponent<Rigidbody>().useGravity=true;
			piece.GetComponent<Rigidbody>().isKinematic=false;
			piece.GetComponent<Rigidbody>().AddForce(knifeHit*force);
			piece.GetComponent<DestroyAfter>().ActivateDestructionTimer();
		}
		GameObject foodInstance=Instantiate (food, transform);
		foodInstance.GetComponent<FoodPickup> ().info = pm.GetRandomIngredient ();
		foodInstance.GetComponent<FoodPickup> ().LoadFromInfo ();
		SoundManager.GetComponent<SoundManager>().newSound(transform,0.5f,0.9f);
	}
	public void Smash(float force){
		Smash (transform.position,force);
	}

}
