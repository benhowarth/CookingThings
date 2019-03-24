using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour {

	public GameObject food;
	public GameObject SoundManager;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider col){
		//Debug.Log (col);
		if (col.gameObject.name=="Knife") {
			GetComponent<Collider> ().enabled = false;
			GetComponent<Rigidbody>().useGravity=false;
			int children=transform.childCount;
			for(int i=0;i<children;i++){
				Transform piece=transform.GetChild(i);
				Vector3 knifeHit=piece.position-col.gameObject.transform.position;
				piece.GetComponent<Collider>().enabled=true;
				piece.GetComponent<Rigidbody>().useGravity=true;
				piece.GetComponent<Rigidbody>().isKinematic=false;
				piece.GetComponent<Rigidbody>().AddForce(knifeHit*10f);
				piece.GetComponent<DestroyAfter>().ActivateDestructionTimer();
			}
			Instantiate (food, transform);
			SoundManager.GetComponent<SoundManager>().newSound(transform,0.5f,0.9f);
		}
	}
}
