using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carChangeScene : MonoBehaviour {
	public SceneChanger sc;
	public Animator anim;
	public GameObject PlayerModel;
	public PlayerMovement PlayerScript;


	public void ExitLevel(){	
		anim.SetTrigger("Leave");
		PlayerModel.SetActive(false);
		PlayerScript.enabled=false;
		sc.FadeToOtherLevel();
	}
}
