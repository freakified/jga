using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S1Puppeteer : CutscenePuppeteer {

	private GameObject ChefTony;
	private GameObject Guard;
	
	public AudioClip knifeSlash;

	// Use this for initialization
	void Start () {
		// get all the objects we'll need for the cutscene 
		ChefTony = GameObject.Find ("Chef Tony");
		Guard = GameObject.Find ("Guard");
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
		if(CurrentScene <= 4) {
			ChefTony.GetComponent<PlayerControl>().enabled = false;
		} else {
			ChefTony.GetComponent<PlayerControl>().enabled = true;
		}

		if(CurrentScene == 3) {
			ChefTony.rigidbody2D.AddForce(Vector2.right * 600f);
			playSound(knifeSlash);
			startTimer();
		}
	}
}
