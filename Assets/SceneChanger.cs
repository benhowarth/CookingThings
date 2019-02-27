using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

	public Animator animator;
	private int levelToLoad;

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.L)) {
			FadeToOtherLevel();
		}
	}

	public void FadeToLevel(int levelIndex){
		levelToLoad = levelIndex;
		animator.SetTrigger ("FadeOut");

	}
	public void FadeToOtherLevel(){
		int index = SceneManager.GetActiveScene ().buildIndex;
		if (index == 0) {
			//save people
			FadeToLevel (1);
		} else {
			//save inventory
			FadeToLevel(0);
		}
	}
	public void OnFadeComplete(){
		SceneManager.LoadScene (levelToLoad);
		//enter cafeteria
		if (levelToLoad == 0) {
			//load inventory
			//load people
		}else if (levelToLoad == 1) {
			//load inventory
		}

	}
}
