using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState{
	PATROL,
	SEARCH,
	CHASE,
	ATTACK,
	STUNNED,
	DEAD
};

public class EnemyAI : MonoBehaviour {

	public Material noAttackMat;
	public Material attackMat;
	public float speed=2.0f;
	public bool dead=false;
	public bool seenPlayer=false;
	public GameObject Player;
	public GameObject bloodParticles;
	public GameObject food;
	private Vector3 knifeHit;
	public NavMeshAgent agent;

	private GameObject viewCone;
	private Renderer viewConeRenderer;
	public Material viewConeIdleMat,viewConeSearchMat,viewConeChaseMat,viewConeAttackMat;


	public EnemyState state;
	public Vector3 spawnLoc,patrolLoc,searchLoc,searchStartLoc;
	public float groundY=1.1f;
	public float searchTimer;
	public float searchTimerMax=16f;
	public float searchRad=6f;
	public float patrolRad=10f;
	public int patrolRouteNoMin=3;
	public int patrolRouteNoMax=7;
	public int patrolRouteNo=4;
	public List<Vector3> patrolRoute;
	public int patrolIndex=0;
	public LayerMask patrolObstacleMask;

	public GameObject fov;


	void Start () {
		Player=GameObject.Find ("Player");
		agent = GetComponent<NavMeshAgent>();
		agent.destination = transform.position;
		transform.Find("CubeTop").GetComponent<Renderer> ().material = noAttackMat;
		transform.Find("CubeBottom").GetComponent<Renderer> ().material = noAttackMat;
		viewCone=transform.Find ("View Visualization").gameObject;
		viewConeRenderer = viewCone.GetComponent<Renderer> ();

		spawnLoc = new Vector3 (transform.position.x,groundY,transform.position.z);
		patrolLoc = spawnLoc;
		searchLoc = spawnLoc;
		//set up patrol route
		patrolRouteNo = Random.Range (patrolRouteNoMin, patrolRouteNoMax);
		GeneratePatrolRoute();
		searchAt (spawnLoc);

	}


	void Update () {

		switch (state) {
		case EnemyState.PATROL:
			viewConeRenderer.material=viewConeIdleMat;
			if(seenPlayer){
				state=EnemyState.CHASE;
			}else{
				if(patrolRoute.Count>0){
					//if near patrolloc
					if(Vector3.Distance(transform.position,patrolLoc)<2.0){
						//get next patrol
						GetNewPatrolPoint();
					}
					for (int i=1; i<patrolRouteNo; i++) {
						Debug.DrawLine(patrolRoute[i-1], patrolRoute[i], Color.green);
					}
					Debug.DrawLine(patrolRoute[patrolRoute.Count-1],patrolRoute[0],Color.green);
				}else{
					Debug.Log ("noroute!");
				}
			}
			break;
		case EnemyState.SEARCH:
			viewConeRenderer.material=viewConeSearchMat;
			if(seenPlayer){
				state=EnemyState.CHASE;
			}


			if(searchTimer>0){
				searchTimer-=Time.deltaTime;
				//Debug.Log ("search dist: "+Vector3.Distance(transform.position,searchLoc));
				if(Vector3.Distance(transform.position,searchLoc)<2.0){
					//get next search
					GetNewSearchPoint();
				}
			}else{
				
				GetNewPatrolPoint();
				state=EnemyState.PATROL;
			}
			break;
		case EnemyState.CHASE:
			viewConeRenderer.material=viewConeChaseMat;
			agent.destination=Player.transform.position;
			if(!seenPlayer){
				searchAt(Player.transform.position);
			}else if(Vector3.Distance(transform.position,Player.transform.position)<=3){
				state=EnemyState.ATTACK;
			}
			break;
		case EnemyState.ATTACK:
			viewConeRenderer.material=viewConeAttackMat;
			if(Vector3.Distance(transform.position,Player.transform.position)>3){
				if(seenPlayer){
					state=EnemyState.CHASE;
				}else{
					searchAt(Player.transform.position);
				}
			}else{
				Player.GetComponent<PlayerMovement>().takeDamage (0.7f);
			}
			break;
		case EnemyState.DEAD:
			break;
		}

	}

	void Unstun(){
		searchAt (transform.position);
		fov.SetActive(true);
	}

	public void Stun(float time){
		state = EnemyState.STUNNED;
		agent.destination = transform.position;
		fov.SetActive(false);
		Invoke ("Unstun", time);
	}

	void GeneratePatrolRoute(){
		patrolRoute.Clear ();
		//Debug.Log ("Generating patrol route!");
		float stepAngleSize = 360f / patrolRouteNo;
		for(int i=0; i<patrolRouteNo;i++){
			//Debug.Log ("route "+i);
			float angle=transform.eulerAngles.y-180f+stepAngleSize*i;
			FieldOfView.ViewCastInfo newViewCast= FieldOfView.ViewCast(spawnLoc,angle,patrolRad,patrolObstacleMask,transform.rotation.eulerAngles,2f);
			patrolRoute.Add (newViewCast.point);
			//Debug.Log ("Patrol route "+i+" added (size now:"+patrolRoute.Count);
		}
		//Debug.Log ("Fin");
	}

	void GetNewPatrolPoint(){
		//Debug.Log ("Get new patrol point!");
		if (patrolIndex + 1 < patrolRoute.Count) {
			patrolIndex++;
		} else {
			patrolIndex=0;
		}
		patrolLoc=patrolRoute[patrolIndex];
		agent.destination = patrolLoc;
	}

	void GetNewSearchPoint(){
		//Debug.Log ("Get new search point");
		bool pointIsObstacle = true;
		Vector2 offset;
		Vector3 offset3=Vector3.zero;
		while (pointIsObstacle) {
			offset = (Random.insideUnitCircle) * searchRad;
			offset3 = new Vector3 (offset.x, groundY, offset.y);
			pointIsObstacle=IsPointObstacle(searchStartLoc+offset3);
		}
		searchLoc=searchStartLoc+offset3;
		agent.destination = searchLoc;
	}

	void SpawnFood(){
		Instantiate (bloodParticles, transform.position, Quaternion.identity);
		GameObject foodObj=Instantiate (food, transform);
		foodObj.GetComponent<FoodPickup> ().LoadFromInfo ();
		Vector3 foodForce = knifeHit * 250f;
		foodForce = new Vector3 (foodForce.x, foodForce.y+30f, foodForce.z);
		foodObj.GetComponent<Rigidbody>().AddForce(foodForce);
	}

	bool IsPointObstacle(Vector3 pos){
		RaycastHit hit;
		Vector3 newPos = pos;
		newPos.y = newPos.y + 10f;
		if (Physics.Raycast (newPos, Vector3.down, out hit, 15f, patrolObstacleMask)) {
			return true;
		} else {
			return false;
		}
	}

	void OnTriggerEnter(Collider col){
		if (col.gameObject.name == "Knife") {
			dead = true;
			state=EnemyState.DEAD;
			viewCone.SetActive(false);
			GetComponent<Collider> ().enabled = false;
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

			
			Invoke ("SpawnFood", 0.2f);
			agent.enabled=false;
		} else if (col.gameObject.tag == "Sound") {
			//seenPlayer=true;
			agent.SetDestination(col.gameObject.transform.position);
		}
	}

	
	void OnCollisionEnter(Collision col){		
		if (col.gameObject.tag == "Player" && !col.gameObject.GetComponent<PlayerMovement>().hidden) {
			seenPlayer=true;
			state=EnemyState.CHASE;
		}else if (col.gameObject.tag == "Crate" && col.impulse.magnitude>2f) {
			Stun(2f);
		}
	}

	public void searchAt(Vector3 pos){
		searchStartLoc = pos;
		searchTimer=searchTimerMax+(agent.speed*Vector3.Distance(transform.position,pos))/10;
		GetNewSearchPoint();
		state=EnemyState.SEARCH;
	}
}
