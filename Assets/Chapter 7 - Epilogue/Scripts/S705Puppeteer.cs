using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S705Puppeteer : CutscenePuppeteer {

	public float CreditsSpeed = 1.0f;

	private GameObject credits;
		
	// Use this for initialization
	void Start () {
		credits = GameObject.Find("Credits");
	}
	
	// Update is called once per frame
	public void Update () {
		if(CurrentScene == 1) {
			Vector3 pos = credits.transform.position;
			pos.y += CreditsSpeed * Time.deltaTime;
			credits.transform.position = pos;
		}
	}

	public override void HandleSceneChange() {

	}

}
