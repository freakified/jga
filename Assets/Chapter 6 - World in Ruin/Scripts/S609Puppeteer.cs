using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S609Puppeteer : CutscenePuppeteer {

	public AudioClip FlashSound, RumbleSound, ExplosionSound;

	//private GameObject FlyingBBall;
	private ParticleSystem SparkParticles;
	private LensFlare ExplosionFlare;
	private MusicPlayer mus;
	private BattleController bc;
	private ScreenFlasher ScreenFlash;


	// Use this for initialization
	void Start () {
		// get all the objects we'll need for the cutscene 
		//FlyingBBall = GameObject.Find ("Flying Basketball");
		ScreenFlash = GameObject.Find ("ScreenFlash").GetComponent<ScreenFlasher>();
		ExplosionFlare = GameObject.Find ("Explosion_Flare").GetComponent<LensFlare>();
		SparkParticles = GameObject.Find ("SparkParticles").GetComponent<ParticleSystem>();

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
		if(CurrentScene == 3) {
			if(timerIsGreaterThan(1f)) {
				nextScene();
			}
		} else if(CurrentScene == 4) {
			if(timerIsGreaterThan(0.5f)) {
				nextScene();
			}
		} else if(CurrentScene == 5) {
			if(timerIsGreaterThan(0.5f)) {
				playSound(RumbleSound);
				Camera.main.GetComponent<CameraShake>().enabled = true;
				nextScene();
			}
		} else if(CurrentScene == 6) {
			if(timerIsGreaterThan(2.0f)) {
				playSound(ExplosionSound);
				ScreenFlash.FlashScreen();
				nextScene();
			}
		} else if(CurrentScene == 7) {
			if(timerIsGreaterThan(4.0f)) {
				playSound(ExplosionSound);
				ScreenFlash.FlashScreen();
				nextScene();
			}
		} else if(CurrentScene == 8) {
			ExplosionFlare.brightness += 1.0f * Time.fixedDeltaTime;
			
			if(ExplosionFlare.brightness > 5f) {
				FadeAndNext(Color.white, 1f, "6-09a Limbo", false);
				nextScene();

			}
		}
	}

	public override void HandleSceneChange() {
		if(CurrentScene == 2) {
			bc.ResumeBattle();
		} else if(CurrentScene == 3 || CurrentScene == 4 || CurrentScene == 5) {
			ScreenFlash.FlashScreen();
			playSound(FlashSound);
			startTimer();
		} else if(CurrentScene == 6) {
			startTimer();
			playSound(ExplosionSound);
			ScreenFlash.FlashScreen();
		} else if(CurrentScene == 7) {
			SparkParticles.Play();
			startTimer();
		} else if(CurrentScene == 8) {
			ExplosionFlare.enabled = true;
			ScreenFlash.FlashScreen();
		}
	}

	public void HandleBattleEvent(BattleEvent type) {
		switch(type) {
		case BattleEvent.TurnChange:
			if(CurrentScene == 0) {
				bc.PauseBattle();
				nextScene();
			} else if(CurrentScene == 2) {
				// did the bball die?
				if(bc.EnemyCombatants[0].HitPoints == 0) {
					bc.PauseBattle();
					mus.StopMusic(3.0f);
					startTimer();
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
