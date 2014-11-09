using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S605Puppeteer : CutscenePuppeteer {

	private GameObject ChefTony, James;
	private MusicPlayer mus;

	public AudioClip EvilEmergesTheme, LightsActivateSound;

	// Use this for initialization
	void Start () {
		// get all the objects we'll need for the cutscene 
		ChefTony = GameObject.Find ("Chef Tony");
		James = GameObject.Find ("James");

		mus = GameObject.Find ("BGM").GetComponent<MusicPlayer>();

		startTimer();
	}
	
	// Update is called once per frame
	public void FixedUpdate () {
		if(CurrentScene == 3) {
			if(ChefTony.transform.position.x > 11.5f) {
				ChefTony.GetComponent<PlayerFreeze>().Freeze();
				nextScene();
			}
		} else if(CurrentScene == 5) {
			if(James.transform.position.x > 9.5f) {
				James.GetComponent<JamesGasBehavior>().DisableFlightMode();
				nextScene();
			}
		} else if(CurrentScene == 9) {
			if(timerIsGreaterThan(2.5f)) {
				nextScene();
			}
		} else if(CurrentScene == 10) {
			if(timerIsGreaterThan(5.5f)) {
				playSound(LightsActivateSound);
				FadeAndNext(Color.white, 1f, "6-06 Courting death", false);
				nextScene();
			}
		}
	}

	public override void HandleSceneChange() {
		if(CurrentScene == 3) {
			ChefTony.GetComponent<PlayerFreeze>().UnFreeze();
		} else if(CurrentScene == 5) {
			James.transform.position = new Vector3(8.3f, 0, 0);
			James.rigidbody2D.isKinematic = false;
			James.GetComponent<JamesGasBehavior>().EnableFlightMode();
			James.rigidbody2D.AddForce(Vector2.right * 250.0f);
		} else if(CurrentScene == 4) {
			mus.StopMusic(3.0f);
		} else if(CurrentScene == 9) {
			mus.PlayMusic(EvilEmergesTheme, false);
			GetComponent<CutsceneController>().textSpeed = 15;
			startTimer();
		} else if(CurrentScene == 10) {
			startTimer();
		}
	}

}
