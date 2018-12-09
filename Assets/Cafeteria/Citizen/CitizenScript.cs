using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenScript : MonoBehaviour {
	public CitizenInfo info;
	public TextAsset namesTxt;
	private string[] names;
	public string citizenName="";
	private char[] alphabet;
	// Use this for initialization
	void Start () {
		alphabet="ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
		randomise ();
	}


	public void randomise(){
		if (namesTxt != null) {
			names=(namesTxt.text.Split('\n'));
			citizenName = names[Random.Range (0, names.Length)]+" "+alphabet[Random.Range(0,alphabet.Length)]+".";
		}
		//randomise stuff
		//head
		transform.Find ("Head").gameObject.GetComponent<planeTextureChooser> ().randomiseTexture ();
		transform.Find ("Head").gameObject.GetComponent<planeTextureChooser> ().UpdateTexture();
		//eyes
		transform.Find ("Head/Eyes").gameObject.GetComponent<eyeRandomiser> ().randomise();
		transform.Find ("Head/Eyes").gameObject.GetComponent<eyeRandomiser> ().UpdateEyes();
		//mouth
		transform.Find ("Head/Eyes/Mouth").gameObject.GetComponent<planeTextureChooser> ().randomiseTexture ();
		transform.Find ("Head/Eyes/Mouth").gameObject.GetComponent<planeTextureChooser> ().UpdateTexture ();
		//body
		transform.Find ("Body").gameObject.GetComponent<planeTextureChooser> ().randomiseTexture ();
		transform.Find ("Body").gameObject.GetComponent<planeTextureChooser> ().UpdateTexture ();
		UpdateTag ();
	}

	void UpdateTag(){
		transform.Find("Body/NameTag").gameObject.GetComponent<TextMesh> ().text = citizenName;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.R)) {
			randomise();
		}		
	}
}
