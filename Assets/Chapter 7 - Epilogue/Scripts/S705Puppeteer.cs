using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S705Puppeteer : CutscenePuppeteer {

	public float CreditsSpeed = 1.0f;

	private GameObject credits;
	private MusicPlayer mus;
		
	// Use this for initialization
	void Start () {
		credits = GameObject.Find("Credits");
		mus = GameObject.Find("BGM").GetComponent<MusicPlayer>();
	}
	
	// Update is called once per frame
	public void FixedUpdate () {
		if(CurrentScene == 1) {
			Vector3 pos = credits.transform.position;
			pos.y += CreditsSpeed * Time.fixedDeltaTime;
			credits.transform.position = pos;

			if(pos.y > 9.9f) {
				nextScene();
			}
		} else if(CurrentScene == 2) {
			if(!mus.IsPlaying()) {
				FadeAndNext(Color.black, 5.0f, "0-01 Title Card 1", false);
				nextScene();
			}
		}
	}

	public override void HandleSceneChange() {

	}

}
