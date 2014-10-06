using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S202Puppeteer : CutscenePuppeteer {

	public AudioClip phoneRing;
	public AudioClip phonePickup;

	private GameObject ChefTony;

	private Animator ctanim;

	private float cameraSpeed = -1.5f;
	private Vector3 cameraPosition;
		
	// Use this for initialization
	void Start () {
		// get all the objects we'll need for the cutscene 
		ChefTony = GameObject.Find ("Chef Tony");
		ctanim = ChefTony.GetComponent<Animator>();
		

		ctanim.SetInteger("HP", 0);
		startTimer();
	}
	
	// Update is called once per frame
	public void FixedUpdate () {
		if(CurrentScene == 0) {
			// camera pan in effect
		 	if(Camera.main.transform.position.z > -10) {
				cameraPosition = Camera.main.transform.position;
				cameraPosition.z += cameraSpeed * Time.fixedDeltaTime;
				Camera.main.transform.position = cameraPosition;
			} else {
				startTimer();
				nextScene();
			}
		} else if(CurrentScene == 1) {
			if(timerIsGreaterThan(1)) {
				stopTimer();
				ctanim.SetInteger("HP", 100);
				nextScene();
			}
		} else if(CurrentScene == 4) {
			if(ChefTony.transform.position.x > -2.2f) {
				ChefTony.GetComponent<PlayerFreeze>().Freeze();
				nextScene();
			}
		} else if(CurrentScene == 7) {
			if(ChefTony.transform.position.x > 0.38f) {
				ChefTony.GetComponent<PlayerFreeze>().Freeze();
				playSound(phonePickup);
				FadeAndNext(Color.black, 2, "2-03 Inside Payphone", false);
				nextScene();
			}
		} 
	}

	public override void HandleSceneChange() {

		if(CurrentScene == 4) {
			ChefTony.GetComponent<PlayerFreeze>().UnFreeze();
		} else if(CurrentScene == 5) {
			playSound(phoneRing);
		} else if(CurrentScene == 7) {
			ChefTony.GetComponent<PlayerFreeze>().UnFreeze();
		}
	}

}
