using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S610Puppeteer : CutscenePuppeteer {

	public AudioClip FlashbackSound, EvilMusic, FinalBossMusic;

	public List<Sprite> FlashbackBGList;
	private Queue<Sprite> flashbackBGs;

	private GameObject Flashback;
	private ScreenFlasher ScreenFlash;
	private MusicPlayer mus;


	// Use this for initialization
	void Start () {
		// get all the objects we'll need for the cutscene 
		Flashback = GameObject.Find ("Flashback");
		ScreenFlash = GameObject.Find ("ScreenFlash").GetComponent<ScreenFlasher>();

		mus = GameObject.Find ("BGM").GetComponent<MusicPlayer>();

		flashbackBGs = new Queue<Sprite>(FlashbackBGList);

	}

	
	// Update is called once per frame
	public void FixedUpdate () {

	}

	public override void HandleSceneChange() {

	}


}
