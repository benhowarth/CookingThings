using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour {
	public List<int> pickupsCollected;
	public List<IngredientInfo> ingredients;
	public SceneChanger sc;

	public void Pickup(int pickupId){
		Debug.Log ("Pickup "+pickupId+" Recieved!");
		pickupsCollected.Add (pickupId);
	}
	// Use this for initialization
	void Start () {
		
	}

	public IngredientInfo GetRandomIngredient(){
		return ingredients[Random.Range (0, ingredients.Count)];
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.K)) {
			EndGame ();
		}
	}

	public void EndGame(){
		foreach(int pickup in pickupsCollected){
			if(PlayerPrefs.HasKey("food"+pickup)){
				PlayerPrefs.SetInt ("food"+pickup,PlayerPrefs.GetInt ("food"+pickup)+1);
			}else{
				PlayerPrefs.SetInt ("food"+pickup,1);
			}
		}
		sc.FadeToOtherLevel ();
	}
}
