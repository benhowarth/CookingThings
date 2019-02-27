using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodManager : MonoBehaviour {
	public GameObject Citizen;
	public CitizenManager citizenManager;
	public GameObject invButton;
	public Order currentOrder;
	public bool lastOrderSuccess;
	public bool waitingForNextOrder;
	public bool finishedServing=false;

	public List<GameObject> ingredients = new List<GameObject> ();
	public Dictionary<int,int> inventory;

	void loadInventory(){
		int foodNo=PlayerPrefs.GetInt("foodCount");
		
		for(int i=0;i<foodNo;i++){
			if(PlayerPrefs.HasKey("food"+i)){
				inventory[i]=PlayerPrefs.GetInt("food"+i);
			}
		}
	}

	void saveInventory(){

		//clear prefs
		int foodNo=PlayerPrefs.GetInt("foodCount");
		
		for(int i=0;i<foodNo;i++){
			if(PlayerPrefs.HasKey("food"+i)){
				PlayerPrefs.DeleteKey("food"+i);
			}
		}

		//save new data
		PlayerPrefs.SetInt("foodCount",ingredients.Count);
		
		foreach(int key in inventory.Keys){
			if(inventory[key]>0){
				PlayerPrefs.SetInt("food"+key,inventory[key]);
			}
		}
	}

	void populateInventoryDict(){
		inventory = new Dictionary<int,int> ();
		for(int i=0;i<ingredients.Count;i++){
			inventory[i]=0;
		}
		if(PlayerPrefs.HasKey ("foodCount")){
			loadInventory();
		}else{
			inventory [0] = 2;
			inventory [1] = 3;
			inventory [2] = 2;
			inventory [3] = 3;
			inventory [4] = 2;
		}
		//also update gui
		UpdateInvUI ();

	}
	void UpdateInvUI(){
		saveInventory ();
		GameObject inv = GameObject.Find ("Inventory");
		foreach(Transform child in inv.transform){
			Destroy (child.gameObject);
		}
		
		int buttonNo = 0;
		foreach(int key in inventory.Keys){
			if(inventory[key]>0){
				GameObject buttonInstance=Instantiate(invButton,inv.transform);
				buttonInstance.transform.SetParent(inv.transform);
				buttonInstance.GetComponent<InventoryButton>().id=key;
				buttonInstance.transform.Find ("InfoText").gameObject.GetComponent<Text>().text=ingredients[key].GetComponent<Ingredient>().info.name;
				buttonInstance.transform.Find ("NumberText").gameObject.GetComponent<Text>().text=inventory[key].ToString();
				buttonNo++;
			}
		}
		if (buttonNo == 0) {
			Debug.Log ("You've run out of ingredients!");
		}
	}
	public void spawnIngredient(int id){
		Instantiate(ingredients[id],transform);
		inventory [id]--;
		UpdateInvUI ();
	}


	public abstract class OrderCondition{
		public abstract bool evaluate (GameObject ingredient);
	}

	public class O_isGreen:OrderCondition{
		public override bool evaluate(GameObject ingredient){
			if (ingredient.GetComponent<Ingredient> ().info.name == "Green") {
				return true;
			} else {
				return false;
			}
		}
	}
	
	public class O_isMonsterFlesh:OrderCondition{
		public override bool evaluate(GameObject ingredient){
			if (ingredient.GetComponent<Ingredient> ().info.name == "Monster Flesh") {
				return true;
			} else {
				return false;
			}
		}
	}

	public class O_isBanana:OrderCondition{
		public override bool evaluate(GameObject ingredient){
			if (ingredient.GetComponent<Ingredient> ().info.name == "Banana") {
				return true;
			} else {
				return false;
			}
		}
	}

	public class O_isBeef:OrderCondition{
		public override bool evaluate(GameObject ingredient){
			if (ingredient.GetComponent<Ingredient> ().info.name == "Beef") {
				return true;
			} else {
				return false;
			}
		}
	}

	public class O_isApple:OrderCondition{
		public override bool evaluate(GameObject ingredient){
			if (ingredient.GetComponent<Ingredient> ().info.name == "Apple") {
				return true;
			} else {
				return false;
			}
		}
	}
	
	
	public class O_isMeat:OrderCondition{
		public override bool evaluate(GameObject ingredient){
			if (ingredient.GetComponent<Ingredient> ().info.tags.Contains("Meat")) {
				return true;
			} else {
				return false;
			}
		}
	}

	public class O_isFruit:OrderCondition{
		public override bool evaluate(GameObject ingredient){
			if (ingredient.GetComponent<Ingredient> ().info.tags.Contains("Fruit")) {
				return true;
			} else {
				return false;
			}
		}
	}

	public class Order{
		public string orderName;
		public List<OrderCondition> conditions=new List<OrderCondition>();
		public Order(string orderString,List<OrderCondition> conditionsParam){
			orderName=orderString;
			conditions=conditionsParam;
		}
		public bool evaluate(Collider[] onPlate){
			bool recipeFailed = false;
			for (int j=0; j<conditions.Count; j++) {
				OrderCondition condition=conditions[j];
				Debug.Log ("Checking requirement"+j);
				for (int i=0; i<onPlate.Length; i++) {
					if(condition.evaluate(onPlate[i].gameObject)){
						Debug.Log("Ingredient"+i+": "+onPlate[i].name+" meets requirement"+j);
						break;
					}else{
						Debug.Log("Ingredient"+i+": "+onPlate[i].name+" doesn't meet requirement"+j);
					}
					if(i==onPlate.Length-1){
						recipeFailed=true;
					}
				}
				if(recipeFailed){break;}
			}
			Debug.Log ("recipe success: " + !recipeFailed);
			return !recipeFailed;
		}
	}
	List<Order> possibleOrders=new List<Order>();


	void initialisePossibleOrders(){
		List<OrderCondition> orderCondTemp=new List<OrderCondition>(){new O_isGreen()};
		possibleOrders.Add (new Order ("something with green",orderCondTemp));
		orderCondTemp=new List<OrderCondition>(){new O_isMonsterFlesh()};
		possibleOrders.Add (new Order ("something with monster flesh",orderCondTemp));
		orderCondTemp=new List<OrderCondition>(){new O_isGreen(),new O_isMonsterFlesh()};
		possibleOrders.Add (new Order ("something with green and monster flesh",orderCondTemp));
		orderCondTemp=new List<OrderCondition>(){new O_isBanana()};
		possibleOrders.Add (new Order ("something with banana",orderCondTemp));
		orderCondTemp=new List<OrderCondition>(){new O_isApple()};
		possibleOrders.Add (new Order ("something with Apple",orderCondTemp));
		orderCondTemp=new List<OrderCondition>(){new O_isBeef()};
		possibleOrders.Add (new Order ("something with Beef",orderCondTemp));
		orderCondTemp=new List<OrderCondition>(){new O_isFruit()};
		possibleOrders.Add (new Order ("something with fruit",orderCondTemp));
		orderCondTemp=new List<OrderCondition>(){new O_isMeat()};
		possibleOrders.Add (new Order ("something with meat",orderCondTemp));
	
	}
	// Use this for initialization
	void Start () {
		populateInventoryDict();
		initialisePossibleOrders();

	}


	Order getOrder(){
		return possibleOrders [Random.Range (0, possibleOrders.Count)];
	}
	// Update is called once per frame
	void Update () {
		
	}

	void nextOrder(){

		//if next citizen
		bool moreCitizens=citizenManager.nextCitizen();
		
		if (!Citizen.activeSelf) {
			//activate citizen
			Citizen.SetActive (true);
		}

		if (!finishedServing && moreCitizens) {
			Debug.Log ("New order for " + Citizen.GetComponent<CitizenScript> ().citizenName + "!");
			//get new order
			waitingForNextOrder = false;
			currentOrder = getOrder ();
			Debug.Log ("They want " + currentOrder.orderName);
		} else {
			Citizen.SetActive (true);
			Debug.Log ("Finished Serving!");
			finishedServing = true;
		}

	}

	public void Serve(){
		if (Citizen.activeSelf) {
			Debug.Log ("Serving up!");
			Vector3 platePos = GameObject.Find ("Plate").transform.position;
			//Collider[] onPlate = Physics.OverlapSphere (platePos,6,LayerMask.GetMask("Player"));
			Collider[] onPlate = Physics.OverlapSphere (platePos, 4, LayerMask.GetMask ("Ingredient"));
			if (onPlate.Length > 0) {
				lastOrderSuccess = currentOrder.evaluate (onPlate);
				for (int i=0; i<onPlate.Length; i++) {
					Destroy (onPlate [i].gameObject);
				}
			} else {
				Debug.Log ("You gave them nothing!");
			}
			waitingForNextOrder = true;
		} else {
			Debug.Log ("Serving begins!");
		}
		Invoke("nextOrder",3f);
	}

	void OnGUI(){

		string drawText="Ring bell to start serving";
		if (currentOrder != null) {
			//Debug.Log (currentOrder);
			//Debug.Log (Citizen);
			drawText = Citizen.GetComponent<CitizenScript> ().citizenName + ": " + currentOrder.orderName;
		}
		if (finishedServing) {
			drawText = "Finished Serving!";
		}
		GUI.Label(new Rect(10, 10, 100, 100), drawText);

		if (waitingForNextOrder) {
			string lastOrderText="Order failed!";
			if(lastOrderSuccess){
				lastOrderText="Order passed!";
			}
			GUI.Label(new Rect(10, 120, 100, 100), lastOrderText);
		}
	}
}
