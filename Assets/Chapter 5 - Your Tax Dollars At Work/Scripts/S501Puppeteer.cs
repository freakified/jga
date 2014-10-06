using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S501Puppeteer : CutscenePuppeteer {

	private GameObject ChefTony;

	//private Animator ctanim;

	// Use this for initialization
	void Start () {
		// get all the objects we'll need for the cutscene 
		ChefTony = GameObject.Find ("Chef Tony");
	}
	
	// Update is called once per frame
	public void FixedUpdate () {
		if (CurrentScene == 4) {
			if (ChefTony.transform.position.x > 1.2f) {
				ChefTony.GetComponent<PlayerFreeze>().Freeze();
				FadeAndNext(Color.black, 2.0f, "5-02 Outside Facility 2", false);
				nextScene();
			}
		}
	}

	public override void HandleSceneChange() {
		if (CurrentScene == 4) {
			ChefTony.GetComponent<PlayerFreeze>().UnFreeze();
		}
	}

}
