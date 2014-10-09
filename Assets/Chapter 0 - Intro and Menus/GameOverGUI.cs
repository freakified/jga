using UnityEngine;
using System.Collections;

public class GameOverGUI : BaseGUI {

	private bool goingToMainMenu = false;

	public override void Start() {
		base.Start();

		keyboardControlEnabled = true;
	}

	public override void OnGUI() {
		base.OnGUI();

		GUILayout.BeginArea(AspectUtility.screenRect);

		GUILayout.BeginArea(new Rect(AspectUtility.screenWidth / 2 - scalePx (100), AspectUtility.screenHeight - scalePx(100) , scalePx(200), scalePx(200)));

		GUI.SetNextControlName("0");
		if(GUILayout.Button("Return to Main Menu", guiSkin.customStyles[5])) {
			goToMainMenu();
		}

		numberOfButtonsVisible = 1;

		GUILayout.EndArea();
		GUILayout.EndArea();

	}

	private void goToMainMenu() {
		if(!goingToMainMenu) {
			//FadeAndNext(Color.black, 2.0f, "0-03 Main menu", true);

			goingToMainMenu = true;
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
