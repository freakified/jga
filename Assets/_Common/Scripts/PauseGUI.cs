using UnityEngine;
using System.Collections;

public class PauseGUI : MonoBehaviour {

	public GUISkin guiSkin;
	public AudioClip MenuSelectSound;

	private bool isPaused = false;

	public void OnGUI() {
		if(isPaused) {
			DrawPauseMenu();
		}
	}

	public void DrawPauseMenu() {
		GUI.skin = guiSkin;
		scaleGUI(guiSkin);
		
		GUILayout.BeginArea(AspectUtility.screenRect, "", guiSkin.customStyles [0]);

		GUILayout.Label("Paused", guiSkin.customStyles[4]);

		
		GUILayout.BeginArea(new Rect(AspectUtility.screenWidth / 2 - scalePx (100), AspectUtility.screenHeight - scalePx(130) , scalePx(200), scalePx(200)));


		GUI.SetNextControlName("0");
		if(GUILayout.Button("Resume Game")) {
			unpause();
		}

		GUI.SetNextControlName("1");
		if(GUILayout.Button("Return To Main Menu")) {
			goToMainMenu();
		}
		
		numberOfButtonsVisible = 2;
		
		GUILayout.EndArea();
		GUILayout.EndArea();
		
		checkKeyControlFocus();
	}

	
	public void Update() {
		input1IsDown = Input.GetButtonDown("Select");

		if(Input.GetButtonDown("Pause")) {
			if(!isPaused) {
				pause();
			} else {
				unpause();
			}
		}
	}
	
	private void scaleGUI(GUISkin guiSkin) {
		//fonts
		guiSkin.button.fontSize = scalePx (17);
		guiSkin.button.margin.bottom = scalePx(10);

		guiSkin.customStyles[4].fontSize = scalePx(30);
		
		//padding for buttons
		guiSkin.button.padding.left = scalePx (15);
		guiSkin.button.padding.right = scalePx (15);
		guiSkin.button.padding.top = scalePx (10);
		guiSkin.button.padding.bottom = scalePx (10);
		guiSkin.button.margin.left = 0;
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
	
	private void checkKeyControlFocus() {
		float v = Input.GetAxisRaw("Vertical");

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
				unpause();
			} else if(currentButtonSelection == 1) {
				goToMainMenu();
			}
		}
		
	}

	private void goToMainMenu() {
		Time.timeScale = 1;
		FadeAndNext(Color.black, 2.0f, "0-03 Main menu", true);
	}

	private bool battleWasPaused = false;

	private void pause() {
		isPaused = true;

		BattleController temp = GameObject.Find("Scripts").GetComponent<BattleController>();

		if(temp && temp.battleEnabled) {
			temp.PauseBattle();
			battleWasPaused = true;
		}

		CutsceneController cc = GameObject.Find("Scripts").GetComponent<CutsceneController>();

		if(cc) {
			cc.pauseDialog = true;
		}

		Time.timeScale = 0;
	}

	private void unpause() {
		isPaused = false;

		if(battleWasPaused) {
			GameObject.Find("Scripts").GetComponent<BattleController>().ResumeBattle();
			battleWasPaused = false;
		}



		Time.timeScale = 1;
		Invoke("unpausedialog", 0.1f);
	}

	private void unpausedialog() {
		CutsceneController cc = GameObject.Find("Scripts").GetComponent<CutsceneController>();
		
		if(cc) {
			cc.pauseDialog = false;
		}
	}

	// TODO make this code not repeat everywhere
	public void FadeAndNext(Color fadeTo, float seconds, string nextScene, bool fadeMusic) {
		
		if(fadeMusic && GameObject.Find("BGM")) {
			GameObject.Find("BGM").GetComponent<MusicPlayer>().StopMusic(seconds / 2);
		}
		
		StartCoroutine(FadeAndNext(fadeTo, seconds, nextScene));
	}
	
	private IEnumerator FadeAndNext(Color fadeTo, float seconds, string nextScene) {
		CameraFade fader = Camera.main.GetComponent<CameraFade>();
		
		fader.SetScreenOverlayColor (new Color(fadeTo.r, fadeTo.g, fadeTo.b, 0));
		fader.StartFade(fadeTo, seconds);
		yield return new WaitForSeconds(seconds);
		if(nextScene != null)
			Application.LoadLevel(nextScene);
	}
}
