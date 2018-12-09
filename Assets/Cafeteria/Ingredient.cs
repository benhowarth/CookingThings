using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BzKovSoft.ObjectSlicerSamples;

public class Ingredient : MonoBehaviour {
	public float startingMass;
	public float minMass;
	public IngredientInfo info;
	// Use this for initialization
	void Start () {
		Debug.Log (GetComponent<Rigidbody> ().mass+"<="+minMass+": "+(GetComponent<Rigidbody> ().mass <= minMass));
		if (GetComponent<Rigidbody> ().mass <= minMass) {
			GetComponent<ObjectSlicerSample>().enabled=false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
