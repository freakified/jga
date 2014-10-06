using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S001Puppeteer : CutscenePuppeteer {

	public float Delay = 1.0f;
		
	// Use this for initialization
	void Start () {
		startTimer();
	}
	
	// Update is called once per frame
	public void FixedUpdate () {
		if(CurrentScene == 0) {
			if(timerIsGreaterThan(5.0f)) {
				StartCoroutine(FadeAndNext(Color.black, 5, "0-03 Main Menu"));
				nextScene();
			}
		}

	}

	public override void HandleSceneChange() {

	}

}
