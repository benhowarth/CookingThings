using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeAttack : MonoBehaviour {
	public float attackTimer=0.0f;
	public float attackTimerMax=30.0f;
	public Material noAttackMat;
	public Material attackMat;
	public GameObject KnifeParticle;
	public GameObject SoundManager;
	public Animator anim;
	// Use this for initialization
	void Start () {
		GetComponent<Collider>().enabled=false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("space") && attackTimer == 0) {
			attackTimer = attackTimerMax;
			GetComponent<Renderer> ().material = attackMat;
			GetComponent<Collider>().enabled=true;
			Instantiate(KnifeParticle,transform);
			SoundManager.GetComponent<SoundManager>().newSound(transform);
			anim.Play("attack1(WeaponOneHand)");
		} else {
			if(attackTimer>0){
				attackTimer=attackTimer-1;
			}else{
				GetComponent<Renderer> ().material = noAttackMat;
				GetComponent<Collider>().enabled=false;
			}
		}


	}
}
