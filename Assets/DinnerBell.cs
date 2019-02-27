using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinnerBell : MonoBehaviour {
	
	public GameObject FoodManager;

	void OnMouseDown(){
		FoodManager.GetComponent<FoodManager> ().Serve ();
	}
}
