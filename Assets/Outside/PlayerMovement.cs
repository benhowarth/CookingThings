using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {
	public Camera cam;
	public float speed;
	public float maxVelocity;
	private Vector3 target;
	public float hp=100.0f;
	public bool dead;
	private Color cubeColor;
	private float healthRegenGap;
	public float healthRegenGapMax=50.0f;
	public GameObject SoundManager;
	public Animator anim;
	public Collider knifeCol;
	private Rigidbody rb;
	public GameObject HPTextObj;
	private Text HPText;
	public float footStepTimer;
	public float footStepTimerMax=0.5f;

	// Use this for initialization
	void Start () {
		target=transform.position;
		//target=new Vector3(transform.position.x+10.0f,transform.position.y,transform.position.z);
		hp = 100.0f;
		cubeColor = Color.green;
		dead = false;
		healthRegenGap = 0.0f;
		
		rb = GetComponent<Rigidbody> ();
		HPText=HPTextObj.GetComponent<Text> ();
		HPText.text = "HP: " + Mathf.Floor (hp)+"%";

		footStepTimer = -1;
	}
	
	// Update is called once per frame
	void Update () {
		cubeColor = Color.Lerp (Color.red, Color.green, hp / 100.0f);
		transform.Find ("PlayerModel/Cube").GetComponent<Renderer> ().material.shader = Shader.Find ("Standard");
		transform.Find ("PlayerModel/Cube").GetComponent<Renderer> ().material.color = cubeColor;

		
		if (hp == 0) {
			dead=true;
		} else {



			Vector3 mPos = Input.mousePosition;
			RaycastHit hit;
			Ray camRay = cam.ScreenPointToRay (mPos);
			if (Physics.Raycast (camRay, out hit, Mathf.Infinity, LayerMask.GetMask ("foodPickup", "Ground"))) {
				Vector3 aimPos = new Vector3 (hit.point.x, transform.Find ("PlayerModel").position.y, hit.point.z);
				transform.Find ("PlayerModel").LookAt (aimPos);
				if (hit.transform.gameObject.tag == "foodPickup") {
					if (Input.GetKey (KeyCode.E)) {
						hit.transform.gameObject.GetComponent<FoodPickup> ().Pickup ();
					}
					if (Input.GetMouseButtonDown (0)) {

					}
				}
			}

			if (healthRegenGap == 0.0f) {
				if (hp + 0.1f >= 100.0f) {
					hp = 100.0f;
				} else {
					hp = hp + 0.5f;
				}
			} else {
				healthRegenGap = healthRegenGap - 1.0f;
			}


			if (Vector3.Distance (transform.position, target) > 0.2) {
				//transform.position = Vector3.MoveTowards (transform.position, target, speed * Time.deltaTime);
				//Debug.Log ("Moving");
			} else {
				//Debug.Log ("Not moving");
			}

		}


	}

	void FixedUpdate(){
		if (dead) {
			anim.Play ("death");
		} else {
			//movement
			float runFactor = 1.0f;
			if (!dead) {
				if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) {
					runFactor = 1.9f;
				}
				
				
				if (Input.GetKey (KeyCode.A)) {
					rb.AddForce (Vector3.left * speed * runFactor);
				}
				if (Input.GetKey (KeyCode.D)) {
					rb.AddForce (Vector3.right * speed * runFactor);
				}
				if (Input.GetKey (KeyCode.W)) {
					rb.AddForce (Vector3.forward * speed * runFactor);
				}
				if (Input.GetKey (KeyCode.S)) {
					rb.AddForce (Vector3.back * speed * runFactor);
				}
				if (Input.GetKeyDown (KeyCode.Q)) {
					SoundManager.GetComponent<SoundManager> ().newSound (transform);
				}
				if (Input.GetKey (KeyCode.O)) {
					hp = 0;
				}
			}

			if (knifeCol.enabled) {
				anim.Play ("attack1(WeaponOneHand)");
			} else {
				if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.S)) {
					if(footStepTimer==-1){footStepTimer=footStepTimerMax;}
					if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) {
						anim.Play ("run");
						footStepTimer+=Time.deltaTime*1.5f;
					} else {
						anim.Play ("walk");
						footStepTimer+=Time.deltaTime;
					}
					if(footStepTimer>=footStepTimerMax){
						footStepTimer=0;
						SoundManager.GetComponent<SoundManager>().newSound(transform,2f*runFactor,0.7f);
					}
				} else {
					footStepTimer=-1;
					anim.Play ("idle");

					//anim.Play("death");
				}
			}
		}
		
		

		if (rb.velocity.sqrMagnitude > maxVelocity*maxVelocity) {
			rb.velocity=rb.velocity.normalized*maxVelocity;
		}
	}

	public void takeDamage(float damage){
		if(hp-damage<=0.0f){
			hp=0;
			dead=true;
		}else{
			hp=hp-damage;
			healthRegenGap=healthRegenGapMax;
		}
		HPText.text = "HP: " + Mathf.Ceil(hp)+"%";

	}

	void OnCollisionEnter(Collision col){
		//Debug.Log ("player enter coll"+col);
	}
	void OnCollisionExit(Collision col){
		//Debug.Log ("player exit coll");
	}

}
