using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S301Puppeteer : CutscenePuppeteer {

	private GameObject chefTony;
	private GameObject ff, os, shoes;
	private MusicPlayer mp;

	private BattleController bc;
		
	// Use this for initialization
	void Start () {
		mp = GameObject.Find("BGM").GetComponent<MusicPlayer>();
		bc = GameObject.Find ("Scripts").GetComponent<BattleController>();

		chefTony = GameObject.Find ("Chef Tony");
		ff = GameObject.Find ("Father Flanagan");
		os = GameObject.Find ("Orphan Shield");
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
		}

	}

	public override void HandleSceneChange() {
		switch(CurrentScene) {
		case 20:
			mp.PlayMusic();
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
		}
	}
	
	public void HandleBattleEvent(BattleEvent type) {
		switch(type) {
		case BattleEvent.TurnChange:
			if(CurrentScene == 28) {
				// when FF's HP starts getting low...
				if(bc.EnemyCombatants[1].HitPoints / (float)bc.EnemyCombatants[1].MaxHitPoints < 0.2f) {
					bc.PauseBattle();
					nextScene();
				}
			} else if(CurrentScene == 31) {
				//did FF die?
				if(bc.EnemyCombatants[1].HitPoints == 0) {
					bc.PauseBattle();
					nextScene();
				}
			}

			break;
		}
	}
}
