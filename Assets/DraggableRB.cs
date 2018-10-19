using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class DraggableRB : MonoBehaviour
{
	
	private Vector3 screenPoint;
	private Vector3 offset;
	private Vector3 dragOffset;
	private float dragDistance;
	private Ray ray;
	
	void OnMouseDown(){
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit) && hit.transform == transform)
		{
			//Debug.Log("hit");
			dragDistance = hit.distance;
			dragOffset = gameObject.GetComponent<Rigidbody>().transform.position - hit.point;
		}
	}
	
	void OnMouseDrag() {
		
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Vector3 cursorPosition = ray.GetPoint (dragDistance) + dragOffset;
		//gameObject.GetComponent<Rigidbody> ().useGravity = false;
		//gameObject.GetComponent<Rigidbody> ().isKinematic = true;
		//gameObject.GetComponent<Rigidbody> ().MovePosition (cursorPosition);

		Vector3 dir = (cursorPosition - gameObject.GetComponent<Rigidbody> ().position).normalized * 10.0f;
		gameObject.GetComponent<Rigidbody> ().velocity = dir;
	}

	void OnMouseUp(){
		
		//gameObject.GetComponent<Rigidbody> ().useGravity = true;
		//gameObject.GetComponent<Rigidbody>().isKinematic = false;
	}
}