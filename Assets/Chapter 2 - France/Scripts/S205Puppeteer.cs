using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S205Puppeteer : CutscenePuppeteer {


	private GameObject chefTony;
	
		
	// Use this for initialization
	void Start () {
		// get all the objects we'll need for the cutscene 
		chefTony = GameObject.Find ("Chef Tony Cart");

	}
	
	// Update is called once per frame
	public void FixedUpdate () {

	}

	public override void HandleSceneChange() {

	}

}
