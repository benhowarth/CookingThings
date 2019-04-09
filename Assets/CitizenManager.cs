using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenManager : MonoBehaviour {
	public CitizenScript citScript; 

	public TextAsset namesTxt;
	public string[] names;
	private char[] alphabet="ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
	

	public Color minHeadColor;
	public Color maxHeadColor;
	public List<Texture> headTextures=new List<Texture>();


	public Color minBodyColor;
	public Color maxBodyColor;
	public List<Texture> bodyTextures=new List<Texture>();

	
	public Color minMouthColor;
	public Color maxMouthColor;
	public List<Texture> mouthTextures=new List<Texture>();

	
	public enum Job{Civilian, Engineer, Doctor};

	public List<CitizenInfo> cits;
	public int currentCit=-1;

	// Use this for initialization
	void Start () {
		Debug.Log (PlayerPrefs.HasKey ("citNo"));
		//if save data
		if (PlayerPrefs.HasKey ("citNo")) {
			//load data
			loadCits();
		//if no save data (new game)
		} else {
			//gen data
			genCits (5);
			saveCits();
			//do tutorial?
		}
	}
	void loadCits(){
		int citNo=PlayerPrefs.GetInt("citNo");
		cits.Clear ();
		
		for (int i=0; i<citNo; i++) {
			CitizenInfo newCitizen = ScriptableObject.CreateInstance<CitizenInfo> ();
			
			newCitizen.name= PlayerPrefs.GetString("cit"+i+"name");
			
			newCitizen.healthLevel= PlayerPrefs.GetFloat("cit"+i+"healthLevel");
			newCitizen.energyLevel= PlayerPrefs.GetFloat("cit"+i+"energyLevel");

			newCitizen.headColorVal=PlayerPrefs.GetFloat("cit"+i+"headColor");
			newCitizen.headColor = Color.Lerp (minHeadColor, maxHeadColor, newCitizen.headColorVal);
			newCitizen.headTexture = PlayerPrefs.GetInt("cit"+i+"headTexture");
			
			
			newCitizen.bodyColorVal=PlayerPrefs.GetFloat("cit"+i+"bodyColor");
			newCitizen.bodyColor = Color.Lerp (minBodyColor, maxBodyColor, newCitizen.bodyColorVal);
			newCitizen.bodyTexture = PlayerPrefs.GetInt("cit"+i+"bodyTexture");

			newCitizen.mouthColorVal=PlayerPrefs.GetFloat("cit"+i+"mouthColor");
			newCitizen.mouthColor = Color.Lerp (minMouthColor, maxMouthColor, newCitizen.mouthColorVal);
			newCitizen.mouthTexture = PlayerPrefs.GetInt("cit"+i+"mouthTexture");

			
			newCitizen.eyeSpacingY = PlayerPrefs.GetFloat("cit"+i+"eyeSpacingY");
			newCitizen.eyeSpacingX = PlayerPrefs.GetFloat("cit"+i+"eyeSpacingX");
			
			newCitizen.job = PlayerPrefs.GetInt("cit"+i+"job");
			
			Debug.Log ("Loaded citizen!");
			newCitizen.Print();
			cits.Add(newCitizen);
		}
	}

	public void saveCits(){


		//clear cits in player prefs
		int citNo = PlayerPrefs.GetInt("citNo");
		for (int i=0; i<citNo; i++) {
			PlayerPrefs.DeleteKey("cit"+i+"name");
			PlayerPrefs.DeleteKey("cit"+i+"healthLevel");
			PlayerPrefs.DeleteKey("cit"+i+"energyLevel");
			
			
			
			PlayerPrefs.DeleteKey("cit"+i+"headColor");
			PlayerPrefs.DeleteKey("cit"+i+"bodyColor");
			PlayerPrefs.DeleteKey("cit"+i+"mouthColor");
			
			PlayerPrefs.DeleteKey("cit"+i+"headTexture");
			PlayerPrefs.DeleteKey("cit"+i+"bodyTexture");
			PlayerPrefs.DeleteKey("cit"+i+"mouthTexture");
			
			
			PlayerPrefs.DeleteKey("cit"+i+"eyeSpacingY");
			PlayerPrefs.DeleteKey("cit"+i+"eyeSpacingX");
			
			PlayerPrefs.DeleteKey("cit"+i+"job");
		}

		//save cits to player prefs
		PlayerPrefs.SetInt("citNo",cits.Count);
		
		for (int i=0; i<cits.Count; i++) {
			CitizenInfo newCitizen = cits[i];
			
			PlayerPrefs.SetString("cit"+i+"name",newCitizen.name);
			//Debug.Log ("Saving "+newCitizen.name+" local hp is "+newCitizen.healthLevel);
			PlayerPrefs.SetFloat("cit"+i+"healthLevel",newCitizen.healthLevel);
			//Debug.Log ("Saved "+newCitizen.name+" saved hp is "+PlayerPrefs.GetFloat("cit"+i+"healthLevel"));
			PlayerPrefs.SetFloat("cit"+i+"energyLevel",newCitizen.energyLevel);

			
			PlayerPrefs.SetFloat("cit"+i+"headColor",newCitizen.headColorVal);
			PlayerPrefs.SetFloat("cit"+i+"bodyColor",newCitizen.bodyColorVal);
			PlayerPrefs.SetFloat("cit"+i+"mouthColor",newCitizen.mouthColorVal);

			PlayerPrefs.SetInt("cit"+i+"headTexture",newCitizen.headTexture);
			PlayerPrefs.SetInt("cit"+i+"bodyTexture",newCitizen.bodyTexture);
			PlayerPrefs.SetInt("cit"+i+"mouthTexture",newCitizen.mouthTexture);

			
			PlayerPrefs.SetFloat("cit"+i+"eyeSpacingY",newCitizen.eyeSpacingY);
			PlayerPrefs.SetFloat("cit"+i+"eyeSpacingX",newCitizen.eyeSpacingX);
			
			PlayerPrefs.SetInt("cit"+i+"job",newCitizen.job);

			Debug.Log ("Saved citizen!");
			newCitizen.Print();
		}
	}

	void genCits(int citNum){
		if (namesTxt != null) {
			names = (namesTxt.text.Split ('\n'));
		}
		for (int i=0; i<citNum; i++) {
			newCitizen ();
		}
	}

	void newCitizen(){
		CitizenInfo newCitizen = ScriptableObject.CreateInstance<CitizenInfo> ();
		//setName
		newCitizen.name= names[Random.Range (0, names.Length)]+" "+alphabet[Random.Range(0,alphabet.Length)]+".";

		newCitizen.healthLevel = 1;
		newCitizen.energyLevel = 1;

		newCitizen.headColorVal =  Random.Range (0.0f, 1.0f);
		newCitizen.headColor = Color.Lerp (minHeadColor, maxHeadColor, newCitizen.headColorVal);
		newCitizen.headTexture = (int) Random.Range (0, headTextures.Count);

		newCitizen.bodyColorVal =  Random.Range (0.0f, 1.0f);
		newCitizen.bodyColor = Color.Lerp (minBodyColor, maxBodyColor, newCitizen.bodyColorVal);
		newCitizen.bodyTexture = (int) Random.Range (0, bodyTextures.Count);
		
		newCitizen.mouthColorVal =  Random.Range (0.0f, 1.0f);
		newCitizen.mouthColor = Color.Lerp (minMouthColor, maxMouthColor, newCitizen.mouthColorVal);
		newCitizen.mouthTexture = (int) Random.Range (0, mouthTextures.Count);


		
		newCitizen.eyeSpacingY = Random.Range (-2.0f, 0.0f);
		newCitizen.eyeSpacingX = Random.Range (10.0f, 25.0f);

		newCitizen.job = Random.Range (0, 3);

		Debug.Log ("New citizen!");
		newCitizen.Print();
		cits.Add(newCitizen);
	}

	void changeCitInfo(){
		citScript.info=cits[currentCit];
		citScript.updateFromInfo();
	}

	public bool nextCitizen(){

		if (currentCit > -1) {
			//save all changes to citizens
			saveCits ();
			cits[currentCit].healthLevel*=0.9f;
			cits[currentCit].energyLevel*=0.9f;
		}
		currentCit++;
		if (currentCit < cits.Count) {
			Debug.Log("Next Citizen");
			cits[currentCit].Print();
			citScript.anim.SetTrigger("Leave");
			Invoke ("changeCitInfo",0.5f);
			return true;
		} else {
			Debug.Log ("End of citizen list");
			return false;
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
 