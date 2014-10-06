using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S703Puppeteer : CutscenePuppeteer {

		
	// Use this for initialization
	void Start () {
		//GameObject.Find("Chef Tony").GetComponent<Animator>().SetBool("IsJumping", true);
	}
	
	// Update is called once per frame
	public void FixedUpdate () {
		if(CurrentScene == 0) {
			if(Camera.main.transform.position.y < -0.3f) {
				FadeAndNext(Color.white, 5, "7-04 Title Card", false);
				nextScene();
			}
		} else if(CurrentScene == 1) {
			if(Camera.main.transform.position.y < -0.49f) {
				Camera.main.GetComponent<ConstantVelocity>().velocity = Vector2.zero;
				nextScene();
			}
		}
		
	}

	public override void HandleSceneChange() {

	}

}
