using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S608Puppeteer : CutscenePuppeteer {

	public AudioClip FlashbackSound, EvilMusic, FinalBossMusic;

	public List<Sprite> FlashbackBGList;
	private Queue<Sprite> flashbackBGs;

	private GameObject ChefTony, James, LikeMike, FlyingBBall, Flashback;
	private ScreenFlasher ScreenFlash;
	private MusicPlayer mus;
	private BattleController bc;


	// Use this for initialization
	void Start () {
		// get all the objects we'll need for the cutscene 
		ChefTony = GameObject.Find ("Chef Tony");
		James = GameObject.Find ("James");
		LikeMike = GameObject.Find ("Like Mike");
		FlyingBBall = GameObject.Find ("Flying Basketball");
		Flashback = GameObject.Find ("Flashback");
		ScreenFlash = GameObject.Find ("ScreenFlash").GetComponent<ScreenFlasher>();

		mus = GameObject.Find ("BGM").GetComponent<MusicPlayer>();
		bc = GetComponent<BattleController>();

		flashbackBGs = new Queue<Sprite>(FlashbackBGList);

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
		if(CurrentScene == 9 ||
		   CurrentScene == 18 ||
		   CurrentScene == 22 ||
		   CurrentScene == 26) {
			if(timerIsGreaterThan(1f)) {
				nextScene();
			}
		}
	}

	public override void HandleSceneChange() {
		if(CurrentScene == 9 ||
		   CurrentScene == 18 ||
		   CurrentScene == 22 ||
		   CurrentScene == 26) {
			ScreenFlash.FlashScreen();
			Flashback.renderer.enabled = true;
			Flashback.GetComponent<SpriteRenderer>().sprite = flashbackBGs.Dequeue();
			playSound(FlashbackSound);
			startTimer();
		}

		if(CurrentScene == 9) {
			mus.PlayMusic(EvilMusic, true);
		}

		if(CurrentScene == 69) {
			mus.PlayMusic(FinalBossMusic, true);
			StartCoroutine(FadeAndNext(Color.white, 2, "6-09 The Final Battle"));
		}
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
