using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class DraggableRB_Old : MonoBehaviour
{
	
	private Vector3 screenPoint;
	private Vector3 offset;
	
	void OnMouseDown(){
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}
	
	void OnMouseDrag() {
		Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
		//transform.position = cursorPosition;
		gameObject.GetComponent<Rigidbody>().useGravity = false;
		gameObject.GetComponent<Rigidbody>().isKinematic = true;
		gameObject.GetComponent<Rigidbody>().MovePosition(cursorPosition);
		//gameObject.GetComponent<Rigidbody>().velocity = cursorPosition * 5.0f * Time.deltaTime; 
	}
	
	void OnMouseUp(){
		
		gameObject.GetComponent<Rigidbody> ().useGravity = true;
		gameObject.GetComponent<Rigidbody>().isKinematic = false;
	}
}