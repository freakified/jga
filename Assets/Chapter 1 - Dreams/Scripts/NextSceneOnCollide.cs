using UnityEngine;
using System.Collections;

public class NextSceneOnCollide : MonoBehaviour {

	private CameraFade fader;

	// get the camera fade script
	void Start () {
		fader = GameObject.Find ("Scripts").GetComponent<CameraFade>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Player") {
			coll.gameObject.GetComponent<PlayerFreeze>().Freeze();

			StartCoroutine(FadeAndNext());
		}

	}

	IEnumerator FadeAndNext() {
		fader.StartFade(new Color(0, 0, 0, 1), 1);
		yield return new WaitForSeconds(1);
		Application.LoadLevel("02-Elevator Entry 2");
	}
}
