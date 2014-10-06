using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S609aPuppeteer : CutscenePuppeteer {

		
	// Use this for initialization
	void Start () {
		startTimer ();
	}
	
	// Update is called once per frame
	public void FixedUpdate () {
		if (timerIsGreaterThan (5f)) {
			FadeAndNext(Color.white, 1.0f, "6-10 Atonement", false);
		}
	}

	public override void HandleSceneChange() {

	}

}
