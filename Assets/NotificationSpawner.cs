using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationSpawner : MonoBehaviour {

	//base notification to instantiate
	public GameObject notificationObj;

	//spawn notification colored string message (e.g. ("+Beef",Color.red) )
	public void SpawnNotification(string msg,Color color){
		//spawn a new notification here and initialise it
		GameObject notificationInstance=Instantiate (notificationObj,transform);
		notificationInstance.GetComponent<Text> ().text = msg;
		notificationInstance.GetComponent<Text> ().color = color;
	}
}
