using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S607Puppeteer : CutscenePuppeteer {

	public AudioClip GasBlastSound1, GasBlastSound2;
	public GameObject SparksPrefab;

	private GameObject ChefTony, James, LMFB, Gasplosion, HatGlow, ScreenFlash;
	private MusicPlayer mus;
	private BattleController bc;


	// Use this for initialization
	void Start () {
		// get all the objects we'll need for the cutscene 
		ChefTony = GameObject.Find ("Chef Tony");
		LMFB = GameObject.Find ("LMFB");
		James = GameObject.Find ("James");
		Gasplosion = GameObject.Find ("Gasplosion");
		HatGlow = GameObject.Find ("Hat_Glow");
		ScreenFlash = GameObject.Find ("ScreenFlash");
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
		if(CurrentScene == 27) {
			if(timerIsGreaterThan(2.0f)) {
				playSound(GasBlastSound2);
				nextScene();
			}
		} else if(CurrentScene == 28) {
			if(timerIsGreaterThan(4.0f)) {
				nextScene();
			}
		} else if(CurrentScene == 30 || 
		          CurrentScene == 32 ||
		          CurrentScene == 34 ||
		          CurrentScene == 36) {
			if(timerIsGreaterThan(0.1f)) {
				ScreenFlash.renderer.enabled = false;
				nextScene();
			}
			
		} else if(CurrentScene == 37) {
			if(timerIsGreaterThan(2.0f)) {
				GameObject sparks = Instantiate(SparksPrefab) as GameObject;
				sparks.transform.position = LMFB.transform.position;
				playSound(GasBlastSound2);
				StartCoroutine(FadeAndNext(Color.green, 4, null));
				nextScene();
			}
		}
	}

	public override void HandleSceneChange() {
		if(CurrentScene == 5 || 
		   CurrentScene == 7 || 
		   CurrentScene == 10 || 
		   CurrentScene == 12 ||
		   CurrentScene == 15 || 
		   CurrentScene == 17 || 
		   CurrentScene == 21) {
			bc.ResumeBattle();
		} else if(CurrentScene == 22) {
			mus.StopMusic(1);
			HatGlow.particleSystem.gravityModifier = -0.02f;
		} else if(CurrentScene == 27) {
			Gasplosion.transform.position = James.transform.position;
			Gasplosion.particleSystem.Play();
			Gasplosion.particleSystem.loop = true;
			playSound(GasBlastSound1);
			Camera.main.GetComponent<CameraShake>().enabled = true;
			
			startTimer();
		} else if (CurrentScene == 30 || 
		           CurrentScene == 32 ||
		           CurrentScene == 34 ||
		           CurrentScene == 36) {
			ScreenFlash.renderer.enabled = true;
			startTimer();
		} else if (CurrentScene == 37) {
			startTimer();
		}
	}

	public void HandleBattleEvent(BattleEvent type) {
		switch(type) {
		case BattleEvent.TurnChange:

			// don't count the FB's turn
			if(bc.currentTurn != 2) {
				if(CurrentScene == 1 ||
				   CurrentScene == 5 || 
				   CurrentScene == 7 || 
				   CurrentScene == 10 || 
				   CurrentScene == 12 ||
				   CurrentScene == 15 || 
				   CurrentScene == 17 || 
				   CurrentScene == 21) {
					bc.PauseBattle();
					nextScene();
				}
			}
			
			break;
		case BattleEvent.Finished:
			mus.StopMusic(1.0f);
			nextScene();
			break;
		}
	}

}
