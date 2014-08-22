using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S501Puppeteer : CutscenePuppeteer {

	private GameObject ChefTony;
	private MusicPlayer mus;

	//private Animator ctanim;

	// Use this for initialization
	void Start () {
		// get all the objects we'll need for the cutscene 
		ChefTony = GameObject.Find ("Chef Tony");
		mus = GameObject.Find ("BGM").GetComponent<MusicPlayer>();
		//ctanim = ChefTony.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	public void FixedUpdate () {
		if (CurrentScene == 4) {
			if (ChefTony.transform.position.x > 1.2f) {
				ChefTony.GetComponent<PlayerFreeze>().Freeze();
				StartCoroutine(FadeAndNext(Color.black, 2.0f, "5-02 Outside Facility 2"));
				nextScene();
			}
		}
	}

	public override void HandleSceneChange() {
		if (CurrentScene == 4) {
			ChefTony.GetComponent<PlayerFreeze>().UnFreeze();
		}
	}

}
