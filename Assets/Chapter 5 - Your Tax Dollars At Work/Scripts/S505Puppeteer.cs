using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S505Puppeteer : CutscenePuppeteer {

	private GameObject ChefTony, LikeMike;

	public AudioClip knifeSlash;

	// Use this for initialization
	void Start () {
		// get all the objects we'll need for the cutscene 
		ChefTony = GameObject.Find ("Chef Tony");
		LikeMike = GameObject.Find ("Like Mike");
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

		if(CurrentScene == 3) {
			ChefTony.rigidbody2D.AddForce(Vector2.right * 1000f);
			playSound(knifeSlash);
			startTimer();
		} else if (CurrentScene == 5) {
			LikeMike.rigidbody2D.AddForce(Vector2.one * 150f);
		} else if (CurrentScene == 7) {
			ChefTony.GetComponent<PlayerFreeze>().UnFreeze();
			LikeMike.rigidbody2D.isKinematic = true;
		}
	}
}
