using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eyeRandomiser : MonoBehaviour {

	public float yValue;
	public float eyeSpacing;

	// Use this for initialization
	void Start () {
		randomise ();
		UpdateEyes ();
	}
	public void randomise(){
		yValue = Random.Range (-2.0f, 0.0f);
		//Debug.Log (yValue);
		eyeSpacing = Random.Range (10.0f, 25.0f);

	}
	public void UpdateEyes(){
		transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y,yValue);


		Vector3 eyeLPos = transform.Find ("EyeL").localPosition;
		transform.Find ("EyeL").localPosition=new Vector3(eyeSpacing,0.8f,0.8f);
		
		
		Vector3 eyeRPos = transform.Find ("EyeR").localPosition;
		transform.Find ("EyeR").localPosition=new Vector3(-eyeSpacing,0.8f,0.8f);

	}
	
	// Update is called once per frame
	void Update () {
		/*if (Input.GetKeyDown (KeyCode.R)) {
			randomise();
			UpdateEyes ();
		}*/	
	}
}
