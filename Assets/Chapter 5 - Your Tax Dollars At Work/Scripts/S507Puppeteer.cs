using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S507Puppeteer : CutscenePuppeteer {

	public AudioClip ButtonPressSound, AlarmSound, DoorOpenSound, DoorCloseSound, SawingSound, ExplosionSound1, ExplosionSound2;
	public AudioClip knife;
	public AudioClip DramaticMusic;

	private MusicPlayer mp;
	private GameObject ChefTony, LikeMike, Scientist, DisclaimerText, BG;
	private GameObject Clouds, JamesDisplay;
	private GameObject[] gasMasks;
	private Animator ctanim;

	private ParticleSystem Gas;

	private GameObject JamesFly;
		
	// Use this for initialization
	void Start () {
		mp = GameObject.Find("BGM").GetComponent<MusicPlayer>();
		// get all the objects we'll need for the cutscene 
		ChefTony = GameObject.Find ("Chef Tony");
		LikeMike = GameObject.Find ("Like Mike");
		Scientist = GameObject.Find ("Scientist");
		DisclaimerText = GameObject.Find ("DisclaimerText");
		BG = GameObject.Find ("02 - BG");

		Gas = GameObject.Find ("Gasplosion").particleSystem;

		JamesFly = GameObject.Find ("james_fly");

		Clouds = GameObject.Find ("Clouds");
		JamesDisplay = GameObject.Find ("James-Display");

		gasMasks = GameObject.FindGameObjectsWithTag("Droppable");
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
			ChefTony.GetComponent<BoxCollider2D>().isTrigger = true;
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
				nextScene();
			}
		} else if (CurrentScene == 23) {
			if(timerIsGreaterThan(1.3f)) {
				Scientist.GetComponent<ConstantVelocity>().velocity.x = 0;
				Scientist.GetComponent<Animator>().SetBool("IsWalking", false);
				nextScene();
			}
		} else if (CurrentScene == 31) {
			if(timerIsGreaterThan(1.3f)) {
				Scientist.renderer.enabled = false;
				Scientist.GetComponent<SpriteShadow>().HideShadow = true;
				playSound(DoorCloseSound);
				nextScene();
			}
		} else if (CurrentScene == 35) {
			if(ChefTony.transform.position.x > 1.51f) {
				ChefTony.GetComponent<ConstantVelocity>().velocity.x = 0;
				ctanim.SetFloat("Speed", 0.0f);
				nextScene();
			}
		} else if (CurrentScene == 40) {
			if(timerIsGreaterThan(1.0f)) {
				nextScene();
			}
		} else if (CurrentScene == 44) {
			if(timerIsGreaterThan(0.8f)) {
				nextScene();
			}
		} else if (CurrentScene == 46) {
			if(gasMasks[2].transform.position.x > 1.4f) {
				gasMasks[2].rigidbody2D.rotation = 0.0f;
				gasMasks[2].rigidbody2D.isKinematic = true;
				gasMasks[2].transform.position = new Vector2(1.6566f, -0.56615f);
				ctanim.SetBool("IsSawing", false);
				stopSound();
				nextScene();
			}
		} else if (CurrentScene == 49) {
			if(timerIsGreaterThan(2.0f)) {
				startTimer();
				nextScene();
			}
		} else if (CurrentScene == 50) {
			if(timerIsGreaterThan(2.0f)) {
				nextScene();
			}
		} else if (CurrentScene == 51) {
			if(timerIsGreaterThan(2.0f)) {
				FadeAndNext(Color.green, 5.0f, "5-08 Limbo", false);
				nextScene();
			}
		}

		if(CurrentScene >= 51) {
			JamesFly.renderer.enabled = true;
			JamesFly.transform.Rotate(0, 0, 5.0f);
			JamesFly.transform.localScale = JamesFly.transform.localScale * 1.03f;
		}
	}

	public override void HandleSceneChange() {
		// once the text is ready, start the battle
		if(CurrentScene == 11) {
			LikeMike.rigidbody2D.AddForce(Vector2.one * 200.0f);
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
			foreach(GameObject gasMask in gasMasks) {
				gasMask.rigidbody2D.isKinematic = false;
			}
			startTimer();
		} else if(CurrentScene == 23) {
			Scientist.renderer.enabled = true;
			Scientist.GetComponent<SpriteShadow>().HideShadow = false;
			playSound(DoorOpenSound);
			Scientist.GetComponent<ConstantVelocity>().velocity.x = -2.0f;
			Scientist.GetComponent<Animator>().SetBool("IsWalking", true);
			startTimer();
		} else if(CurrentScene == 30) {
			gasMasks[0].renderer.enabled = false;
		} else if (CurrentScene == 31) {
			gasMasks[1].renderer.enabled = false;
			Scientist.transform.localScale = Vector3.one;
			Scientist.GetComponent<ConstantVelocity>().velocity.x = 2.0f;
			Scientist.GetComponent<Animator>().SetBool("IsWalking", true);
			startTimer();
		} else if (CurrentScene == 32) {
			mp.PlayMusic(DramaticMusic, true);
		} else if (CurrentScene == 35) {
			ChefTony.GetComponent<ConstantVelocity>().enabled = true;
			ChefTony.GetComponent<ConstantVelocity>().velocity.x = 2.0f;
			ctanim.SetFloat("Speed", 1.0f);
		} else if (CurrentScene == 38) {
			DisclaimerText.guiText.enabled = true;
		} else if (CurrentScene == 39) {
			DisclaimerText.guiText.enabled = false;
		} else if (CurrentScene == 40) {
			ctanim.SetBool("IsSawing", true);
			playSound(SawingSound, true);
			startTimer();
		} else if (CurrentScene == 44) {
			LikeMike.rigidbody2D.AddForce(Vector2.one * 120.0f);
			startTimer();
		} else if (CurrentScene == 46) {
			gasMasks[2].rigidbody2D.AddForce(Vector2.one * 200.0f);
		} else if (CurrentScene == 47) {

		} else if (CurrentScene == 49) {
			Camera.main.GetComponent<CameraShake>().enabled = true;
			playSound(ExplosionSound1);
			Clouds.renderer.enabled = false;
			JamesDisplay.renderer.enabled = false;
			Gas.Play();
			startTimer();
		} else if(CurrentScene == 50 || CurrentScene == 51) {
			playSound(ExplosionSound2);
		}
	}

}
