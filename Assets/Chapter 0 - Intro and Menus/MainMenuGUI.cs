using UnityEngine;
using System.Collections;

public class MainMenuGUI : MonoBehaviour {

	public GUISkin guiSkin;
	public AudioClip MenuSelectSound;

	private int currentSave;

	bool buttonPressed = false;

	void Start() {
		currentSave = PlayerPrefs.GetInt("HighestCompletedChapter", -1);
	}

	void OnGUI() {
		GUI.skin = guiSkin;
		scaleGUI(guiSkin);

		GUILayout.BeginArea(AspectUtility.screenRect);

		GUILayout.BeginArea(new Rect(scalePx(20), (Screen.height - scalePx(110)) , scalePx(200), scalePx(200)));

		numberOfButtonsVisible = 0;

		GUI.SetNextControlName("0");
		if(GUILayout.Button("Start New Game")) {
			beginGame();
		}

		numberOfButtonsVisible++;

		// do they have a save?
		if(currentSave >= 0) {
			GUI.enabled = true;
			GUI.SetNextControlName("1");
			numberOfButtonsVisible++;
		} else {
			GUI.enabled = false;
		}

		if(GUILayout.Button("Chapter Selection")) {
			goToChapterSelect();
		}



		GUILayout.EndArea();
		GUILayout.EndArea();

		checkKeyControlFocus();
	}

	private void beginGame() {
		if(!buttonPressed) {
			StartCoroutine(FadeAndNext(Color.black, 2.0f, "01 Elevator Entry"));

			buttonPressed = true;
		}
	}

	protected IEnumerator FadeAndNext(Color fadeTo, float seconds, string nextScene) {
		CameraFade fader = Camera.main.GetComponent<CameraFade>();
		
		fader.SetScreenOverlayColor (new Color(fadeTo.r, fadeTo.g, fadeTo.b, 0));
		fader.StartFade(fadeTo, seconds);
		yield return new WaitForSeconds(seconds);
		if(nextScene != null)
			Application.LoadLevel(nextScene);
	}

	private void goToChapterSelect() {
		if(!buttonPressed) {
			AudioSource.PlayClipAtPoint(MenuSelectSound, Vector3.zero);
			//TODO: this should automatically skip the chapter scene if only one chapter has been unlocked
			//StartCoroutine(FadeAndNext(Color.black, 2.0f, "0-04 Chapter Selection"));
			Application.LoadLevel("0-04 Chapter Selection");

			buttonPressed = true;
		}	
	}

	void Update() {
		input1IsDown = Input.GetButtonDown("Select");
	}

	private void scaleGUI(GUISkin guiSkin) {
		//fonts
		guiSkin.button.fontSize = scalePx (17);
		guiSkin.button.margin.bottom = scalePx(10);

		//padding for buttons
		guiSkin.button.padding.left = scalePx (15);
		guiSkin.button.padding.right = scalePx (15);
		guiSkin.button.padding.top = scalePx (10);
		guiSkin.button.padding.bottom = scalePx (10);
		guiSkin.button.alignment = TextAnchor.MiddleCenter;
		guiSkin.button.fixedWidth = scalePx (200);
	}

	private int scalePx(int targetSize) {
		return (int)((targetSize * Screen.width) / 640);
	}

	private int numberOfButtonsVisible = 0;
	private int currentButtonSelection = 0;
	private bool dirKeyDown = false;
	
	private bool input1IsDown = false;
	private bool buttonKeyDown = true;

	private void checkKeyControlFocus() {
		float v = Input.GetAxis("Vertical");
		
		if(!dirKeyDown) { 
			if(v != 0) {
				if(v < 0) {
					currentButtonSelection++;
				} else {
					currentButtonSelection--;
				}
				
				if(currentButtonSelection < numberOfButtonsVisible && currentButtonSelection >= 0) {
					AudioSource.PlayClipAtPoint(MenuSelectSound, Camera.main.transform.position);
				} else {
					currentButtonSelection = Mathf.Clamp(currentButtonSelection, 0, numberOfButtonsVisible - 1);
				}
				
				dirKeyDown = true;
			}
		} else {
			if(v == 0) {
				dirKeyDown = false;
			}
		}
		
		GUI.FocusControl(currentButtonSelection.ToString());

		if(input1IsDown) {
			if(currentButtonSelection == 0) {
				beginGame();
			} else if(currentButtonSelection == 1) {
				goToChapterSelect();
			}
		}
		
	}
}
