using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S702Puppeteer : CutscenePuppeteer {

	private Stack<OrphanCombatant> orphans;
		
	// Use this for initialization
	void Start () {
		orphans = new Stack<OrphanCombatant>(GameObject.Find("Orphans").GetComponentsInChildren<OrphanCombatant>());

		startTimer();
	}
	
	// Update is called once per frame
	public void FixedUpdate () {
		if(CurrentScene == 0) {
			if(timerIsGreaterThan(0.02f)) {
				orphans.Pop().IsJumping = true;
				startTimer();
				if(orphans.Count == 0) {
					nextScene();
				}
			}
		} else if(CurrentScene == 1) {
			if(Camera.main.transform.position.x > 0.40f) {
				FadeAndNext(Color.white, 5, "7-03 Fate of Chef Tony", false);
				nextScene();
			}
		} else if(CurrentScene == 2) {
			if(Camera.main.transform.position.x > 0.55f) {
				Camera.main.GetComponent<ConstantVelocity>().velocity = Vector2.zero;
			}
		}
		
	}

	public override void HandleSceneChange() {

	}

}
