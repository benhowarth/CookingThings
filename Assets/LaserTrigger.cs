using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrigger : MonoBehaviour {

	public SoundManager sm;
	public Material normalMat;
	public Material alertMat;
	public Renderer render;
	public bool playerIn;

	void Start(){
		render.material = normalMat;
		playerIn = false;
	}

	void NotifyEnemiesInRadius(float rad){
		Collider[] enemiesToNotify = Physics.OverlapSphere (transform.position, rad, LayerMask.GetMask ("Enemy"));
		if (enemiesToNotify.Length > 0) {
			for (int i=0; i<enemiesToNotify.Length; i++) {
				EnemyAI ai=enemiesToNotify[i].gameObject.GetComponent<EnemyAI>();
				ai.searchAt(transform.position);
				//Debug.Log ("Notify enemy "+i+"!");
			}
		} else {
			//Debug.Log ("No enemies to notify!");
		}
	}

	void unAlert(){
		if (!playerIn) {
			render.material = normalMat;
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Player") {
			//sm.newSound(transform,3f,10f);
			NotifyEnemiesInRadius(40.0f);
			render.material=alertMat;
			playerIn=true;
			Invoke ("unAlert",2f);
		}
	}

	void OnTriggerExit(Collider other){
		if (other.gameObject.tag == "Player") {
			unAlert ();
			playerIn = false;
		}

	}
}
