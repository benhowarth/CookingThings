using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationUI : MonoBehaviour {
	public float ageMax=2f;
	public float age;
	public Vector3 speed=new Vector3(0,1,0);
	private Text textComponent;


	void Start(){
		age = 0f;
		textComponent = GetComponent<Text> ();
	}
	// Update is called once per frame
	void Update () {
		if (age < ageMax) {
			Color.Lerp (textComponent.color, Color.clear, age/ageMax);
			transform.position += speed * Time.deltaTime;
			age += Time.deltaTime;
		} else {
			Destroy(gameObject);
		}
	}
}
