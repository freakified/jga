using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S609Puppeteer : CutscenePuppeteer {

	private GameObject ChefTony, James, LikeMike, FlyingBBall;
	private MusicPlayer mus;
	private BattleController bc;


	// Use this for initialization
	void Start () {
		// get all the objects we'll need for the cutscene 
		ChefTony = GameObject.Find ("Chef Tony");
		James = GameObject.Find ("James");
		LikeMike = GameObject.Find ("Like Mike");
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

	}

	public void HandleBattleEvent(BattleEvent type) {
		switch(type) {
		case BattleEvent.TurnChange:
			
			break;
		case BattleEvent.Finished:
			mus.StopMusic(1.0f);
			nextScene();
			break;
		}
	}

}
