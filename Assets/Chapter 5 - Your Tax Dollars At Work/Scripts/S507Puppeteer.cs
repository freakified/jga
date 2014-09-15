using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S507Puppeteer : CutscenePuppeteer {

	public AudioClip ButtonPressSound, AlarmSound;
	public AudioClip knife;

	public GameObject SparksPrefab;

	private GameObject ChefTony, LikeMike, BG;
	private GameObject sparks;
	private Animator ctanim;
		
	// Use this for initialization
	void Start () {
		// get all the objects we'll need for the cutscene 
		ChefTony = GameObject.Find ("Chef Tony");
		LikeMike = GameObject.Find ("Like Mike");
		BG = GameObject.Find ("02 - BG");
	}
	
	// Update is called once per frame
	public void FixedUpdate () {

		if(CurrentScene == 0) {
			if(ChefTony.transform.position.x > -1.79) {
				// next: Chef Tony needs disable/enable control methods
				ChefTony.GetComponent<PlayerFreeze>().Freeze();

				ctanim = ChefTony.GetComponent<Animator>();

				//start the cutscene
				nextScene();
			}
		} else if(CurrentScene == 4) {
			ctanim.SetFloat("Speed", 1);
			ChefTony.rigidbody2D.gravityScale = 0;
			ChefTony.rigidbody2D.velocity = new Vector2(2f, 0.3f);
			ChefTony.GetComponent<SpriteShadow>().LockShadowY();
			nextScene();
		} else if(CurrentScene == 5) {
			if(ChefTony.transform.position.x > -0.3) {
				ctanim.SetFloat("Speed", 0);
				ChefTony.rigidbody2D.velocity = Vector2.zero;
				startTimer();
				nextScene();
			}
		} else if(CurrentScene == 6) {
			if(timerIsGreaterThan(0.2f)) {
				stopTimer();
				playSound(knife);
				ctanim.SetBool("IsAttacking", true);
				startTimer();
				nextScene();
			}
		} else if(CurrentScene == 7) {
			if(timerIsGreaterThan(0.5f)) {
				nextScene();
			}
		} else if(CurrentScene == 11) {
			if(timerIsGreaterThan(1.0f)) {
				nextScene();
			}
		} else if (CurrentScene == 15) {
			if(timerIsGreaterThan(0.2f)) {
				ctanim.SetBool("IsAttacking", false);
				nextScene();
			}
		} else if (CurrentScene == 16) {
			if(timerIsGreaterThan(2f)) {
				playSound(AlarmSound);
				BG.GetComponent<SpriteRenderer>().color = new Color(.70f, .21f, .21f);
				nextScene();
			}
		} else if (CurrentScene == 21) {
			if(timerIsGreaterThan(1.0f)) {
				LikeMike.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
				nextScene();
			}
		}


	}

	public override void HandleSceneChange() {
		// once the text is ready, start the battle
		if(CurrentScene == 11) {
			LikeMike.rigidbody2D.AddForce(new Vector2(200.0f, 200.0f));
			startTimer();
		} else if (CurrentScene == 10) {
			ctanim.SetBool("IsAttacking", false);
		} else if (CurrentScene == 15) {
			playSound(ButtonPressSound);
			ctanim.SetBool("IsAttacking", true);
			startTimer();
		} else if (CurrentScene == 16) {
			startTimer();
		} else if (CurrentScene == 21) {
			GameObject[] gasMasks = GameObject.FindGameObjectsWithTag("Droppable");
			foreach(GameObject gasMask in gasMasks) {
				gasMask.rigidbody2D.isKinematic = false;
			}
			startTimer();
		}
	}

}
