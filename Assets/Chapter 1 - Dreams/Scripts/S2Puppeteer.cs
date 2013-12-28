using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S2Puppeteer : CutscenePuppeteer {

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
		base.OnEnable();

		BattleController.OnBattleEvent -= HandleBattleEvent;
	}

	
	// Update is called once per frame
	public override void Update () {
		base.Update();

		if(CurrentScene == 0) {
			if(ChefTony.transform.position.x > -1.79) {
				// next: Chef Tony needs disable/enable control methods
				ChefTony.GetComponent<PlayerFreeze>().Freeze();

				//start the cutscene
				nextScene();
			}
		}
			
	}

	public override void HandleSceneChange() {
		// once the text is ready, start the battle
		if(CurrentScene == 4)
			GetComponent<BattleController>().StartBattle();
		else if(CurrentScene == 7)
			ChefTony.GetComponent<PlayerFreeze>().UnFreeze();
	}

	public void HandleBattleEvent(BattleEvent type) {
		if(type == BattleEvent.Finished) {
			nextScene();
		}
	}
}
