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
			int children=transform.childCount;
			for(int i=0;i<children;i++){
				Vector3 knifeHit=col.gameObject.transform.position-transform.GetChild(i).transform.position;
				transform.GetChild(i).GetComponent<Collider>().enabled=true;
				transform.GetChild(i).GetComponent<Rigidbody>().useGravity=true;
				transform.GetChild(i).GetComponent<Rigidbody>().isKinematic=false;
				transform.GetChild(i).GetComponent<Rigidbody>().AddForce(10f*-knifeHit);

			}
			Instantiate (food, transform);
			SoundManager.GetComponent<SoundManager>().newSound(transform);
		}
	}
}
