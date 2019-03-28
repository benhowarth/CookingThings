using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {
	public Camera cam;
	public float speed;
	public float maxVelocity;
	public float hp=100.0f;
	private float hpMax=100.0f;
	public bool dead;

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
	public bool canBeSeenStartingHide = false;
	public GameObject currentLocker;


	public GameObject foodPickupHoverOver;
	public GameObject crateHoverOver;
	public float crateOpenAmount;

	public GameObject CursorLoading;
	public GameObject pickupTooltip;
	public Text pickupTooltipText;


	void Start () {
		//init hp variables
		hp = hpMax;
		dead = false;
		HPBar.fillAmount = hp / hpMax;

		rb = GetComponent<Rigidbody> ();

		footStepTimer = -1;
		anim.SetBool ("dead",false);
		
		foodPickupHoverOver = null;
		crateHoverOver = null;
		crateOpenAmount = 0f;

		//init variables for hiding in lockers
		hidden=false;
		canUnhide=false;
		canBeSeenStartingHide = false;
	}
	
	// Update is called once per frame
	void Update () {

		//if not dead
		if (hp != 0) {

			//check what the mouse is hovering over (foodpickup or crate)
			Vector3 mPos = Input.mousePosition;
			RaycastHit hit;
			Ray camRay = cam.ScreenPointToRay (mPos);
			//use ray from camera to ground through mouse position on screen
			if (Physics.Raycast (camRay, out hit, Mathf.Infinity, LayerMask.GetMask ("foodPickup","Crate","Ground"))) {
				//turn playermodel on the y axis to face where mouse ray hit 
				Vector3 aimPos = new Vector3 (hit.point.x, transform.Find ("PlayerModel").position.y, hit.point.z);
				transform.Find ("PlayerModel").LookAt (aimPos);
			
				//if hover over food pickup
				if (hit.transform.gameObject.tag == "foodPickup") {
					//if not hovering over foodpickup or hovering over a different foodpickup to before
					if(foodPickupHoverOver==null || foodPickupHoverOver!=hit.transform.gameObject){
						//store the currently hovered over foodpickup
						foodPickupHoverOver=hit.transform.gameObject;
					}
					//if mouse is clicked, pickup foodpickup (see FoodPickup.cs and PickupManager.cs)
					if (Input.GetMouseButton (0)) {
						hit.transform.gameObject.GetComponent<FoodPickup> ().Pickup ();
					}
					//reset crate open percentage (so progress of a crate is reset when mouse moves away)
					crateOpenAmount=0f;
				
				//if hover over crate
				}else if(hit.transform.gameObject.tag=="Crate"){
					//if still hovering over same crate
					if(hit.transform.gameObject==crateHoverOver){
						//if holding down mouse button
						if (Input.GetMouseButton (0)) {
							//increment crate open percentage
							crateOpenAmount+=crateHoverOver.GetComponent<Crate>().openSpeed*Time.deltaTime;
							//if crate open percentage reached max
							if(crateOpenAmount>=1){
								//open crate
								crateHoverOver.GetComponent<Crate>().Smash (1f);
								//reset crate open percentage for next crate
								crateOpenAmount=0f;
							}
						}
					}else{
						//reset crate open percentage (so progress of a crate is reset when mouse moves away)
						crateOpenAmount=0f;
						//store last crate hovered over
						crateHoverOver=hit.transform.gameObject;
					}
					//reset foodpickup hovered over as no longer hovering over a pickup
					foodPickupHoverOver=null;
				
				//hovering over nothing of interest
				}else{
					crateOpenAmount=0f;
					crateHoverOver=null;
					foodPickupHoverOver=null;
				}
				//set fill percentage of radial loading cursor
				CursorLoading.GetComponent<CursorLoading>().setPerc(crateOpenAmount);
			}

			//if hovering over foodpickup
			if(foodPickupHoverOver){
				//show foodpickup info tooltip
				pickupTooltip.SetActive(true);
				pickupTooltip.transform.position = Input.mousePosition;
				pickupTooltipText.text=foodPickupHoverOver.GetComponent<FoodPickup>().GetInfoString();
			}else{
				//hide foodpickup info tooltip
				pickupTooltip.SetActive(false);
			}



		}


	}

	void FixedUpdate(){
		//if player in locker
		if (hidden) {
			//if player moves and enough time has passed in the locker (to stop infinite loop of getting in and out)
			if (canUnhide && (Input.GetKey (KeyCode.A)||Input.GetKey (KeyCode.D)||Input.GetKey (KeyCode.W)||Input.GetKey (KeyCode.S))) {
				unhide ();
			}
		//if not in locker (i.e. running about)
		} else {
			//amount to multiply move speed by (1.0 is walking)
			float runFactor = 1.0f;

			//if player currently alive
			if (!dead) {
				//shift=run (run speed almost twice walk speed is comfortable)
				if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) {
					runFactor = 1.9f;
				}
				

				//handle moving with WASD
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


				//if not attacking (attacking animation/collisions etc. handled in KnifeAttack.cs)
				if (!knifeCol.enabled){
					//if walking/running around
					if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.S)) {
						//if taking the first step after being still
						//begin footstep timer with a step (stops
						if(footStepTimer==-1){footStepTimer=footStepTimerMax;}
						//if running, play running animation and speed up footstep timer
						if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) {
							//set animation bools
							anim.SetBool("walking",false);
							anim.SetBool("running",true);
							footStepTimer+=Time.deltaTime*1.5f;
						//else (walking), add to footstep timer
						} else {
							//set animation bools
							anim.SetBool("running",false);
							anim.SetBool("walking",true);
							footStepTimer+=Time.deltaTime;
						}
						//if footstep timer has reached maximum, reset and spawn sound
						if(footStepTimer>=footStepTimerMax){
							footStepTimer=0;
							SoundManager.GetComponent<SoundManager>().newSound(transform,0.2f*runFactor,0.7f);
						}
					//if standing still, footstep timer is set to -1 so a sound is spawned upon walking
					} else {
						footStepTimer=-1;
						//set animation bools
						anim.SetBool("running",false);
						anim.SetBool("walking",false);
					}
				}
			}//end of if not dead

		}//end of if not hidden
		
		

		if (rb.velocity.sqrMagnitude > maxVelocity*maxVelocity) {
			rb.velocity=rb.velocity.normalized*maxVelocity;
		}
	}

	//start period where enemies can see you when hiding in a locker
	void startToHide(){
		canBeSeenStartingHide = true;
		//amount of time that an enemy can see the player "entering" the locker
		Invoke ("safelyHidden", 2f);
	}

	//confirm player is safely hidden (now cannot be seen by enemies)
	void safelyHidden(){
		canBeSeenStartingHide = false;
	}

	//allow player to unhide from a locker
	void enableUnhiding(){
		canUnhide = true;
	}

	//function to hide player (for use in Locker.cs)
	public void hide(){
		anim.SetBool ("walking", false);
		anim.SetBool ("running", false);
		Invoke ("enableUnhiding", 0.8f);
		hidden = true;
		//playerModel.SetActive(false);
		//playerCol.enabled = false;
		rb.isKinematic = true;
		knifeObj.SetActive(false);
		startToHide ();
	}

	
	//function to unhide player (for use in Locker.cs)
	public void unhide(){
		hidden = false;
		canUnhide = false;
		//playerModel.SetActive(true);
		//playerCol.enabled = true;
		rb.isKinematic = false;
		knifeObj.SetActive(true);
		currentLocker.GetComponent<Locker>().Open();
		currentLocker.GetComponent<Locker> ().DisableUntil (1.3f);
		currentLocker = null;
	}

	//function to handle player health/animations for damage and to check for DEATH
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
		}
		HPBar.fillAmount = hp / hpMax;

	}
	

}
