using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {
	
	public Material noAttackMat;
	public Material attackMat;
	public float speed=2.0f;
	public bool dead=false;
	public bool seenPlayer=false;
	public GameObject Player;
	public GameObject food;
	private Vector3 knifeHit;
	public NavMeshAgent agent;
	private GameObject visionLight;

	// Use this for initialization
	void Start () {
		Player=GameObject.Find ("Player");
		visionLight = transform.Find ("Flashlight").gameObject;
		agent = GetComponent<NavMeshAgent>();
		agent.destination = transform.position;
		transform.Find("CubeTop").GetComponent<Renderer> ().material = noAttackMat;
		transform.Find("CubeBottom").GetComponent<Renderer> ().material = noAttackMat;
	}
	
	// Update is called once per frame
	void Update () {
		if (!dead) {
			if(seenPlayer){
				visionLight.GetComponent<Light>().color=Color.red;
				if(Vector3.Distance(transform.position,Player.transform.position)<2.5){
					Debug.Log ("Attack player");
					Player.GetComponent<PlayerMovement>().takeDamage (0.7f);
				}
			}else{
				visionLight.GetComponent<Light>().color=Color.white;
			}
		}
	}
	void SpawnFood(){
		GameObject foodObj=Instantiate (food, transform);
		Vector3 foodForce = knifeHit * 250f;
		foodForce = new Vector3 (foodForce.x, foodForce.y+30f, foodForce.z);
		foodObj.GetComponent<Rigidbody>().AddForce(foodForce);
	}
	void OnTriggerEnter(Collider col){
		if (col.gameObject.name == "Knife") {
			dead = true;
			GetComponent<Collider> ().enabled = false;
			transform.Find ("Flashlight").GetComponent<Light> ().enabled = false;
			transform.Find ("CubeTop").GetComponent<Collider> ().enabled = true;
			transform.Find ("CubeTop").GetComponent<Rigidbody> ().isKinematic = false;
			transform.Find ("CubeTop").GetComponent<Rigidbody> ().useGravity = true;
			knifeHit = col.gameObject.transform.position - transform.Find ("CubeTop").transform.position;
			//transform.Find("CubeTop").GetComponent<Rigidbody>().AddForce(new Vector3((Random.value*2)-1,1,Random.value)*500f);
			transform.Find ("CubeTop").GetComponent<Rigidbody> ().AddForce (-knifeHit * 500f);	
			transform.Find ("CubeTop").GetComponent<Renderer> ().material = attackMat;

			
			transform.Find ("CubeBottom").GetComponent<Collider> ().enabled = true;
			transform.Find ("CubeBottom").GetComponent<Rigidbody> ().isKinematic = false;
			transform.Find ("CubeBottom").GetComponent<Rigidbody> ().useGravity = true;
			transform.Find ("CubeBottom").GetComponent<Renderer> ().material = attackMat;

			
			GetComponent<Collider> ().enabled = false;
			
			transform.Find ("Vision").GetComponent<Collider> ().enabled = false;

			
			Invoke ("SpawnFood", 0.2f);
			agent.enabled=false;
		} else if (col.gameObject.tag == "Sound") {
			//seenPlayer=true;
			agent.SetDestination(col.gameObject.transform.position);
		}
	}

	
	void OnCollisionEnter(Collision col){
		/*Debug.Log ("enemy enter coll");
		
		if (col.gameObject.tag == "Player") {
			attackingPlayer=true;
		}*/
	}
	void OnCollisionExit(Collision col){
		/*Debug.Log ("enemy exit coll");
		
		if (col.gameObject.tag == "Player") {
			attackingPlayer=false;
		}*/
	}
}
