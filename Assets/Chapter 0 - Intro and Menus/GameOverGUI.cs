using UnityEngine;
using System.Collections;

public class GameOverGUI : BaseGUI {

	private bool somethingWasSelected = false;
	private int currentSave;

	public override void Start() {
		base.Start();

		currentSave = PlayerPrefs.GetInt("HighestCompletedChapter", -1);
		enableGuiControl();

	}

	public override void OnGUI() {
		base.OnGUI();

		GUILayout.BeginArea(AspectUtility.screenRect);

		GUILayout.BeginArea(new Rect(AspectUtility.screenWidth / 2 - scalePx (100), AspectUtility.screenHeight - scalePx(100) , scalePx(200), scalePx(200)));

		GUI.SetNextControlName("0");
		if(GUILayout.Button("Try Again", guiSkin.customStyles[5])) {
			replayChapter();
		}

		GUI.SetNextControlName("1");
		if(GUILayout.Button("Return to Main Menu", guiSkin.customStyles[5])) {
			goToMainMenu();
		}

		numberOfButtonsVisible = 2;

		GUILayout.EndArea();
		GUILayout.EndArea();

	}

	private void replayChapter() {
		if(!somethingWasSelected) {
			Camera.main.GetComponent<CameraFade>().FadeAndNext(Color.black, 2.0f, PlayerPrefs.GetString("LastScenePlayed", "0-03 Main menu"), true);

			somethingWasSelected = true;
		}
	}

	private void goToMainMenu() {
		if(!somethingWasSelected) {
			Camera.main.GetComponent<CameraFade>().FadeAndNext(Color.black, 2.0f, "0-03 Main menu", true);

			somethingWasSelected = true;
		}
	}

	public override void Update() {
		base.Update();

		if(input1IsDown) {
			if(currentButtonSelection == 0) {
				goToMainMenu();
			} 
		}
	}
	
}
