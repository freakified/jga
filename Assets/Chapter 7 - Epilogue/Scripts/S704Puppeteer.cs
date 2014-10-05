using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S704Puppeteer : CutscenePuppeteer {

	public float CameraVelocity = 5.0f;
	public GUIText SubtitleText;

	private Transform cameraTransform;
	private Vector3 cameraPos;
		
	// Use this for initialization
	void Start () {
		cameraTransform = Camera.main.transform;
		cameraPos = cameraTransform.position;
	}
	
	// Update is called once per frame
	public void Update () {
		if(CurrentScene == 0) {
			//Camera.main.transform.position += Vector3.back * CameraVelocity * Time.deltaTime;

			if(Camera.main.transform.position.z < 0) {
				nextScene();
			}

		} else if (CurrentScene == 1) {
			SubtitleText.color = new Color(1, 1, 1, SubtitleText.color.a + Time.deltaTime);

			if(SubtitleText.color.a >= 0.98f) {
				StartCoroutine(FadeAndNext(Color.black, 10.0f, null));
				nextScene();
			}
		}

		Camera.main.transform.position += Vector3.back * CameraVelocity * Time.deltaTime;


	}

	public override void HandleSceneChange() {

	}

}
