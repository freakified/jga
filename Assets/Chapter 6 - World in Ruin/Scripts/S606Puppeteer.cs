using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S606Puppeteer : CutscenePuppeteer {

	public AudioClip DariasTheme;

	private GameObject ChefTony, James, Daria, LMFB;
	private MusicPlayer mus;
	private BattleController bc;


	// Use this for initialization
	void Start () {
		// get all the objects we'll need for the cutscene 
		ChefTony = GameObject.Find ("Chef Tony");
		LMFB = GameObject.Find ("LMFB");
		James = GameObject.Find ("James");
		Daria = GameObject.Find ("Daria");
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
		if(CurrentScene == 6) {
			Daria.rigidbody2D.AddForce(Vector2.right * -500);
		} else if(CurrentScene == 11) {
			LMFB.rigidbody2D.AddForce(new Vector2(1.0f, 0.5f) * 200);
			bc.StartBattle();
			mus.PlayMusic(DariasTheme, true);
		} else if(CurrentScene == 15) {
			bc.ResumeBattle();
		}
	}

	public void HandleBattleEvent(BattleEvent type) {
		switch(type) {
		case BattleEvent.TurnChange:
			if(CurrentScene == 11) {
				// after daria's first laser strike
				bc.PauseBattle();
				nextScene();
			} 
			
			break;
		}
	}

}
