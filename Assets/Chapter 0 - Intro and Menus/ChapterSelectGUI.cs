using UnityEngine;
using System.Collections;

public class ChapterSelectGUI : MonoBehaviour {

	public GUISkin guiSkin;
	public AudioClip MenuSelectSound;

	bool buttonPressed = false;

	void OnGUI() {
		GUI.skin = guiSkin;
		scaleGUI(guiSkin);

		GUILayout.BeginArea(new Rect(scalePx(15), scalePx(50), Screen.width - scalePx(20), Screen.height - scalePx(40)));
		GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));

		if(GUILayout.Button("Chapter 1\n<b>Opening</b>")) {
			startGame();
		}

		if(GUILayout.Button("Chapter 2\n<b>Seek the Shoes</b>")) {
			startGame();
		}

		if(GUILayout.Button("Chapter 3\n<b>All downhill from here</b>")) {
			startGame();
		}

		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));
		
		if(GUILayout.Button("Chapter 1\n<b>Opening</b>")) {
			startGame();
		}
		
		if(GUILayout.Button("Chapter 2\n<b>Seek the Shoes</b>")) {
			startGame();
		}
		
		if(GUILayout.Button("Chapter 3\n<b>All downhill from here</b>")) {
			startGame();
		}
		
		GUILayout.EndHorizontal();



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
		guiSkin.button.fontSize = scalePx (16);

		guiSkin.button.margin.top = 0;
		guiSkin.button.margin.bottom = scalePx(10);
		guiSkin.button.margin.right = scalePx(10);
		guiSkin.button.margin.left = scalePx(10);

		//padding for buttons
		guiSkin.button.padding.left = scalePx (15);
		guiSkin.button.padding.right = scalePx (10);
		guiSkin.button.padding.top = scalePx (10);
		guiSkin.button.padding.bottom = scalePx (10);
		guiSkin.button.fixedWidth = (Screen.width - scalePx(50)) / 3;
		guiSkin.button.alignment = TextAnchor.MiddleLeft;
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
