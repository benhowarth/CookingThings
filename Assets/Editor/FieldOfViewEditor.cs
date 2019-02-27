using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(FieldOfView))]
public class FieldOfViewEditor : Editor {

	void OnSceneGUI(){
		FieldOfView fov = (FieldOfView)target;
		Handles.color = Color.white;
		Handles.DrawWireArc(fov.transform.position,Vector3.up,Vector3.forward,360,fov.viewRad);
		Vector3 viewAngleA = FieldOfView.DirFromAngle (-fov.viewAngle / 2, false,fov.transform.rotation.eulerAngles);
		Vector3 viewAngleB = FieldOfView.DirFromAngle (fov.viewAngle / 2, false,fov.transform.rotation.eulerAngles);

		
		Handles.DrawLine (fov.transform.position, fov.transform.position + viewAngleA * fov.viewRad);
		Handles.DrawLine (fov.transform.position, fov.transform.position + viewAngleB * fov.viewRad);

		Handles.color = Color.red;
		foreach (Transform visibleTarget in fov.visibleTargets) {
			Handles.DrawLine(fov.transform.position,visibleTarget.position);
		}
	}
}
