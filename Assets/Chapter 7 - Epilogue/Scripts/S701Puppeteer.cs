using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S701Puppeteer : CutscenePuppeteer {

		
	// Use this for initialization
	void Start () {
		startTimer();

		Invoke ("startCameraMotion", 4f);
	}


	private void startCameraMotion() {
		Camera.main.GetComponent<ConstantVelocity>().velocity = Vector2.up * -0.2f;
	}


	private bool jamesHasStarted = false;
	
	// Update is called once per frame
	public void FixedUpdate () {
		if(CurrentScene == 0) {
			if(!jamesHasStarted && Camera.main.transform.position.y < -2.5f) {
				GameObject.Find("James").GetComponent<ConstantVelocity>().velocity.x = 3.0f;

				jamesHasStarted = true;
			}

			if(Camera.main.transform.position.y < -3.2f) {
				FadeAndNext(Color.white, 5, "7-02 Fate of Like Mike", false);
				nextScene();
			}
		} else if(CurrentScene == 1) {
			if(Camera.main.transform.position.y < -3.6f) {
				Camera.main.GetComponent<ConstantVelocity>().velocity = Vector2.zero;
				nextScene();
			}
		}
		
	}

	public override void HandleSceneChange() {

	}

}
