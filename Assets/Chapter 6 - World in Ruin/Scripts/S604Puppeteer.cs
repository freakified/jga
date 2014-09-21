using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S604Puppeteer : CutscenePuppeteer {

	public ParticleSystem GasplosionParticles;
	public AudioClip EvilMusic;

	private GameObject ChefTony, James, Basketballs;
	private MusicPlayer mus;

	private Rigidbody2D[] balls;
	private Animator[] ballsAnim;
	
	// Use this for initialization
	void Start () {
		// get all the objects we'll need for the cutscene 
		ChefTony = GameObject.Find ("Chef Tony");
		James = GameObject.Find ("James");
		Basketballs = GameObject.Find ("Flying BBalls");
		mus = GameObject.Find ("BGM").GetComponent<MusicPlayer>();

		balls = Basketballs.GetComponentsInChildren<Rigidbody2D>();
		ballsAnim = Basketballs.GetComponentsInChildren<Animator>();

		//everyone is DEAD
		James.GetComponent<Animator>().SetInteger("HP", 0);
		ChefTony.GetComponent<Animator>().SetInteger("HP", 0);

		startTimer();
	}
	
	// Update is called once per frame
	public void FixedUpdate () {
		if(CurrentScene == 0) {
			if(timerIsGreaterThan(1.0f)) {
				nextScene();
			}
		} else if(CurrentScene == 6) {
			if(timerIsGreaterThan(3.0f)) {
				GasplosionParticles.Stop();

				foreach(Rigidbody2D ball in balls) {
					ball.gravityScale = 1;
					ball.AddForce(Vector2.right * Random.Range(-2, -6));
					ball.AddTorque(Random.Range(2, 5));
				}

				ballsAnim[0].enabled = false;
				ballsAnim[1].enabled = false;

				startTimer();
				nextScene();
			}
		} else if(CurrentScene == 7) {
			if(timerIsGreaterThan(2.5f)) {
				ChefTony.GetComponent<Animator>().SetInteger("HP", 100);
				nextScene();
			}
		} else if(CurrentScene == 10) {
			if(ChefTony.transform.position.x > -12f) {
				ChefTony.GetComponent<PlayerFreeze>().Freeze();
				James.transform.position = new Vector3(-17.02f, 1.0f, -4.065f);;
				James.rigidbody2D.isKinematic = false;
				James.GetComponent<JamesGasBehavior>().EnableFlightMode();
				James.rigidbody2D.AddForce(Vector2.right * 300.0f);
				nextScene();
			}
		} else if(CurrentScene == 11) {
			if(James.transform.position.x > -14f) {
				James.GetComponent<JamesGasBehavior>().DisableFlightMode();
				nextScene();
			}
		} else if(CurrentScene == 21) {
			if(timerIsGreaterThan(2.0f)) {
				James.transform.localScale = new Vector3(-1, 1, 1);
				ChefTony.transform.localScale = new Vector3(-1, 1, 1);
				nextScene();
			}

		}
	}

	public override void HandleSceneChange() {
		if(CurrentScene == 1) {
			GameObject textbox = GameObject.Find ("TextBox(Clone)");
			textbox.transform.localRotation = Quaternion.identity;
		} else if(CurrentScene == 6) {
			James.GetComponent<Animator>().SetInteger("HP", 100);
			GasplosionParticles.Play();
			startTimer();
		} else if(CurrentScene == 10) {
			ChefTony.GetComponent<PlayerFreeze>().UnFreeze();
			James.rigidbody2D.isKinematic = true;
		} else if(CurrentScene == 12) {
			mus.PlayMusic(EvilMusic, false);
		} else if(CurrentScene == 21) {
			balls[2].transform.localPosition = new Vector3(7.0f,  
			                                          		balls[2].transform.localPosition.y,
			                                               balls[2].transform.localPosition.z);
			balls[2].AddForce(Vector2.right * 200f);
			balls[2].AddTorque(10.0f);
			startTimer();
		} else if(CurrentScene == 35) {
			ballsAnim[2].enabled = false;
			James.transform.localScale = Vector3.one;
		} else if(CurrentScene == 44) {
			ChefTony.transform.localScale = Vector3.one;
			James.rigidbody2D.isKinematic = true;
			ChefTony.GetComponent<PlayerFreeze>().UnFreeze();
		}
	}

}
