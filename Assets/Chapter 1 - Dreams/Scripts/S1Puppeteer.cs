using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S1Puppeteer : CutscenePuppeteer {

	private GameObject ChefTony;

	public AudioClip knifeSlash;

	// Use this for initialization
	void Start () {
		// get all the objects we'll need for the cutscene 
		ChefTony = GameObject.Find ("Chef Tony");
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();

		if(CurrentScene == 3 && timerIsGreaterThan(1)) {
			nextScene();
			stopTimer();
		}
			
	}

	public override void HandleSceneChange() {
		if(CurrentScene <= 5) {
			ChefTony.GetComponent<PlayerControl>().enabled = false;
		} else {
			ChefTony.GetComponent<PlayerControl>().enabled = true;
		}

		if(CurrentScene == 3) {
			ChefTony.rigidbody2D.AddForce(Vector2.right * 1000f);
			playSound(knifeSlash);
			startTimer();
		}
	}
}
