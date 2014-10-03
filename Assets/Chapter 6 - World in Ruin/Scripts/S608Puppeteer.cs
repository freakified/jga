using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S608Puppeteer : CutscenePuppeteer {

	public AudioClip FlashbackSound, EvilMusic, FinalBossMusic;

	public List<Sprite> FlashbackBGList;
	private Queue<Sprite> flashbackBGs;

	private GameObject Flashback, FlyingBBall, Background, Background2;
	private ScreenFlasher ScreenFlash;
	private MusicPlayer mus;

	private bool fadingStarted = false;


	// Use this for initialization
	void Start () {
		// get all the objects we'll need for the cutscene 
		Flashback = GameObject.Find ("Flashback");
		ScreenFlash = GameObject.Find ("ScreenFlash").GetComponent<ScreenFlasher>();
		FlyingBBall = GameObject.Find ("FlyingBasketball");
		Background = GameObject.Find ("Background-Flicker");
		Background2 = GameObject.Find ("Background");

		mus = GameObject.Find ("BGM").GetComponent<MusicPlayer>();

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
		} else if(CurrentScene == 69) {
			FlyingBBall.transform.localScale += Vector3.one * Time.fixedDeltaTime * 0.3f;
			Color temp = ((SpriteRenderer)Background.renderer).color;
			temp.a -= 0.3f * Time.fixedDeltaTime;
			((SpriteRenderer)Background.renderer).color = temp;

			if(FlyingBBall.transform.localScale.x > 2f && !fadingStarted) {
				StartCoroutine(FadeAndNext(Color.white, 1, "6-09 The Final Battle"));
				fadingStarted = true;
			}

		}
	}

	public override void HandleSceneChange() {
//		while(CurrentScene < 67) {
//			nextScene();
//		}

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
			Background.GetComponent<SpriteFlicker>().enabled = false;
			((SpriteRenderer)Background.renderer).color = Color.white;
			Background2.renderer.enabled = false;
			startTimer();
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
