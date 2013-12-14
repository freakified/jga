using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S2Puppeteer : CutscenePuppeteer {

	private GameObject ChefTony;
	
	// Use this for initialization
	void Start () {
		// get all the objects we'll need for the cutscene 
		ChefTony = GameObject.Find ("Chef Tony");
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();

		if(CurrentScene == 0) {
			if(ChefTony.transform.position.x > -1.79) {
				ChefTony.GetComponent<PlayerControl>().enabled = false;
				nextScene();
			}
		}
			
	}

	public override void HandleSceneChange() {
		// intentionally blank
	}
}
