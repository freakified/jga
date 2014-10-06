using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S3Puppeteer : CutscenePuppeteer {

	public AudioClip explosionNoise;
	public AudioClip knife;

	public GameObject SparksPrefab;

	private GameObject ChefTony;
	private GameObject sparks;
	private Animator ctanim;
		
	// Use this for initialization
	void Start () {
		// get all the objects we'll need for the cutscene 
		ChefTony = GameObject.Find ("Chef Tony");
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
		} else if(CurrentScene == 2) {
			ctanim.SetFloat("Speed", 1);
			ChefTony.rigidbody2D.gravityScale = 0;
			ChefTony.rigidbody2D.velocity = new Vector2(2f, 0.3f);
			ChefTony.GetComponent<SpriteShadow>().LockShadowY();
			nextScene();
		} else if(CurrentScene == 3) {
			if(ChefTony.transform.position.x > -0.3) {
				ctanim.SetFloat("Speed", 0);
				ChefTony.rigidbody2D.velocity = Vector2.zero;
				startTimer();
				nextScene();
			}
		} else if(CurrentScene == 4) {
			if(timerIsGreaterThan(0.2f)) {
				stopTimer();
				playSound(knife);
				ctanim.SetBool("IsAttacking", true);
				nextScene();
			}
		} else if(CurrentScene == 5) {
			sparks = Instantiate(SparksPrefab) as GameObject;
			sparks.transform.position = ChefTony.transform.position + new Vector3(.4f, 0f, 0f);
			playSound(explosionNoise);
			startTimer();
			nextScene();
		} else if(CurrentScene == 6) {
			if(timerIsGreaterThan(0.3f)) {
				stopTimer();
				FadeAndNext(Color.white, 5, "2-01 Limbo", true);
				nextScene();
			}
		}


	}

	public override void HandleSceneChange() {
		// once the text is ready, start the battle

	}

}
