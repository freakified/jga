using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S601Puppeteer : CutscenePuppeteer {

	public AudioClip RainSound, JamesIntro;

	private GameObject ChefTony, James;
	private MusicPlayer mus;

	private Animator ctanim;

	// Use this for initialization
	void Start () {
		// get all the objects we'll need for the cutscene 
		ChefTony = GameObject.Find ("Chef Tony");
		James = GameObject.Find ("James");
		ctanim = ChefTony.GetComponent<Animator>();
		mus = GameObject.Find ("BGM").GetComponent<MusicPlayer>();


		playSound(RainSound, true);
		ctanim.SetInteger("HP", 0);
		startTimer();

	}
	
	// Update is called once per frame
	public void FixedUpdate () {
		if (CurrentScene == 0) {
			if(timerIsGreaterThan(10.0f)) {
				//start the cutscene
				nextScene();
			}
		} else if(CurrentScene == 8) {
			if(ChefTony.transform.position.x > 1.5f) {
				ChefTony.GetComponent<PlayerFreeze>().Freeze();
				mus.PlayMusic(JamesIntro, true);
				James.GetComponent<ConstantVelocity>().velocity.x = 6;
				nextScene();
			}
		} else if (CurrentScene == 9) {
			if(James.transform.position.x > 5) {
				James.transform.position = new Vector2(James.transform.position.x, James.transform.position.y - 1.5f);
				James.GetComponent<ConstantVelocity>().velocity.x = -6;
				nextScene();
			}
		} else if (CurrentScene == 10) {
			if(James.transform.position.x < ChefTony.transform.position.x) {
				ChefTony.transform.localScale = new Vector3(-1, 1, 1);
			}

			if(James.transform.position.x < 1) {
				James.GetComponent<ConstantVelocity>().enabled = false;
				startTimer();
				nextScene();
			}
		} else if (CurrentScene == 11) {
			James.rigidbody2D.AddForce((new Vector3(0, .3f, 0) - James.transform.position) * Time.fixedDeltaTime * 50);

			if(timerIsGreaterThan(1.0f) && James.transform.position.x > 0.3f ) {
				nextScene();
			}
		} else if (CurrentScene == 12) {
			if(James.rigidbody2D.velocity.magnitude == 0) {
				nextScene();
			}
		} else if(CurrentScene == 22) {
			James.rigidbody2D.AddForce(new Vector2(2, 300) * Time.fixedDeltaTime);

			if(James.transform.position.y > 0.2f) {
				nextScene();
			}
		} else if(CurrentScene == 23) {
			James.rigidbody2D.AddForce(Vector2.right * Time.fixedDeltaTime * 400);

			if(James.transform.position.x > ChefTony.transform.position.x) {
				ChefTony.transform.localScale = new Vector3(1, 1, 1);
			}


			if(James.transform.position.x > 5) {
				nextScene();
				James.GetComponent<JamesGasBehavior>().DisableFlightMode();
				James.rigidbody2D.Sleep();
				mus.StopMusic();
			}
		}
	}

	public override void HandleSceneChange() {

		if (CurrentScene == 7) {
			// wake up, mr. tony. wake up and smell...the ashes
			ctanim.SetInteger("HP", 100);
		} else if (CurrentScene == 8) {
			ChefTony.GetComponent<PlayerFreeze>().UnFreeze();
		} else if (CurrentScene == 12) {
			// restore james to normalcy
			James.GetComponent<JamesGasBehavior>().DisableFlightMode();
		} else if (CurrentScene == 22) {
			James.GetComponent<JamesGasBehavior>().EnableFlightMode();
			James.rigidbody2D.gravityScale = 0.2f;
		} else if (CurrentScene == 24) {
			ChefTony.GetComponent<PlayerFreeze>().UnFreeze();
		}
	}

}
