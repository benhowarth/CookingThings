using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName="CitizenInfo",menuName="CitizenInfo")]
public class CitizenInfo : ScriptableObject {
	public string name;
	public float healthLevel;
	public enum Job{Civilian, Engineer, Doctor};
	public Job job;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
