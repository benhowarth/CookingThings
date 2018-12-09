using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGen : MonoBehaviour {
	public float tileSize;
	public int gridSize;
	public GameObject GroundTile;
	public GameObject RoadTile;
	public GameObject HouseTile;
	public GameObject HouseObj;
	public int[,] tiles;
	public List<List<int[,]>> buildings;
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
				//tiles[y,x]=(int) Mathf.Floor(Random.Range(0.0f,2.0f));
			}
		}
		
		
		//draw road
		int roadCount = 0;
		/*int yMult = Random.Range (0f, 1f) > 0.5 ? 1 : -1;	
		int xMult = Random.Range (0f, 1f) > 0.5 ? 1 : -1;
		int curTileX = yMult==1?0:gridSize-1;
		int curTileY = xMult==1?0:gridSize-1;*/
		
		int yMult = 1;
		int curTileY = 0;
		//if(Random.Range(0f,1f)>0.5){yMult=-1;curTileY=gridSize-1;}
		int xMult = 1;
		int curTileX = 0;
		//if(Random.Range(0f,1f)>0.5){xMult=-1;curTileX=gridSize-1;}
		//Debug.Log (yMult);
		//Debug.Log (xMult);
		//Debug.Log (curTileY);
		//Debug.Log (curTileX);
		while(roadCount<1000 && curTileY<gridSize-1 && curTileY>=0 && curTileX<gridSize-1 && curTileX>=0){
			tiles[curTileY,curTileX]=1;
			if(Random.Range(0.0f,1.0f)>0.5f){
				curTileY+=yMult;
			}else{
				curTileX+=xMult;
			}
			roadCount++;
		}

		curTileX = (int) Mathf.Floor (Random.Range(gridSize*0.1f,gridSize*0.9f));
		curTileY = (int) Mathf.Floor (Random.Range(gridSize*0.1f,gridSize*0.9f));
		//Debug.Log (tiles [curTileY,curTileX]);
		while (tiles[curTileY,curTileX]==0) {
			curTileX = (int) Mathf.Floor (Random.Range(gridSize*0.1f,gridSize*0.9f));
			curTileY = (int) Mathf.Floor (Random.Range(gridSize*0.1f,gridSize*0.9f));
			//Debug.Log (tiles [curTileY,curTileX]);
		}


		roadCount = 0;
		while(roadCount<1000 && curTileY<gridSize-1 && curTileY>=0 && curTileX<gridSize-1 && curTileX>=0){
			tiles[curTileY,curTileX]=1;
			if(Random.Range(0.0f,1.0f)>0.5f){
				curTileY++;
			}else{
				curTileX-=xMult;
			}
			roadCount++;
		}
		
		//make road thicker
		int[,] tilesOld = (int[,]) tiles.Clone();
		for (int y=0; y<gridSize; y++) {
			for (int x=0; x<gridSize; x++) {
				List<int> n=getN(y,x,tilesOld,false);
				if(tilesOld[y,x]==0 && n.Contains(1)){
					tiles[y,x]=1;
				}
				//tiles[y,x]=(int) Mathf.Floor(Random.Range(0.0f,2.0f));
			}
		}
		
		//get house tiles
		for (int y=0; y<gridSize; y++) {
			for (int x=0; x<gridSize; x++) {
				List<int> n=getN(y,x,tiles,false);
				if(tiles[y,x]==0 && n.Contains(1)){
					if(Random.Range(0.0f,1.0f)>0.8f){
						tiles[y,x]=2;
					}
				}
				//tiles[y,x]=(int) Mathf.Floor(Random.Range(0.0f,2.0f));
			}
		}



		//random house blobs
		for (int j=0; j<20; j++) {
			curTileX = (int)Mathf.Floor (Random.Range (gridSize * 0.1f, gridSize * 0.9f));
			curTileY = (int)Mathf.Floor (Random.Range (gridSize * 0.1f, gridSize * 0.9f));
			//Debug.Log (tiles [curTileY,curTileX]);
			while (tiles[curTileY,curTileX]!=0) {
				curTileX = (int)Mathf.Floor (Random.Range (gridSize * 0.1f, gridSize * 0.9f));
				curTileY = (int)Mathf.Floor (Random.Range (gridSize * 0.1f, gridSize * 0.9f));
				//Debug.Log (tiles [curTileY,curTileX]);
			}
			tiles[curTileY,curTileX]=2;
		}



		//make houses thicker
		for (int j=0; j<2; j++) {
			tilesOld = (int[,])tiles.Clone ();
			for (int y=0; y<gridSize; y++) {
				for (int x=0; x<gridSize; x++) {
					List<int> n = getN (y, x, tilesOld, false);
					if (tilesOld [y, x] == 0 && n.Contains (2)) {
						if (Random.Range (0.0f, 1.0f) > 0.6f) {
							tiles [y, x] = 2;
						}
					}
					//tiles[y,x]=(int) Mathf.Floor(Random.Range(0.0f,2.0f));
				}
			}
		}

		//get rid of holes in house blocks
		for (int j=0; j<3; j++) {
			tilesOld = (int[,])tiles.Clone ();
			for (int y=0; y<gridSize; y++) {
				for (int x=0; x<gridSize; x++) {
					List<int> n = getN (y, x, tilesOld, false);
					int count=0;
					foreach (int nb in n.FindAll(i => i==2)){
						count ++;
					}
					if (tilesOld [y, x] == 0 && count>=4) {
						tiles [y, x] = 2;
					}
					else if(tilesOld[y,x]==2 && count<=2){
						tiles[y,x]=0;
					}
					//tiles[y,x]=(int) Mathf.Floor(Random.Range(0.0f,2.0f));
				}
			}
		}

		//get building regions





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
				/*if(tile==HouseTile){
					GameObject houseInstance=Instantiate(HouseObj,transform);
					houseInstance.transform.position=new Vector3(transform.position.x+x*tileSize,transform.position.y,transform.position.z+y*tileSize);
					houseInstance.name="House("+x+","+y+")";
					houseInstance.GetComponent<deleteWallOnGen>().x=x;
					houseInstance.GetComponent<deleteWallOnGen>().y=y;
					houseInstance.GetComponent<deleteWallOnGen>().CheckWalls();
				}*/
			}
		}
	}
}
