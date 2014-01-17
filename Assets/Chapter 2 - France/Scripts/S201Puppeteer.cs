using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S201Puppeteer : CutscenePuppeteer {

		
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	public void FixedUpdate () {

		if(CurrentScene == 3) {
			StartCoroutine(FadeAndNext(Color.white, 0, "2-02 France 1"));
			nextScene();
		}
		
	}

	public override void HandleSceneChange() {

	}

}
