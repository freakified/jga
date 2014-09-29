using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S205Puppeteer : CutscenePuppeteer {

	private PlayerCartControl pcc;
		
	// Use this for initialization
	void Start () {
		pcc = GameObject.Find("Chef Tony Cart").GetComponent<PlayerCartControl>();

		startTimer();

	}
	
	// Update is called once per frame
	public void FixedUpdate () {
		if(CurrentScene == 0) {
			if(timerIsGreaterThan(1.5f)) {
				pcc.FreezeCart();
				GameObject.Find("TextBox(Clone)").transform.localPosition = new Vector3(-0.86f, 1.17f, 5.34f);

				nextScene();
			}
		}
	}

	public override void HandleSceneChange() {
		if(CurrentScene == 4) {
			pcc.UnFreezeCart();
		}
	}

}
