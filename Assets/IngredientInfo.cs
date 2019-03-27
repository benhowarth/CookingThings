using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="IngredientInfo",menuName="IngredientInfo")]
public class IngredientInfo : ScriptableObject {

	public string name;
	public int id;
	public float disease;
	public float vitamins;
	public float energy;
	public Mesh mesh;
	public Texture textureAlbedo;
	public Texture textureNormal;
	public Color color;
	public float pickupScale=1f;
	public List<string> tags;
}
