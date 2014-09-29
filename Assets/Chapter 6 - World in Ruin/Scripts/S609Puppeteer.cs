using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S609Puppeteer : CutscenePuppeteer {

	private GameObject FlyingBBall;
	private MusicPlayer mus;
	private BattleController bc;


	// Use this for initialization
	void Start () {
		// get all the objects we'll need for the cutscene 
		FlyingBBall = GameObject.Find ("Flying Basketball");

		mus = GameObject.Find ("BGM").GetComponent<MusicPlayer>();
		bc = GetComponent<BattleController>();

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

	}

	public override void HandleSceneChange() {
		if(CurrentScene == 2) {
			bc.ResumeBattle();
		}
	}

	public void HandleBattleEvent(BattleEvent type) {
		switch(type) {
		case BattleEvent.TurnChange:
			if(CurrentScene == 0) {
				bc.PauseBattle();
				nextScene();
			}

			break;
		case BattleEvent.Finished:
			mus.StopMusic(1.0f);
			nextScene();
			break;
		}
	}

}
