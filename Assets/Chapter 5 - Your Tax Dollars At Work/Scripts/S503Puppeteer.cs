using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S503Puppeteer : CutscenePuppeteer {

	public GameObject PacingGuard;
	private GameObject ChefTony;

	//private Animator ctanim;

	// Use this for initialization
	void Start () {
		// get all the objects we'll need for the cutscene 
		ChefTony = GameObject.Find ("Chef Tony");

		//ctanim = ChefTony.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	public void FixedUpdate () {
		if (CurrentScene == 0) {
			if(ChefTony.transform.position.x > 1.41) {
				ChefTony.GetComponent<PlayerFreeze>().Freeze();
				PacingGuard.transform.localScale = new Vector3(-1, 1, 1);
				PacingGuard.GetComponent<PaceBehavior>().enabled = false;
				nextScene();
			}
		} else if (CurrentScene == 2) {
			if(timerIsGreaterThan(1.0f)) {
				GetComponent<BattleController> ().StartBattle ();
				nextScene();
			}
		}
	}

	public override void HandleSceneChange() {
		if (CurrentScene == 2) {
			ChefTony.rigidbody2D.AddForce(new Vector2(-500.0f, 100.0f));
			startTimer();

		}
	}

}
