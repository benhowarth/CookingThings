using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planeTextureChooser : MonoBehaviour {

	public List<Texture> textureOptions;
	public int textureId;
	public Color color;
	public Color minColor;
	public Color maxColor;

	// Use this for initialization
	void Start () {
		//randomiseTexture ();
		//UpdateTexture ();
	}

	public void randomiseTexture(){
		color = Color.Lerp (minColor, maxColor, Random.Range (0.0f, 1.0f));
		textureId = (int) Mathf.Floor (Random.Range (0.0f, textureOptions.Count));
	}
	public void UpdateTexture(){
		transform.GetComponent<Renderer> ().material.color = color;
		transform.GetComponent<Renderer> ().material.SetTexture("_MainTex",textureOptions [textureId]);
	}
	
	// Update is called once per frame
	void Update () {
		/*if (Input.GetKeyDown (KeyCode.R)) {
			randomiseTexture();
			UpdateTexture();
		}*/
	}
}
