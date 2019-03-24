using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorLoading : MonoBehaviour {

	public float cursorPerc;
	public Image cursorImage;
	public Color color0;
	public Color color1;
	// Use this for initialization
	void Start () {
		cursorPerc = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (cursorPerc>0) {
			transform.position = Input.mousePosition;
		}
	}
	public void setPerc(float perc){
		//cursorPerc should only be set between 1 and 0
		cursorPerc = Mathf.Clamp (perc, 0, 1);
		cursorImage.fillAmount=cursorPerc;
		cursorImage.color = Color.Lerp (color0, color1, cursorPerc);
	}
	public void disable(){
		cursorPerc = 0;
	}
	
}
