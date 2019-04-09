using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CitizenScript : MonoBehaviour {
	public CitizenInfo info;
	public CitizenManager citizenManager;
	public Animator anim;

	public string citizenName="name";

	public Image healthBar;
	public Image energyBar;

	public MeshRenderer headMeshRenderer;
	public GameObject eyes;
	public GameObject eyeL;
	public GameObject eyeR;
	public MeshRenderer mouthMeshRenderer;
	public MeshRenderer bodyMeshRenderer;

	public TextMesh nameTagTextMesh;

	public float healthLevel;
	public float energyLevel;

	// Use this for initialization
	void Start () {
		//randomise ();
		updateFromInfo ();
	}

	public void updateFromInfo(){
		citizenName = info.name;
		healthLevel = info.healthLevel;
		energyLevel = info.energyLevel;
		
		healthBar.fillAmount=healthLevel;
		energyBar.fillAmount=energyLevel;

		headMeshRenderer.material.color = info.headColor;
		headMeshRenderer.material.SetTexture("_MainTex",citizenManager.headTextures[info.headTexture]);


		bodyMeshRenderer.material.color = info.bodyColor;
		bodyMeshRenderer.material.SetTexture("_MainTex",citizenManager.bodyTextures[info.bodyTexture]);

		Vector3 eyesPos = eyes.transform.localPosition;
		eyes.transform.localPosition=new Vector3(eyesPos.x,eyesPos.y,info.eyeSpacingY);
		Vector3 eyePos = eyeL.transform.localPosition;
		eyeL.transform.localPosition = new Vector3 (info.eyeSpacingX, eyePos.y, eyePos.z);
		eyeR.transform.localPosition = new Vector3 (-info.eyeSpacingX, eyePos.y, eyePos.z);

		
		mouthMeshRenderer.material.color = info.mouthColor;
		mouthMeshRenderer.material.SetTexture("_MainTex",citizenManager.mouthTextures[info.mouthTexture]);
		
		nameTagTextMesh.text = info.name;
	}



	
	// Update is called once per frame
	void Update () {

	}
}
