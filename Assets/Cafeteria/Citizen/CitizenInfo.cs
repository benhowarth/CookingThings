using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName="CitizenInfo",menuName="CitizenInfo")]
public class CitizenInfo : ScriptableObject {
	public string name;
	public int headTexture;
	public int bodyTexture;
	public int mouthTexture;
	public float headColorVal;
	public float bodyColorVal;
	public float mouthColorVal;
	public Color headColor;
	public Color bodyColor;
	public Color mouthColor;
	public float eyeSpacingY;
	public float eyeSpacingX;
	public float healthLevel;
	public float energyLevel;
	public int job;
	public void Print(){
		Debug.Log (name);
	}
}
