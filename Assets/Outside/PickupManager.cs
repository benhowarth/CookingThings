using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour {
	public List<int> pickupsCollected;
	public List<IngredientInfo> ingredients;
	public SceneChanger sc;

	//add pickup id to list of pickups to be added to the inventory when the level ends
	public void Pickup(int pickupId){
		Debug.Log ("Pickup "+pickupId+" Recieved!");
		pickupsCollected.Add (pickupId);
	}

	//get random ingredientinfo, for use in random spawning of foodpickup in Smash() in Crate.cs
	public IngredientInfo GetRandomIngredient(){
		return ingredients[Random.Range (0, ingredients.Count)];
	}


	void Update () {
		//move to cafeteria scene from outside
		if (Input.GetKey (KeyCode.K)) {
			EndGame ();
		}
	}

	public void EndGame(){
		//add pickups collected to inventory for cooking with
		foreach(int pickup in pickupsCollected){
			if(PlayerPrefs.HasKey("food"+pickup)){
				PlayerPrefs.SetInt ("food"+pickup,PlayerPrefs.GetInt ("food"+pickup)+1);
			}else{
				PlayerPrefs.SetInt ("food"+pickup,1);
			}
		}
		//fade out to cafeteria
		sc.FadeToOtherLevel ();
	}
}
