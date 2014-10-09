using UnityEngine;
using System.Collections;

public class PauseGUI : BaseGUI {

	private bool isPaused = false;
	private bool battleWasPaused = false;

	public override void Start() {
		base.Start();
	}

	public override void Update() {
		base.Update();
		
		if(Input.GetButtonDown("Pause")) {
			if(!isPaused) {
				pause();
			} else {
				unpause();
			}
		}
	}

	public override void OnGUI() {
		base.OnGUI();

		if(isPaused) {
			DrawPauseMenu();
		}
	}

	private void DrawPauseMenu() {
		GUILayout.BeginArea(AspectUtility.screenRect, "", guiSkin.customStyles [0]);

		GUILayout.Label("Paused", guiSkin.customStyles[4]);
		
		GUILayout.BeginArea(new Rect(AspectUtility.screenWidth / 2 - scalePx (100), AspectUtility.screenHeight - scalePx(130) , scalePx(200), scalePx(200)));


		GUI.SetNextControlName("0");
		if(GUILayout.Button("Resume Game", guiSkin.customStyles[5])) {
			unpause();
		}

		GUI.SetNextControlName("1");
		if(GUILayout.Button("Return To Main Menu", guiSkin.customStyles[5])) {
			goToMainMenu();
		}
		
		numberOfButtonsVisible = 2;
		
		GUILayout.EndArea();
		GUILayout.EndArea();
		
	}
	private void pause() {
		isPaused = true;
		keyboardControlEnabled = true;

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
		keyboardControlEnabled = false;

		if(battleWasPaused) {
			GameObject.Find("Scripts").GetComponent<BattleController>().ResumeBattle();
			battleWasPaused = false;
		}



		Time.timeScale = 1;
		Invoke("unpauseDialog", 0.1f);
	}

	
	private void goToMainMenu() {
		Time.timeScale = 1;
		FadeAndNext(Color.black, 2.0f, "0-03 Main menu", true);
	}


	private void unpauseDialog() {
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
