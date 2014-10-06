using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S301Puppeteer : CutscenePuppeteer {

	private GameObject chefTony;
	private GameObject ff, os, shoes;
	private MusicPlayer mp;

	private BattleController bc;

	public GameObject BGBlackout;

	public AudioClip FlanagansTheme;
	public AudioClip BattleMusic;
	public AudioClip VictoryMusic;
	public AudioClip ShoesTheme;

	private bool fadingHasStarted = false;

	// Use this for initialization
	void Start () {
		mp = GameObject.Find("BGM").GetComponent<MusicPlayer>();
		bc = GameObject.Find ("Scripts").GetComponent<BattleController>();

		chefTony = GameObject.Find ("Chef Tony");
		ff = GameObject.Find ("Father Flanagan");
		os = GameObject.Find ("Orphan Shield");
		shoes = GameObject.Find ("ShoesTie");
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
		switch(CurrentScene) {
		case 0:
			if(chefTony.transform.position.x > 1.0f) {
				chefTony.GetComponent<PlayerFreeze>().Freeze();
				ff.GetComponent<Animator>().SetTrigger("FistPump");
				ff.rigidbody2D.AddForce(new Vector2(-400.0f, 0));
				mp.PlayMusic(FlanagansTheme, true);
				nextScene();
			}
			break;
		case 20:
			if(timerIsGreaterThan(1.2f)) {
				chefTony.rigidbody2D.AddForce(new Vector2(-500.0f, 100.0f));
				startTimer();
				nextScene();
			}
			break;
		case 21:
			if(timerIsGreaterThan(0.8f)) {
				nextScene();
			}
			break;
		case 32:
			if(bc.EnemyCombatants[1].HitPoints == 0 && timerIsGreaterThan(2.0f)) {
				nextScene();
			}
			break;
		case 38:
			if(timerIsGreaterThan(1.0f)) {
				nextScene();
			}
			break;
		case 40:
			// has CT reached the shoes?
			if(chefTony.transform.position.x > shoes.transform.position.x - 0.6f) {
				chefTony.GetComponent<ConstantVelocity>().enabled = false;
				chefTony.GetComponent<Animator>().SetFloat("Speed", 0);

				nextScene();
			}
			break;
		case 44:
			// are the shoes well into their explosion?
			if(timerIsGreaterThan(7.0f)) {
				//pull in poor chef tony
				chefTony.rigidbody2D.fixedAngle = false;
				chefTony.GetComponent<Animator>().SetFloat("Speed", 1);
				chefTony.GetComponent<SpriteShadow>().HideShadow = true;
				chefTony.rigidbody2D.gravityScale = 0;
				chefTony.rigidbody2D.drag = 2.0f;
				startTimer();
				nextScene();
			}
			break;
		case 45:
			// chef tony gets pulled in
			chefTony.rigidbody2D.AddForce(50.0f * (shoes.transform.position - chefTony.transform.position) * Time.fixedDeltaTime);
			chefTony.rigidbody2D.AddTorque(3.0f * Time.fixedDeltaTime);
			BGBlackout.renderer.material.color = 
				new Color(0, 0, 0, Mathf.Lerp(0.0f, 1.0f, elapsedTime / 3));

			if(elapsedTime / 3 > 1.0f) {
				startTimer();
				nextScene();
			}

			break;
		case 46:
			if(timerIsGreaterThan(1.5f)) {
				startTimer();
				nextScene();
			}
			break;
		case 47: 
			//clear the NOOOOOOOOOOO dialog after 2 seconds
			if(timerIsGreaterThan(1.0f)) {
				startTimer();
				nextScene();
			}
			break;
		case 48:
			Vector3 newCameraPos = Camera.main.transform.position;
			newCameraPos.x = Mathf.Lerp(0.0f, 1.87f, elapsedTime / 10);
			Camera.main.transform.position = newCameraPos;
			
			Camera.main.orthographicSize = Mathf.Lerp(1.8f, 0.5f, elapsedTime / 10);
			
			if(elapsedTime / 10 > 0.5f && !fadingHasStarted) {
				FadeAndNext(Color.white, 5, "4-01 Shrine Exterior", false);
				fadingHasStarted = true;
			} else if(elapsedTime / 10 > 0.9f && fadingHasStarted && !mp.fadingOut) {

				mp.StopMusic(1.0f);
			}

			break;
		}
	}

	public override void HandleSceneChange() {

		switch(CurrentScene) {
		case 20:
			//flanagan throws down the gauntlet
			mp.PlayMusic(BattleMusic, true);
			os.rigidbody2D.AddForce(new Vector2(-220.0f, 100.0f));
			ff.GetComponent<FlanaganCombatant>().AutoAttack(null);
			startTimer();
			break;
		case 23:
			os.GetComponent<OrphanCombatant>().IsJumping = true;
			break;
		case 28:
			GetComponent<BattleController>().StartBattle();
			break;
		case 31:
			bc.ResumeBattle();
			os.GetComponent<OrphanCombatant>().leaveBattle();
			break;
		case 38:
			//flanagan's illustrious exit
			ff.rigidbody2D.AddForce(new Vector2(320.0f, 200.0f));

			//oh and look the shoes fell down what a coincidence
			shoes.rigidbody2D.isKinematic = false;
			shoes.rigidbody2D.AddForce(-10 * Vector2.right);
			shoes.GetComponent<ShoeWind>().enabled = false;

			startTimer();

			break;
		case 40:
			//chef tony's shoe investigation
			chefTony.GetComponent<Animator>().SetFloat("Speed", 10);
			chefTony.GetComponent<ConstantVelocity>().enabled = true;
			chefTony.GetComponent<ConstantVelocity>().velocity =  Vector2.right * 3;
			break;
		case 44:
			shoes.GetComponent<Shoesplosion>().enabled = true;
			mp.PlayMusic(ShoesTheme, false);
			startTimer();
			break;
		}
	}
	
	public void HandleBattleEvent(BattleEvent type) {
		switch(type) {
		case BattleEvent.TurnChange:
			if(CurrentScene == 28) {
				// when FF's HP starts getting low...

				if(bc.EnemyCombatants[1].HitPoints / (float)bc.EnemyCombatants[1].MaxHitPoints < 0.2f) {
					bc.PauseBattle();
					ff.GetComponent<FlanaganCombatant>().WakeUp();
					nextScene();
				}
			} else if(CurrentScene == 31) {
				//did FF die?
				if(bc.EnemyCombatants[1].HitPoints == 0) {
					bc.PauseBattle();
					mp.PlayMusic(VictoryMusic, false);
					startTimer();
					nextScene();
				}
			}

			break;
		}
	}
}
