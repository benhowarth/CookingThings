using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPickup : MonoBehaviour {
	private Camera cam;
	public IngredientInfo info;
	public PickupManager pm;
	public NotificationSpawner NS;
	// Use this for initialization
	void Start () {
		cam = Camera.main;
		pm = GameObject.Find ("PickupManager").GetComponent<PickupManager> ();
		NS = GameObject.Find ("NotificationSpawner").GetComponent<NotificationSpawner> ();
	}

	public void LoadFromInfo(){
		GetComponent<MeshFilter>().mesh = info.mesh;
		GetComponent<MeshRenderer> ().material.mainTexture = info.textureAlbedo;
		GetComponent<MeshRenderer> ().material.SetTexture("_BumpMap",info.textureNormal);
		transform.localScale = new Vector3 (info.pickupScale, info.pickupScale, info.pickupScale);
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void Pickup(){
		//Debug.Log ("Picked Up "+info.name+"!");
		pm.Pickup(info.id);
		NS.SpawnNotification ("+" + info.name, info.color);
		Destroy (transform.gameObject);
	}

	void onCollisionEnter(Collision col){
		/*Debug.Log (col);
		if (col.gameObject.name == "Player") {
			Destroy (gameObject);
		}*/
	}

	public string GetInfoString(){
		return (info.name+"\nEnergy:"+info.energy+"\nVitamins:"+info.vitamins+"\nDisease:"+info.disease);
	}
}
