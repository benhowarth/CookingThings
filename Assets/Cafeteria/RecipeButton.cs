using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RecipeButton : MonoBehaviour {
	public int id;
	// Use this for initialization
	void Start () {
		GetComponent<Button>().onClick.AddListener (() => CustomClick (id));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void CustomClick(int id){
		GameObject.Find ("Food Manager").gameObject.GetComponent<FoodManager2> ().Serve (id);
	}
}
