using UnityEngine;
using System.Collections;

public class Cookable : MonoBehaviour {
	public float cookHp=100.0f;
	public float burnHp=100.0f;
	public float beingCookedAmount=0.0f;
	public bool cooked=false;
	public bool burnt = false;
	public Material cookedMat;
	public Material burntMat;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (beingCookedAmount > 0) {
			cookHp-=beingCookedAmount;
		}
		if (cookHp <= -burnHp) {
			burnt = true;
			GetComponent<Renderer>().material=burntMat;
		}else if (cookHp <= 0) {
			cooked = true;
			GetComponent<Renderer>().material=cookedMat;
		}
	
	}
}
