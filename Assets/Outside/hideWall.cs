using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hideWall : MonoBehaviour {
	private Camera cam;
	private GameObject Player;
	public Material normalWallMat;
	public Material transWallMat;
	// Use this for initialization
	void Start () {
		cam = Camera.main;
		Player=GameObject.Find ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Renderer>().material=normalWallMat;
		Vector3 mPos = Input.mousePosition;
		RaycastHit hit;
		Vector3 PlayerFeetPos = Player.transform.position;
		PlayerFeetPos.y = 0.0f;
		Ray camRay = new Ray(cam.transform.position,PlayerFeetPos-cam.transform.position);
		if (Physics.Raycast (camRay, out hit, Mathf.Infinity,LayerMask.GetMask("Player","Wall"))) {
			//Vector3 hitPos = new Vector3 (hit.point.x, transform.position.y, hit.point.z);
			//transform.position = hitPos;
			Debug.DrawRay (camRay.origin, camRay.direction *  50, Color.yellow);
			if(hit.transform==transform){
				//GetComponent<MeshRenderer>().enabled=false;
				GetComponent<Renderer>().material=transWallMat;
			}
		}
	}
}
