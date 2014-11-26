using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S610Puppeteer : CutscenePuppeteer {

	public AudioClip EpilogMusic, Rain, Explosion;

	private MusicPlayer mus;

	private ParticleSystem riftParticles;
	private float InitRiftIntensity = 0.05f;
	private float FinalRiftIntensity = 50.0f;
	private float RiftExpansionDuration = 5.0f;
	private float RiftExpansionDelay = 0.1f;


	// Use this for initialization
	void Start () {
		// get all the objects we'll need for the cutscene 
		riftParticles = GameObject.Find ("RiftParticles").GetComponent<ParticleSystem>();

		mus = GameObject.Find ("BGM").GetComponent<MusicPlayer>();

		playSound(Rain, true);
	}

	
	// Update is called once per frame
	public void FixedUpdate () {
		if(CurrentScene == 32) {
			if(elapsedTime > RiftExpansionDelay) {
				float animTime = elapsedTime - RiftExpansionDelay;
				
				if(animTime < RiftExpansionDuration) {
					riftParticles.startLifetime = 
						Mathf.Lerp(InitRiftIntensity, FinalRiftIntensity, animTime / RiftExpansionDuration);
				}
			}

			if(timerIsGreaterThan(5.0f)) {
				FadeAndNext(Color.white, 10, "6-10a Limbo", false);
				mus.PlayMusic(EpilogMusic, false);
				nextScene();
			}
		}


	}

	public override void HandleSceneChange() {
		if(CurrentScene == 1) {
			mus.PlayMusic();

		} else if(CurrentScene == 32) {
			playSound(Explosion);
			mus.StopMusic(3);

			riftParticles.Play();

			startTimer();
		}
	}


}
