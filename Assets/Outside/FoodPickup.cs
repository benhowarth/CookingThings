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
		//load mesh, texture and scale from ingredientinfo
		GetComponent<MeshFilter>().mesh = info.mesh;
		GetComponent<MeshRenderer> ().material.mainTexture = info.textureAlbedo;
		GetComponent<MeshRenderer> ().material.SetTexture("_BumpMap",info.textureNormal);
		transform.localScale = new Vector3 (info.pickupScale, info.pickupScale, info.pickupScale);
	}

	public void Pickup(){
		//send pickup to pickup manager
		pm.Pickup(info.id);
		//spawn notification at bottom of screen (e.g. "+Beef")
		NS.SpawnNotification ("+" + info.name, info.color);
		//destroy pickup gameobject
		Destroy (transform.gameObject);
	}


	//for sending information from ingredientinfo to tooltip in PlayerMovement.cs
	public string GetInfoString(){
		return (info.name+"\nEnergy:"+info.energy+"\nVitamins:"+info.vitamins+"\nDisease:"+info.disease);
	}
}
