using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S404Puppeteer : CutscenePuppeteer {

	public ParticleSystem WarpParticles;

	public List<SpriteRenderer> BackgroundLayers;
	public AudioClip LikeMikeMusic;
	public AudioClip EvilMusic;

	private GameObject flyingBasketball;
	private MusicPlayer mus;
		
	// Use this for initialization
	void Start () {
		flyingBasketball = GameObject.Find ("FlyingBasketball");
		mus = GameObject.Find ("BGM").GetComponent<MusicPlayer>();
	}
	
	// Update is called once per frame
	public void FixedUpdate () {

		if(CurrentScene >= 25 && BackgroundLayers[0].color != new Color(1, 1, 1, 0)) {
			Color colorFade = new Color(1.0f, 1.0f, 1.0f, Mathf.Lerp(1.0f, 0.0f, elapsedTime / 2));
			BackgroundLayers.ForEach(layer => layer.color = colorFade);
		}

		if (CurrentScene == 26) {
			flyingBasketball.rigidbody2D.AddForce(Vector3.zero - flyingBasketball.transform.position);

			if(flyingBasketball.transform.position.x < 0.01 &&
			   flyingBasketball.transform.position.x > -0.01) {
				WarpParticles.Play();
				startTimer();
				nextScene();
			}
		} else if(CurrentScene == 27) {

			if(!timerIsGreaterThan(1.0f)) {
				flyingBasketball.rigidbody2D.AddTorque(2.0f);
			} else {
				StartCoroutine(FadeAndNext(new Color(1.0f, 0.47f, 0.2f), 2.0f, null));
				nextScene();
			}
		}
	}

	public override void HandleSceneChange() {
		if (CurrentScene == 6) {
			//basketball interjection
			mus.PlayMusic(LikeMikeMusic, false);

			flyingBasketball.rigidbody2D.AddForce(new Vector2(-150, -10));
		} else if (CurrentScene == 19) {
			mus.PlayMusic(EvilMusic, false);
		} else if (CurrentScene == 25) {
			startTimer();
			mus.StopMusic(2.0f);
			// next: fade the background to black like we did before
		} else if (CurrentScene == 26) {
			flyingBasketball.GetComponent<FBHoverBehavior> ().enabled = false;

		}
	}

}
