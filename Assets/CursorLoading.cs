using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorLoading : MonoBehaviour {

	public float cursorPerc;
	public Image cursorImage;
	public Color color0;
	public Color color1;


	void Start () {
		//hide cursor at beginning
		cursorPerc = 0;
	}

	void Update () {
		//if cursor has some fill
		if (cursorPerc>0) {
			//follow the cursor
			transform.position = Input.mousePosition;
		}
	}

	public void setPerc(float perc){
		//cursorPerc should only be set between 1 and 0
		cursorPerc = Mathf.Clamp (perc, 0, 1);
		//set fill amount with given percentage
		cursorImage.fillAmount=cursorPerc;
		//set color (lerped using percentage given)
		cursorImage.color = Color.Lerp (color0, color1, cursorPerc);
	}

	//for hiding the cursor
	public void disable(){
		cursorPerc = 0;
	}
	
}
