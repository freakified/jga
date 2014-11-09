using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S610aPuppeteer : CutscenePuppeteer {

		
	// Use this for initialization
	void Start () {
		startTimer ();
	}
	
	// Update is called once per frame
	public void FixedUpdate () {
		if (timerIsGreaterThan (3f)) {
			FadeAndNext(Color.white, 1.0f, "7-01 Fate of James", false);
		}
	}

	public override void HandleSceneChange() {

	}

}
