using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S204Puppeteer : CutscenePuppeteer {

	public Sprite foodCartFull;

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
		} else if(CurrentScene == 7) {
			if(police.transform.position.x > -2.9f) {
				police.GetComponent<Animator>().SetFloat("Speed", 0f);
				police.rigidbody2D.velocity = Vector2.zero;
				nextScene();
			}
		} else if(CurrentScene == 10) {
			if(chefTony.transform.position.x > 0.53) {
				foodCart.GetComponent<SpriteRenderer>().sprite = foodCartFull;
				chefTony.renderer.enabled = false;

				police.GetComponent<Animator>().SetFloat("Speed", 3f);
				police.rigidbody2D.velocity = new Vector2(3f, 0f);

				flipObject(salesman);
				nextScene();
			}
		} else if(CurrentScene == 11) {
			Vector3 temp = foodCart.transform.position;
			temp.x = chefTony.transform.position.x;
			foodCart.transform.position = temp;
		}
	}

	public override void HandleSceneChange() {

		if(CurrentScene == 3) {
			chefTony.GetComponent<PlayerFreeze>().UnFreeze();
		} else if(CurrentScene == 7) {
			//make CT look backward
			flipObject(chefTony);

			//make the guard face CT
			flipObject(police);

			//jump the guard to just off the screen so he can waltz right in
			Vector3 pos = police.transform.position;
			pos.x = -6;
			police.transform.position = pos;
			police.GetComponent<Animator>().SetFloat("Speed", 3f);
			police.rigidbody2D.velocity = new Vector2(3f, 0f);

		} else if(CurrentScene == 10) {
			// chef tony should run off the screen with the cart
			flipObject(chefTony);

			chefTony.rigidbody2D.isKinematic = true;
			chefTony.rigidbody2D.velocity = new Vector2(3f, 0f);
			ctanim.SetFloat("Speed", 3f);
		} else if(CurrentScene == 12) {
			StartCoroutine(FadeAndNext(Color.black, 2, null));
		}
	}

}
