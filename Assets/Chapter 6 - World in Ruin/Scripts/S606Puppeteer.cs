using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S606Puppeteer : CutscenePuppeteer {

	public AudioClip DariasTheme;

	private GameObject ChefTony, James, Daria;
	private MusicPlayer mus;
	

	// Use this for initialization
	void Start () {
		// get all the objects we'll need for the cutscene 
		ChefTony = GameObject.Find ("Chef Tony");
		James = GameObject.Find ("James");
		Daria = GameObject.Find ("Daria");
		mus = GameObject.Find ("BGM").GetComponent<MusicPlayer>();

	}
	
	// Update is called once per frame
	public void FixedUpdate () {

	}

	public override void HandleSceneChange() {

	}

}
