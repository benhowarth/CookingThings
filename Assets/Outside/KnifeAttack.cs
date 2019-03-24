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
	// Use this for initialization
	void Start () {
		GetComponent<Collider>().enabled=false;
		reloaded = true;
	}

	void DisableKnife(){
		GetComponent<Collider>().enabled=false;
	}

	void Reload(){
		reloaded = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (reloaded) {
			if (knifeEnabled && (Input.GetMouseButtonDown (1) || Input.GetKeyDown ("space"))) {
				reloadTimer=0f;
				KnifeBar.fillAmount=0;
				reloaded = false;
				Invoke ("Reload", reloadTime);
				GetComponent<Collider> ().enabled = true;
				Invoke ("DisableKnife", attackTime);
				Instantiate (KnifeParticle, transform);
				SoundManager.GetComponent<SoundManager> ().newSound (transform, 0.7f, 0.9f);
				anim.SetTrigger ("attack");
				//Debug.Log("playing attack!");
			}
		} else {
			reloadTimer+=Time.deltaTime;
			KnifeBar.fillAmount=reloadTimer/reloadTime;
		}
	}

}
