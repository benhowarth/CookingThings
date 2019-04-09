using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnifeAttack : MonoBehaviour {
	public bool knifeEnabled = true;
	public float reloadTimer = 0f;
	public float reloadTime=10.0f;
	public bool reloaded;
	public float attackTime=1f;
	public GameObject KnifeParticle;
	public GameObject SoundManager;
	public Image KnifeBar;
	public Animator anim;

	//to start player is not attacking, and knife is already fully charged
	void Start () {
		GetComponent<Collider>().enabled=false;
		reloaded = true;
		reloadTimer = reloadTime;
	}

	//disable hurtbox
	void DisableKnife(){
		GetComponent<Collider>().enabled=false;
	}

	void Reload(){
		reloaded = true;
	}


	void FixedUpdate () {
		KnifeBar.fillAmount=reloadTimer/reloadTime;
		//if knife has charge
		if (reloaded) {
			//if can attack and press attack button
			if (knifeEnabled && (Input.GetMouseButtonDown (1) || Input.GetKeyDown ("space"))) {
				//set reload timer, bool and bar to 0
				reloadTimer=0f;
				KnifeBar.fillAmount=0;
				reloaded = false;
				//reload in an amount of time
				Invoke ("Reload", reloadTime);
				//enable hurtbox infront of player
				GetComponent<Collider> ().enabled = true;
				//disable hurtbox in an amount of time
				Invoke ("DisableKnife", attackTime);
				//spawn sweet attack particle effect
				Instantiate (KnifeParticle, transform);
				//spawn sound as attacking should be loud
				SoundManager.GetComponent<SoundManager> ().newSound (transform, 0.7f, 0.9f);
				//make attack animation play
				anim.SetTrigger ("attack");
			}
		} else {
			//keep reloading knife
			reloadTimer+=Time.deltaTime;
		}
	}

}
