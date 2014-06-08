using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S402Puppeteer : CutscenePuppeteer {

	private GameObject ChefTony;
	private GameObject ExamineInfo;
	private GUIText ExamineNextText;
	//private MusicPlayer mus;

	//private Animator ctanim;

	// Use this for initialization
	void Start () {
		// get all the objects we'll need for the cutscene 
		ChefTony = GameObject.Find ("Chef Tony");
		ExamineInfo = GameObject.Find ("ExamineInfo");
		ExamineNextText = GameObject.Find ("ExamineNextText").GetComponent<GUIText>();
		ExamineNextText.enabled = false;

		//mus = GameObject.Find ("BGM").GetComponent<MusicPlayer>();
		//ctanim = ChefTony.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	public void FixedUpdate () {
		if(CurrentScene == 3) {
			if(ChefTony.transform.position.x > 0.537f) {
				ChefTony.GetComponent<PlayerFreeze>().Freeze();
				nextScene();
			}
		} else if(CurrentScene == 5) {
			if(!ExamineNextText.enabled && timerIsGreaterThan(1)) {
				ExamineNextText.enabled = true;
				stopTimer();
			} else if(Input.GetButtonDown("Select")) {
				GameObject.Destroy(ExamineInfo);
				nextScene();
			}

		} 
	}

	public override void HandleSceneChange() {
		if(CurrentScene == 3) {
			ChefTony.GetComponent<PlayerFreeze>().UnFreeze();
		} else if(CurrentScene == 5) {
			//show the coffin examination thing
			startTimer();

			ExamineInfo.transform.position = Vector3.zero;
		}
	}

}
