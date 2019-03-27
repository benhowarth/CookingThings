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
	private float hpMax=100.0f;
	public bool dead;
	private float healthRegenGap;
	public float healthRegenGapMax=50.0f;

	public GameObject SoundManager;

	public Collider playerCol;
	public GameObject playerModel;
	public Animator anim;

	public GameObject knifeObj;
	public Collider knifeCol;
	private Rigidbody rb;

	public Image HPBar;


	public float footStepTimer;
	public float footStepTimerMax=0.5f;

	public bool hidden=false;
	public bool canUnhide=false;
	public GameObject currentLocker;


	public GameObject foodPickupHoverOver;
	public GameObject crateHoverOver;
	public float crateOpenAmount;

	public GameObject CursorLoading;
	public GameObject pickupTooltip;
	public Text pickupTooltipText;

	// Use this for initialization
	void Start () {
		target=transform.position;
		//target=new Vector3(transform.position.x+10.0f,transform.position.y,transform.position.z);
		hp = hpMax;
		dead = false;
		healthRegenGap = 0.0f;
		
		rb = GetComponent<Rigidbody> ();
		HPBar.fillAmount = hp / hpMax;

		footStepTimer = -1;
		anim.SetBool ("dead",false);
		
		foodPickupHoverOver = null;
		crateHoverOver = null;
		crateOpenAmount = 0f;
	}
	
	// Update is called once per frame
	void Update () {

		if (hp != 0) {
			Vector3 mPos = Input.mousePosition;
			RaycastHit hit;
			Ray camRay = cam.ScreenPointToRay (mPos);
			if (Physics.Raycast (camRay, out hit, Mathf.Infinity, LayerMask.GetMask ("foodPickup","Crate","Ground"))) {
				Vector3 aimPos = new Vector3 (hit.point.x, transform.Find ("PlayerModel").position.y, hit.point.z);
				transform.Find ("PlayerModel").LookAt (aimPos);
				Debug.Log ("Hover"+hit.transform.gameObject);
				if (hit.transform.gameObject.tag == "foodPickup") {
					if(foodPickupHoverOver==null || foodPickupHoverOver!=hit.transform.gameObject){
						foodPickupHoverOver=hit.transform.gameObject;
					}
					if (Input.GetMouseButton (0) || Input.GetKey (KeyCode.E)) {
						hit.transform.gameObject.GetComponent<FoodPickup> ().Pickup ();
					}
					crateOpenAmount=0f;
				}else if(hit.transform.gameObject.tag=="Crate"){
					if(hit.transform.gameObject==crateHoverOver){
						if (Input.GetMouseButton (0) || Input.GetKey (KeyCode.E)) {
							Debug.Log ("Crate open amount:"+crateOpenAmount);
							crateOpenAmount+=crateHoverOver.GetComponent<Crate>().openSpeed*Time.deltaTime;
							if(crateOpenAmount>=1){
								crateHoverOver.GetComponent<Crate>().Smash (1f);
								crateOpenAmount=0f;
							}
						}
					}else{
						crateOpenAmount=0f;
						crateHoverOver=hit.transform.gameObject;
					}
					foodPickupHoverOver=null;
				}else{
					crateOpenAmount=0f;
					crateHoverOver=null;
					foodPickupHoverOver=null;
				}
				CursorLoading.GetComponent<CursorLoading>().setPerc(crateOpenAmount);
			}

			if(foodPickupHoverOver){
				pickupTooltip.SetActive(true);
				pickupTooltip.transform.position = Input.mousePosition;
				pickupTooltipText.text=foodPickupHoverOver.GetComponent<FoodPickup>().GetInfoString();
			}else{
				pickupTooltip.SetActive(false);
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
		if (hidden) {
			if (canUnhide && (Input.GetKey (KeyCode.A)||Input.GetKey (KeyCode.D)||Input.GetKey (KeyCode.W)||Input.GetKey (KeyCode.S))) {
				Debug.Log("Try to unhide");
				unhide ();
			}
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
					//SoundManager.GetComponent<SoundManager> ().newSound (transform);
					if(hidden){unhide();}else{hide ();}
				}
				if (Input.GetKey (KeyCode.O)) {
					hp = 0;
				}
			}

			if (!knifeCol.enabled && !dead) {
				if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.S)) {
					if(footStepTimer==-1){footStepTimer=footStepTimerMax;}
					if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) {
						//anim.Play ("run");
						anim.SetBool("walking",false);
						anim.SetBool("running",true);
						footStepTimer+=Time.deltaTime*1.5f;
					} else {
						//anim.Play ("walk");
						anim.SetBool("running",false);
						anim.SetBool("walking",true);
						footStepTimer+=Time.deltaTime;
					}
					if(footStepTimer>=footStepTimerMax){
						footStepTimer=0;
						SoundManager.GetComponent<SoundManager>().newSound(transform,0.2f*runFactor,0.7f);
					}
				} else {
					footStepTimer=-1;
					anim.SetBool("running",false);
					anim.SetBool("walking",false);
					//anim.Play ("idle");

					//anim.Play("death");
				}
			}
		}
		
		

		if (rb.velocity.sqrMagnitude > maxVelocity*maxVelocity) {
			rb.velocity=rb.velocity.normalized*maxVelocity;
		}
	}

	void enableUnhiding(){
		canUnhide = true;
	}

	public void hide(){
		anim.SetBool ("walking", false);
		anim.SetBool ("running", false);
		Invoke ("enableUnhiding", 0.8f);
		hidden = true;
		//playerModel.SetActive(false);
		playerCol.enabled = false;
		rb.isKinematic = true;
		knifeObj.SetActive(false);
	}

	public void unhide(){
		hidden = false;
		canUnhide = false;
		//playerModel.SetActive(true);
		playerCol.enabled = true;
		rb.isKinematic = false;
		knifeObj.SetActive(true);
		currentLocker.GetComponent<Locker>().Open();
		currentLocker.GetComponent<Locker> ().DisableUntil (1.3f);
		currentLocker = null;
	}

	public void takeDamage(float damage){
		//anim.Play ("hit");
		anim.SetTrigger ("hit");
		if(hp-damage<=0.0f){
			hp=0;
			dead=true;
			anim.Play ("death");
			anim.SetBool ("dead",true);
			anim.SetTrigger ("dead");
			knifeObj.SetActive(false);
			playerCol.enabled = false;
		}else{
			hp=hp-damage;
			healthRegenGap=healthRegenGapMax;
		}
		HPBar.fillAmount = hp / hpMax;

	}

	void OnTriggerEnter(Collider col){
		//Debug.Log ("player enter trig"+col);
	}
	void OnCollisionEnter(Collision col){
		//Debug.Log ("player enter coll"+col);
	}
	void OnCollisionExit(Collision col){
		//Debug.Log ("player exit coll");
	}

}
