using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S704Puppeteer : CutscenePuppeteer {

	public float CameraVelocity = 5.0f;
	public GUIText SubtitleText;

	private Rigidbody cameraForce;
	private Vector3 cameraPos;
		
	// Use this for initialization
	void Start () {
		cameraForce = Camera.main.GetComponent<Rigidbody>();
		//cameraPos = cameraTransform.position;
		cameraForce.AddForce(Vector3.back * 2000);
	}
	
	// Update is called once per frame
	public void FixedUpdate () {
		if(CurrentScene == 0) {
			if(Camera.main.transform.position.z < 100) {
				cameraForce.drag = 0.75f;
				nextScene();
			}

		} else if (CurrentScene == 1) {
			SubtitleText.color = new Color(1, 1, 1, SubtitleText.color.a + Time.deltaTime);

			if(SubtitleText.color.a >= 0.98f) {
				startTimer();
				nextScene();
			}
		} else if(CurrentScene == 2) {
			if(timerIsGreaterThan(4.0f)) {
				FadeAndNext(Color.black, 5.0f, "7-05 Credits 1", false);
				nextScene();
			}
		}

		//Camera.main.transform.position += Vector3.back * CameraVelocity * Time.deltaTime;


	}

	public override void HandleSceneChange() {

	}

}
