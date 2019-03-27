using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationSpawner : MonoBehaviour {

	public GameObject notificationObj;

	void Update(){
		if (Input.GetKey (KeyCode.N)) {
			SpawnNotification("test notification",Color.green);
		}
	}

	public void SpawnNotification(string msg,Color color){
		GameObject notificationInstance=Instantiate (notificationObj,transform);
		notificationInstance.GetComponent<Text> ().text = msg;
		notificationInstance.GetComponent<Text> ().color = color;
	}
}
