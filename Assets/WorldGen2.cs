using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldGen2 : MonoBehaviour {
	public float tileSize;
	public int gridSize;
	public GameObject GroundTile;
	public GameObject RoadTile;
	public GameObject HouseTile;
	public GameObject WallObj;
	public GameObject Crate;
	public GameObject Enemy;
	public GameObject LaserTrigger;
	public int[,] tiles;
	//public List<List<int[,]>> buildings;



	List<Vector2> walkersPos=new List<Vector2>();
	List<Vector2> walkersDir=new List<Vector2>();

	List<Vector2> deadEnds=new List<Vector2>();


	void paintTile(int y,int x){
		tiles[y,x]=1;
	}
	void paintTiles(int y,int x,int brushSize){
		int curX = x-brushSize;
		int curY;
		while (curX<=x+brushSize) {
			curY = y-brushSize;
			while(curY<=y+brushSize){
				if (curX > 0 && curX < gridSize - 1 && curY > 0 && curY < gridSize - 1) {
					paintTile (curY, curX);
				}
				curY+=1;
			}
			curX+=1;
		}
	}

	List<int> getN(int y, int x,int[,] tiles, bool manhattan){
		List<int> neighbours=new List<int>();
		//if has top
		if (y > 0) {neighbours.Add(tiles[y-1,x]);}
		//if has bottom
		if (y < gridSize-1) {neighbours.Add(tiles[y+1,x]);}
		//if has left
		if (x > 0) {neighbours.Add(tiles[y,x-1]);}
		//if has right
		if (x < gridSize-1) {neighbours.Add(tiles[y,x+1]);}
		
		if (!manhattan) {
			//if has top right
			if (y > 0 && x < gridSize-1) {neighbours.Add(tiles[y-1,x+1]);}
			//if has bottom right
			if (y < gridSize-1 && x < gridSize-1) {neighbours.Add(tiles[y+1,x+1]);}
			//if has top left
			if (y > 0 && x > 0) {neighbours.Add(tiles[y-1,x-1]);}
			//if has bottom left
			if (y < gridSize-1 && x > 0) {neighbours.Add(tiles[y+1,x-1]);}
			
		}
		
		
		return neighbours;
	}

	int getNCount(int val, int y, int x, int[,] tiles, bool manhattan){
		int num = 0;
		List<int> neighbours=getN (y,x,tiles,manhattan);
		foreach(int neighbour in neighbours){
			if(neighbour==val){num++;}
		}
		return num;
	}
	// Use this for initialization
	void Start () {
		genMap ();
		displayMap ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.R)) {
			genMap();
			displayMap();
		}
	}
	
	void genMap(){
		tiles=new int[gridSize,gridSize];
		for (int y=0; y<gridSize; y++) {
			for (int x=0; x<gridSize; x++) {
				tiles[y,x]=0;
				/*if(Random.Range(0f,1f)>0.8){
					tiles[y,x]=1;
				}*/
			}
		}


		//initialise 4x4 square for start
		for (int y=0; y<4; y++) {
			for (int x=0; x<4; x++) {
				tiles [y, x] = 1;
			}
		}


		int walkerLim = 15;
		walkersPos.Add (new Vector2(3,3));
		walkersDir.Add (new Vector2 (1, 0));
		for (int i=0; i<70; i++) {
			for(int j=0;j<walkersPos.Count;j++){
				Vector2 wPos=walkersPos[j];
				paintTiles((int)wPos.y,(int)wPos.x,Random.Range(0,Random.Range (0,Random.Range(1,2))));
				if(Random.Range (0f,1f)>0.5){
					if(Random.Range (0f,1f)>0.5){
						walkersDir[j]=new Vector2(0,1);
					}else{
						walkersDir[j]=new Vector2(1,0);
					}
				}
				walkersPos[j]=wPos+walkersDir[j];
				if(Random.Range (0f,1f)>0.8 && walkersPos.Count<walkerLim){
					walkersPos.Add (new Vector2(walkersPos[j].x,walkersPos[j].y));
					walkersDir.Add (new Vector2(walkersDir[j].x,walkersDir[j].y)*-1);
				}
			}
		}
		/*
		for (int i=0; i<3; i++) {
			for (int y=3; y<gridSize; y++) {
				for (int x=3; x<gridSize; x++) {
					if (tiles [y, x] == 0 && getN (y, x, tiles, true).Contains (1)) {
						if (Random.Range (0f, 1f) > 0.5) {
							tiles [y, x] = 1;
						}
					}
					else if(tiles[y,x]==1 && getNCount(0,y, x, tiles, true)>0){
						if (Random.Range (0f, 1f) > 0.8) {
							tiles [y, x] = 0;
						}
					}
				}
			}
		}
		*/


		//make outer walls of 'cave'
		for (int y=0; y<gridSize; y++) {
			for (int x=0; x<gridSize; x++) {
				if (tiles [y, x] == 0 && getN (y, x, tiles, false).Contains (1)) {
					tiles [y, x] = 2;
				}
			}
		}

		//remove gaps
		for (int y=0; y<gridSize; y++) {
			for (int x=0; x<gridSize; x++) {
				if (tiles [y, x] == 1 && getNCount (2,y, x, tiles, true)==4) {
					tiles [y, x] = 2;
				}
			}
		}

		//add outer walls
		/*
		for (int y=0; y<gridSize; y++) {
			tiles[y,0]=2;
			tiles[y,gridSize-1]=2;
		}
		for (int x=0; x<gridSize; x++) {
			tiles[0,x]=2;
			tiles[gridSize-1,x]=2;
		}
		*/
		tiles [0,0] = 0;
		tiles [1,0] = 0;
		tiles [0,1] = 0;
		
		
		
	}
	void displayMap(){
		//clear
		foreach(Transform child in transform){
			Destroy (child.gameObject);
		}
		
		GameObject tile=GroundTile;
		for (int y=0; y<gridSize; y++) {
			for (int x=0; x<gridSize; x++) {
				if(tiles[y,x]==0){
					tile=GroundTile;
				}else if(tiles[y,x]==1){
					tile=RoadTile;
				}else if(tiles[y,x]==2){
					tile=HouseTile;
				}
				GameObject tileInstance=Instantiate(tile,transform);
				tileInstance.transform.position=new Vector3(transform.position.x+x*tileSize,transform.position.y,transform.position.z+y*tileSize);
				if(tile==HouseTile){
					GameObject houseInstance=Instantiate(WallObj,transform);
					houseInstance.transform.position=new Vector3(transform.position.x+x*tileSize,transform.position.y+1.75f,transform.position.z+y*tileSize);
				}
			}
		}
		for (int y=1; y<gridSize-1; y++) {
			for (int x=1; x<gridSize-1; x++) {
				if(tiles[y,x]==0 && ((tiles[y+1,x]==1&&tiles[y-1,x]==1)||(tiles[y,x+1]==1&&tiles[y,x-1]==1))){
					if(Random.Range(0f,1f)>0.00){
						Debug.Log("new laser boi!");
						Vector3 laserPos=new Vector3(transform.position.x+x*tileSize,transform.position.y+3f,transform.position.z+y*tileSize);
						Instantiate(LaserTrigger,laserPos,Quaternion.identity);
					}
				}
			}
		}

		for (int y=0; y<gridSize; y++) {
			for (int x=0; x<gridSize; x++) {
				//if above (1,1) and is a road tile 
				if(x>1 && y>1 && tiles[y,x]==1 && Random.Range (0f,1f)>0.96){
					Vector3 enemyTransformPosition=new Vector3(transform.position.x+x*tileSize,transform.position.y+3f,transform.position.z+y*tileSize);
					Instantiate(Enemy,enemyTransformPosition,Quaternion.identity);
				}
			}
		}

		
		
		for (int i=0; i<walkersPos.Count; i++) {
			Instantiate(Crate,new Vector3(transform.position.x+(walkersPos[i].x-walkersDir[i].x)*tileSize,transform.position.y+4,transform.position.z+(walkersPos[i].y-walkersDir[i].y)*tileSize),Quaternion.Euler (0,0,0));
		}

	}
}
