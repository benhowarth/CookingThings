using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour {
	public List<int> pickupsCollected;

	public void Pickup(int pickupId){
		Debug.Log ("Pickup "+pickupId+" Recieved!");
		pickupsCollected.Add (pickupId);
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
