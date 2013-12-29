using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S2Puppeteer : CutscenePuppeteer {

	public AudioClip elevNoise;

	private GameObject ChefTony;
	private bool elevTriggered = false;
		
	// Use this for initialization
	void Start () {
		// get all the objects we'll need for the cutscene 
		ChefTony = GameObject.Find ("Chef Tony");
	}

	public override void OnEnable() {
		base.OnEnable();

		BattleController.OnBattleEvent += HandleBattleEvent;
	}
	
	
	public override void OnDisable() {
		base.OnEnable();

		BattleController.OnBattleEvent -= HandleBattleEvent;
	}

	
	// Update is called once per frame
	public override void Update () {
		base.Update();

		if(CurrentScene == 0) {
			if(ChefTony.transform.position.x > -1.79) {
				// next: Chef Tony needs disable/enable control methods
				//ChefTony.GetComponent<PlayerFreeze>().Freeze();

				//start the cutscene
				//nextScene();
			}
		} else if(CurrentScene == 7) {

		}

		if(ChefTony.transform.position.x > 2.4 && !elevTriggered) {
			// actions that must run before the elevator activates
			ChefTony.GetComponent<PlayerFreeze>().Freeze();

			playSound(elevNoise);
			ChefTony.renderer.enabled = false;

			//fade out
			ChefTony.transform.position = new Vector2(ChefTony.transform.position.x, -1.2f);
			
			StartCoroutine(FadeAndNext(Color.black, 2.5f, "03-Elevator-Entry 3"));

			elevTriggered = true;
		}

		if(elevTriggered) {
			//FLY, CHEF
			ChefTony.rigidbody2D.AddForce(Vector2.up * 15);
		}

	}

	public override void HandleSceneChange() {
		// once the text is ready, start the battle
		if(CurrentScene == 4)
			GetComponent<BattleController>().StartBattle();
		else if(CurrentScene == 7)  
			ChefTony.GetComponent<PlayerFreeze>().UnFreeze();
	}

	public void HandleBattleEvent(BattleEvent type) {
		if(type == BattleEvent.Finished) {
			nextScene();
		}
	}
}
