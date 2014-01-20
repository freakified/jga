using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S204Puppeteer : CutscenePuppeteer {
	
	private GameObject chefTony;
	private GameObject police;
	private GameObject salesman;
	private GameObject foodCart;

	private Animator ctanim;
		
	// Use this for initialization
	void Start () {
		// get all the objects we'll need for the cutscene 
		chefTony = GameObject.Find ("Chef Tony");
		ctanim = chefTony.GetComponent<Animator>();

		police = GameObject.Find ("Guard");
		salesman = GameObject.Find ("Salesman");
		foodCart = GameObject.Find ("Food Cart");

	}
	
	// Update is called once per frame
	public void FixedUpdate () {
		if(CurrentScene == 3) {
			if(chefTony.transform.position.x > -1.38f) {
				chefTony.GetComponent<PlayerFreeze>().Freeze();
				nextScene();
			}
		} else if(CurrentScene == 6) {
			if(police.transform.position.x > -1.89f) {
				police.GetComponent<Animator>().SetFloat("Speed", 0f);
				police.rigidbody2D.velocity = Vector2.zero;
				nextScene();
			}
		}
	}

	public override void HandleSceneChange() {

		if(CurrentScene == 3) {
			chefTony.GetComponent<PlayerFreeze>().UnFreeze();
		} else if(CurrentScene == 6) {
			//make CT look backward
			flipObject(chefTony);

			//make the guard face CT
			flipObject(police);
			police.GetComponent<Animator>().SetFloat("Speed", 2f);
			police.rigidbody2D.velocity = new Vector2(2f, 0f);

		}
	}

}
