using UnityEngine;
using System.Collections;

public class PauseGUI : BaseGUI {

	private bool isPaused = false;
	private bool battleWasPaused = false;
	private int guiDepth = -1001;

	private CameraFade fader;
	private MusicPlayer mus;

	public override void Start() {
		base.Start();
		fader = Camera.main.GetComponent<CameraFade>();

		GameObject bgm = GameObject.Find ("BGM");

		if(bgm) {
			mus = bgm.GetComponent<MusicPlayer>();
		}

		// save the current scene for the game over "try again" feature
		// (note: this doesn't really belong here, but every level has a PauseGUI script
		//  and so this where where I decided to put it)
		PlayerPrefs.SetString("LastScenePlayed", Application.loadedLevelName);

		PlayerPrefs.Save();
	}

	public override void Update() {
		base.Update();
		
		if(Input.GetButtonDown("Pause")) {
			if(!fader.isFading) {
				if(!isPaused) {
					pause();
				} else {
					unpause();
				}
			}
		}

		if(isPaused && Input.GetButtonDown("Select")) {
			if(currentButtonSelection == 0) {
				unpause();
			} else if(currentButtonSelection == 1) {
				goToMainMenu();
			}
		}
	}

	public override void OnGUI() {
		base.OnGUI();

		#if UNITY_ANDROID
		DrawMobilePauseButton();
		#endif

		if(isPaused) {
			GUI.depth = guiDepth; //appear over fades
			DrawPauseMenu();
		}
	}

	private void DrawMobilePauseButton() {
		GUILayout.BeginArea(AspectUtility.screenRect);

		if(GUI.Button(new Rect(AspectUtility.screenWidth - scalePx(40),
		                       AspectUtility.screenRect.y + scalePx(5),
		                       scalePx(35),
		                       scalePx(30)),
		              "\u275a \u275a")) {
			if(!isPaused) {
				pause();
			} else {
				unpause();
			}
		}

		GUILayout.EndArea();
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
		enableGuiControl();

		BattleController temp = GameObject.Find("Scripts").GetComponent<BattleController>();

		if(temp && temp.battleEnabled) {
			temp.PauseBattle();
			battleWasPaused = true;
		}

		CutsceneController cc = GameObject.Find("Scripts").GetComponent<CutsceneController>();

		if(cc) {
			cc.pauseDialog = true;
		}

		if(mus) {
			mus.Pause();
		}

		Time.timeScale = 0;
	}

	private void unpause() {
		isPaused = false;
		disableGuiControl();

		if(battleWasPaused) {
			GameObject.Find("Scripts").GetComponent<BattleController>().ResumeBattle();
			battleWasPaused = false;
		}

		if(mus) {
			mus.UnPause();
		}

		Time.timeScale = 1;
		Invoke("unpauseDialog", 0.1f);
	}

	
	private void goToMainMenu() {
		Time.timeScale = 1;
		guiDepth = 0;
		Camera.main.GetComponent<CameraFade>().FadeAndNext(Color.black, 2.0f, "0-03 Main menu", true);
	}


	private void unpauseDialog() {
		CutsceneController cc = GameObject.Find("Scripts").GetComponent<CutsceneController>();
		
		if(cc) {
			cc.pauseDialog = false;
		}
	}
}
