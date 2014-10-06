using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S401Puppeteer : CutscenePuppeteer {

	private GameObject ChefTony;

	//private Animator ctanim;

	// Use this for initialization
	void Start () {
		// get all the objects we'll need for the cutscene 
		ChefTony = GameObject.Find ("Chef Tony");
		//ctanim = ChefTony.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	public void FixedUpdate () {
		if(CurrentScene == 4) {
			if(ChefTony.transform.position.x > 10.0f) {
				ChefTony.GetComponent<PlayerFreeze>().Freeze();
				nextScene();
			}
		} else if(CurrentScene == 7) {
			if(ChefTony.transform.position.x > 13.85f) {
				ChefTony.GetComponent<PlayerFreeze>().Freeze();
				FadeAndNext(Color.black, 2, "4-02 Temple Interior", true);
				
				nextScene();
			}
		} 
	}

	public override void HandleSceneChange() {
		if(CurrentScene == 4 || CurrentScene == 7) {
			ChefTony.GetComponent<PlayerFreeze>().UnFreeze();
		}
	}

}
