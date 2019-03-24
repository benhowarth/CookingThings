using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locker : MonoBehaviour {

	public Animator anim;
	public Collider col;


	public void Open(){
		anim.SetTrigger("Open");
	}

	void Enable(){
		col.enabled = true;
	}

	public void DisableUntil(float time){
		col.enabled = false;
		Invoke ("Enable", time);
	}
	
	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Player") {
			//Debug.Log ("Player in locker");
			PlayerMovement p=other.GetComponent<PlayerMovement>();
			p.currentLocker=gameObject;
			other.transform.position=transform.position;
			p.hide();
			Open();
		}
	}
}
