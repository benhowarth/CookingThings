using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="RecipeInfo",menuName="RecipeInfo")]
public class RecipeInfo : ScriptableObject {
	
	//general info
	public string name;
	public int id;
	public bool unlocked;
	//stats for citizen's health etc.
	public float disease;
	public float energy;
	//required ingredients
	public List<int> ingredients;
	public List<int> ingredientsNo;
	//for recipes and checking order requirements
	public List<string> tags;
}