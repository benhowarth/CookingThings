using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deleteWallOnGen : MonoBehaviour {
	public int x;
	public int y;
	public GameObject crate;
	public GameObject enemy;
	// Use this for initialization
	void Start () {
		if (Random.Range (0.0f, 1.0f) > 0.3f) {
			Instantiate(crate,transform.Find("SpawnPoint").position,Quaternion.Euler(0,0,0),GameObject.Find ("WorldGen").transform);
		} else if (Random.Range (0.0f, 1.0f) > 0.9f) {
			Instantiate(enemy,transform.Find("SpawnPoint").position,Quaternion.Euler(0,180,0),GameObject.Find ("WorldGen").transform);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CheckWalls(){
		//Debug.Log (gameObject.name+" checking walls");
		int[,] tiles = GameObject.Find ("WorldGen").GetComponent<WorldGen> ().tiles;
		int gridSize = GameObject.Find ("WorldGen").GetComponent<WorldGen> ().gridSize;
		
		//if has top
		if (y > 0) {
			//Debug.Log ("checking top: "+(y-1)+","+x);
			if(tiles[y-1,x]==2){
				//Debug.Log ("Destroy");
				//Destroy(transform.Find("BackWall").gameObject);
			}
		}
		//if has bottom
		if (y < gridSize-1) {
			//Debug.Log ("checking bottom: "+(y+1)+","+x);
			if(tiles[y+1,x]==2){
				//Debug.Log ("Destroy");
				Destroy(transform.Find("BackWall").gameObject);
			}
		}
		//if has left
		if (x > 0) {
			
			//Debug.Log ("checking left: "+y+","+(x-1));
			if(tiles[y,x-1]==2){
				//Debug.Log ("Destroy");
				Destroy(transform.Find("LeftWall").gameObject);
			}
		}
		//if has right
		if (x < gridSize-1) {
			
			//Debug.Log ("checking right: "+y+","+(x+1));
			if(tiles[y,x+1]==2){
				//Debug.Log ("Destroy");
				Destroy(transform.Find("RightWall").gameObject);
			}
		}
	}


}
