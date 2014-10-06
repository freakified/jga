using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S403Puppeteer : CutscenePuppeteer {

		
	// Use this for initialization
	void Start () {
		startTimer ();
	}
	
	// Update is called once per frame
	public void FixedUpdate () {
		if (timerIsGreaterThan (3.5f)) {
			nextScene();
			FadeAndNext(Color.white, 0.1f, "4-04 Temple Interior 2", false);
		}
	}

	public override void HandleSceneChange() {

	}

}
