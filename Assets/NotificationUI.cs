using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationUI : MonoBehaviour {
	//max time notification exists for
	public float ageMax=2f;
	public float age;
	//vec3 velocity
	public Vector3 speed=new Vector3(0,1,0);
	private Text textComponent;


	void Start(){
		//init age timer
		age = 0f;
		//get text component
		textComponent = GetComponent<Text> ();
	}

	void Update () {
		//if notification under maximum age
		if (age < ageMax) {
			//fade out text on notification
			Color.Lerp (textComponent.color, Color.clear, age/ageMax);
			//move using on velocity
			transform.position += speed * Time.deltaTime;
			age += Time.deltaTime;
		//kill notification
		} else {
			Destroy(gameObject);
		}
	}
}
