using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyNavMeshTest : MonoBehaviour {
	private Camera cam;
	private NavMeshAgent agent;
	// Use this for initialization
	void Start () {
		cam = Camera.main;
		agent = GetComponent<NavMeshAgent>();
		agent.destination = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton (0)) {
			Vector3 mPos = Input.mousePosition;
			RaycastHit hit;
			Ray camRay = cam.ScreenPointToRay (mPos);
			if (Physics.Raycast (camRay, out hit, Mathf.Infinity)) {
				Vector3 hitPos = new Vector3 (hit.point.x, transform.position.y, hit.point.z);
				//transform.position = hitPos;
				agent.destination=hitPos;
				Debug.Log (hitPos);
			}
		}
	}
}
