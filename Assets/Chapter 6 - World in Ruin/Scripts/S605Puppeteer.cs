using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S605Puppeteer : CutscenePuppeteer {

	private GameObject ChefTony, James;
	private MusicPlayer mus;

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
			if(ChefTony.transform.position.x > 4.2f) {
				ChefTony.GetComponent<PlayerFreeze>().Freeze();
				nextScene();
			}
		} else if(CurrentScene == 5) {
			if(James.transform.position.x > 2.3f) {
				James.GetComponent<JamesGasBehavior>().DisableFlightMode();
				nextScene();
			}
		}
	}

	public override void HandleSceneChange() {
		if(CurrentScene == 3) {
			ChefTony.GetComponent<PlayerFreeze>().UnFreeze();
		} else if(CurrentScene == 5) {
			James.transform.position = new Vector3(1.0f, 0, 0);
			James.rigidbody2D.isKinematic = false;
			James.GetComponent<JamesGasBehavior>().EnableFlightMode();
			James.rigidbody2D.AddForce(Vector2.right * 250.0f);
		}
	}

}
