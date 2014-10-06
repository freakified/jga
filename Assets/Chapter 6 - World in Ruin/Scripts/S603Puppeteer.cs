using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S603Puppeteer : CutscenePuppeteer {

	public AudioClip DariasTheme, ElecZap;
	public ParticleSystem ZapParticlePrefab;
	private ParticleSystem zapParticles;

	private GameObject ChefTony, James, Daria, Basketballs;
	private MusicPlayer mus;

	private Rigidbody2D[] balls;

	private Animator ctanim;

	// Use this for initialization
	void Start () {
		// get all the objects we'll need for the cutscene 
		ChefTony = GameObject.Find ("Chef Tony");
		James = GameObject.Find ("James");
		Daria = GameObject.Find ("Daria");
		Basketballs = GameObject.Find ("Flying BBalls");
		ctanim = ChefTony.GetComponent<Animator>();
		mus = GameObject.Find ("BGM").GetComponent<MusicPlayer>();

		balls = Basketballs.GetComponentsInChildren<Rigidbody2D>();

		stopTimer();
	}
	
	// Update is called once per frame
	public void FixedUpdate () {
		if(CurrentScene == 0) {
			ctanim.SetFloat("Speed", 2);

			if(ChefTony.transform.position.x > -2.3f) {
				ChefTony.GetComponent<ConstantVelocity>().enabled = false;
				ctanim.SetFloat("Speed", 0);
				startTimer();
				nextScene();
			}
		} else if(CurrentScene == 1) {
			if(timerIsGreaterThan(1.0f)) {
				nextScene();
			}
		} else if(CurrentScene == 4) {
			if(Daria.transform.position.x < 1.15f) {
				nextScene();
			}
		} else if(CurrentScene == 18) {
			if(timerIsGreaterThan(1.5f)) {
				nextScene();
			}
		} else if(CurrentScene == 30) {
			if(timerIsGreaterThan(1.0f)) {
				playSound(ElecZap);
				zapParticles = Instantiate(ZapParticlePrefab, new Vector3(-2.1f, -0.3f, 0), Quaternion.identity) as ParticleSystem;
				zapParticles.Play();

				startTimer();
				nextScene();
			}
		} else if(CurrentScene == 31) {
			ctanim.SetInteger("HP", 0);
			James.GetComponent<Animator>().SetInteger("HP", 0);
			FadeAndNext(Color.black, 4, "6-04 Another Fortress", true);
			nextScene();
		}
	}

	public override void HandleSceneChange() {
		if(CurrentScene == 4) {
			Daria.rigidbody2D.AddForce(Vector2.right * -600);
			mus.PlayMusic(DariasTheme, true);
		} else if (CurrentScene == 18) {
			foreach(Rigidbody2D ball in balls) {
				ball.AddForce(Vector2.right * -200);
			}

			startTimer();
		} else if(CurrentScene == 30) {
			foreach(Rigidbody2D ball in balls) {
				ball.AddForce(new Vector2(-100, -20));
			}

			startTimer();
		}
	}

}
