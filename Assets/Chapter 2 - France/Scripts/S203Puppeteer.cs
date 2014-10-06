using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S203Puppeteer : CutscenePuppeteer {

	public AudioClip evilMusic;
	public AudioClip hangupSound;

	private MusicPlayer mp;

	// Use this for initialization
	void Start () {
		mp = GameObject.Find("BGM").GetComponent<MusicPlayer>();
	}
	
	// Update is called once per frame
	public void FixedUpdate () {

//		if(CurrentScene == 3) {
//			StartCoroutine(FadeAndNext(Color.white, 0, "2-02 France 1"));
//			nextScene();
//		}
//		
	}

	public override void HandleSceneChange() {
		if(CurrentScene == 14) {
			mp.PlayMusic(evilMusic, false);
		} else if (CurrentScene == 28) {
			playSound(hangupSound);
			FadeAndNext(Color.black, 2, "2-04 France 2", true);
		}
	}

}
