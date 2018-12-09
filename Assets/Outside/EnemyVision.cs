using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyVision : MonoBehaviour {
	
	private NavMeshAgent agent;
	private GameObject Player;
	// Use this for initialization
	void Start () {
		//GetComponent<Collider> ().enabled = true;
		Player = GameObject.Find ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		bool playerInVision = false;
		if (!transform.parent.GetComponent<EnemyAI> ().dead) {
			Collider[] hitColliders = Physics.OverlapSphere (transform.position+transform.forward*3.5f, 4,LayerMask.GetMask("Player"));
			if (hitColliders.Length > 0) {
				for (int i=0; i<hitColliders.Length; i++) {
					//Debug.Log ("hitCollider"+i+":"+hitColliders[i].gameObject.name);
					//if (hitColliders [i].gameObject.name == "Player" && !transform.parent.GetComponent<EnemyAI> ().seenPlayer) {
					if (hitColliders [i].gameObject.tag == "Player") {
						RaycastHit hit;
						
						Ray ray = new Ray(transform.position,Player.transform.position-transform.position);
						if(Physics.Raycast(ray, out hit,Mathf.Infinity,LayerMask.GetMask("Player", "Wall"))){
							if(hit.transform.gameObject.tag=="Player"){
							playerInVision=true;
							Debug.Log ("Seen " + hitColliders [i] + "!");
							transform.parent.GetComponent<EnemyAI> ().seenPlayer = true;
							transform.parent.GetComponent<EnemyAI> ().agent.SetDestination(hitColliders[i].gameObject.transform.position);
							}
						}
					}
				}
			}
			if(!playerInVision){
				transform.parent.GetComponent<EnemyAI> ().seenPlayer = false;
			}
		}
	}
	void OnCollisionEnter(Collision col){
		Debug.Log ("vision coll");
	}
	void OnTriggerEnter(Collider col){
		Debug.Log ("vision trig");
		/*if (col.gameObject.name == "Player") {
			Debug.Log ("Vision Hit");
			//transform.parent.GetComponent<EnemyAI>().seenPlayer=true;
		}*/
	}
}
