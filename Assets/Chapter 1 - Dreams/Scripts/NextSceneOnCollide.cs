using UnityEngine;
using System.Collections;

public class NextSceneOnCollide : MonoBehaviour {


	public string NextSceneName = null;
	public Color FadeToColor = new Color(0, 0, 0, 1);
	public float FadeDuration = 1.0f;

	private CameraFade fader;

	// get the camera fade script
	void Start () {
		fader = Camera.main.GetComponent<CameraFade>();
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
		fader.StartFade(FadeToColor, FadeDuration);
		yield return new WaitForSeconds(FadeDuration);

		if(NextSceneName != "") {
			Application.LoadLevel(NextSceneName);
		}
	}
}
