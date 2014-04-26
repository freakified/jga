using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S301Puppeteer : CutscenePuppeteer {

	private GameObject ChefTony;
		
	// Use this for initialization
	void Start () {
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

//		if(CurrentScene == 0) {
//			ChefTony.GetComponent<PlayerFreeze>().Freeze();
//			GetComponent<BattleController>().StartBattle();
//			nextScene();
//		}
		
	}

	public override void HandleSceneChange() {

	}
	
	public void HandleBattleEvent(BattleEvent type) {
		if(type == BattleEvent.Finished) {
			nextScene();
		}
	}
}
