using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S601Puppeteer : CutscenePuppeteer {

	public AudioClip RainSound;

	private GameObject ChefTony;
	private MusicPlayer mus;

	private Animator ctanim;

	// Use this for initialization
	void Start () {
		// get all the objects we'll need for the cutscene 
		ChefTony = GameObject.Find ("Chef Tony");
		ctanim = ChefTony.GetComponent<Animator>();
		mus = GameObject.Find ("BGM").GetComponent<MusicPlayer>();


		playSound(RainSound);
		ctanim.SetInteger("HP", 0);
		startTimer();
	}
	
	// Update is called once per frame
	public void FixedUpdate () {
		if (CurrentScene == 0) {
			if(timerIsGreaterThan(8.0f)) {
				// wake up, mr. tony. wake up and smell...the ashes
				ctanim.SetInteger("HP", 100);
			}

			if(timerIsGreaterThan(10.0f)) {
				//start the cutscene
				nextScene();
			}
		}
	}

	public override void HandleSceneChange() {
		if (CurrentScene == 8) {
			ChefTony.GetComponent<PlayerFreeze>().UnFreeze();
		}
	}

}
