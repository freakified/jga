using UnityEngine;
using System.Collections;

public class MainMenuGUI : MonoBehaviour {

	public GUISkin guiSkin;
	public AudioClip MenuSelectSound;

	bool buttonPressed = false;

	void OnGUI() {
		GUI.skin = guiSkin;
		scaleGUI(guiSkin);

		GUILayout.BeginArea(new Rect((Screen.width/2)-scalePx(100), (Screen.height/2 + scalePx(50)) , scalePx(200), scalePx(100)));


		if(GUILayout.Button("Begin Game")) {
			startGame();
		}

		GUILayout.EndArea();
	}

	void Update() {
		if(Input.GetButtonDown("Select")) {
			startGame();
		}
	}

	private void startGame() {
		if(!buttonPressed) {
			AudioSource.PlayClipAtPoint(MenuSelectSound, Vector3.zero);
			//TODO: this should automatically skip the chapter scene if only one chapter has been unlocked
			StartCoroutine(FadeAndNext(Color.black, 2.0f, "0-04 Chapter Selection"));
		}	
	}

	private void scaleGUI(GUISkin guiSkin) {
		//fonts
		guiSkin.button.fontSize = scalePx (18);

		//padding for buttons
		guiSkin.button.padding.left = scalePx (20);
		guiSkin.button.padding.right = scalePx (20);
		guiSkin.button.padding.top = scalePx (15);
		guiSkin.button.padding.bottom = scalePx (15);
		guiSkin.button.alignment = TextAnchor.MiddleCenter;
	}

	private int scalePx(int targetSize) {
		return (int)((targetSize * Screen.width) / 640);
	}

	protected IEnumerator FadeAndNext(Color fadeTo, float seconds, string nextScene) {
		CameraFade fader = Camera.main.GetComponent<CameraFade>();

		fader.SetScreenOverlayColor (new Color(fadeTo.r, fadeTo.g, fadeTo.b, 0));
		fader.StartFade(fadeTo, seconds);
		yield return new WaitForSeconds(seconds);
		if(nextScene != null)
			Application.LoadLevel(nextScene);
	}
}
