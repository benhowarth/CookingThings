using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="IngredientInfo",menuName="IngredientInfo")]
public class IngredientInfo : ScriptableObject {

	//general info
	public string name;
	public int id;
	//stats for citizen's health etc.
	public float disease;
	public float energy;
	//for foodpickup
	public Mesh mesh;
	public Texture textureAlbedo;
	public Texture textureNormal;
	public float pickupScale=1f;
	//for notification and UI
	public Color color;
	//for recipes and checking order requirements
	public List<string> tags;
}
