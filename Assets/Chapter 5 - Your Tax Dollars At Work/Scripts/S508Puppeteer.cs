using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S508Puppeteer : CutscenePuppeteer {

	private bool fadeStarted = false;
		
	// Use this for initialization
	void Start () {
		startTimer ();
	}
	
	// Update is called once per frame
	public void FixedUpdate () {
		if (timerIsGreaterThan (3.5f)) {
			if(!fadeStarted) {
				FadeAndNext(Color.black, 10.0f, "6-01 World in ruin", true);
				fadeStarted = true;
			}
		}
	}

	public override void HandleSceneChange() {

	}

}
