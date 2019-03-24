using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour {

	public float seconds=3f;

	//commence destruction in t minus seconds
	public void ActivateDestructionTimer(){
		Destroy (gameObject, seconds);
	}
}
