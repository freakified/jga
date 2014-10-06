using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S300Puppeteer : CutscenePuppeteer {

		
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	public void FixedUpdate () {

		if(CurrentScene == 3) {
			FadeAndNext(Color.black, 0, "3-10 Orphanage", false);
			nextScene();
		}
		
	}

	public override void HandleSceneChange() {

	}

}
