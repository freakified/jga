using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S502Puppeteer : CutscenePuppeteer {

	public EdgeCollider2D LeftBound;
	public AudioClip DoorSound, SmashSound;

	private GameObject ChefTony, LikeMike, Guard;

	//private Animator ctanim;

	// Use this for initialization
	void Start () {
		// get all the objects we'll need for the cutscene 
		ChefTony = GameObject.Find ("Chef Tony");
		LikeMike = GameObject.Find ("Like Mike");
		Guard = GameObject.Find ("Guard");


		//ctanim = ChefTony.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	public void FixedUpdate () {
		if (CurrentScene == 2) {
			if(timerIsGreaterThan(1.0f)) {
				LeftBound.isTrigger = false;

				nextScene();
			}
		} else if(CurrentScene == 4) {
			if(Guard.transform.position.x > 2.40f) {
				playSound(DoorSound);
				Guard.renderer.enabled = false;
				Guard.GetComponent<SpriteShadow>().HideShadow = true;
				nextScene();
			}
		} else if(CurrentScene == 10) {
			if(ChefTony.transform.position.x > 2.40f) {
				playSound(SmashSound);
				ChefTony.renderer.enabled = false;
				ChefTony.GetComponent<SpriteShadow>().HideShadow = true;
				nextScene();
			}
		}
	}

	public override void HandleSceneChange() {
		if (CurrentScene == 2) {
			//disable normal colliding to let CT et al through
			LeftBound.isTrigger = true;

			ChefTony.rigidbody2D.AddForce(new Vector2(200, 100));
			LikeMike.rigidbody2D.AddForce(new Vector2(130, 200));

			startTimer();
			//ChefTony.GetComponent<PlayerFreeze>().UnFreeze();
		} else if (CurrentScene == 4) {
			Guard.transform.localScale = Vector3.one;
			Guard.GetComponent<Animator>().SetFloat("Speed", 5.0f);
			Guard.GetComponent<ConstantVelocity>().enabled = true;
			Guard.GetComponent<ConstantVelocity>().velocity = new Vector2(5.0f, 0.0f);
		} else if (CurrentScene == 10) {
			ChefTony.GetComponent<Animator>().SetFloat("Speed", 5.0f);
			ChefTony.GetComponent<ConstantVelocity>().enabled = true;
			ChefTony.GetComponent<ConstantVelocity>().velocity = new Vector2(5.0f, 0.0f);
		} else if (CurrentScene == 12) {
			FadeAndNext(Color.black, 3.0f, "5-03 Inside Facility 1", true);
		}
	}

}
