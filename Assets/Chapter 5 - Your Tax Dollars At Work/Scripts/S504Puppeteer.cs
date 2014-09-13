using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S504Puppeteer : CutscenePuppeteer {

	private GameObject ChefTony;

	// Use this for initialization
	void Start () {
		// get all the objects we'll need for the cutscene 
		ChefTony = GameObject.Find ("Chef Tony");

	}

	public override void OnEnable() {
		base.OnEnable();
		
		BattleController.OnBattleEvent += HandleBattleEvent;
	}
	
	
	public override void OnDisable() {
		base.OnDisable();
		
		BattleController.OnBattleEvent -= HandleBattleEvent;
	}
	
	// Update is called once per frame
	public void FixedUpdate () {
		if (CurrentScene == 0) {
			if(ChefTony.transform.position.x > 1.2f) {
				ChefTony.GetComponent<PlayerFreeze>().Freeze();
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
		} else if (CurrentScene == 5) {
			ChefTony.GetComponent<PlayerFreeze>().UnFreeze();
		}
	}

	public void HandleBattleEvent(BattleEvent type) {
		switch(type) {
		case BattleEvent.Finished:
			nextScene();

			break;
		}
	}

}
