using UnityEngine;
using System.Collections;

public class GameOverGUI : CutscenePuppeteer {

	public GUISkin guiSkin;
	public AudioClip MenuSelectSound;
	
	bool buttonPressed = false;

	void OnGUI() {
		GUI.skin = guiSkin;
		scaleGUI(guiSkin);

		GUILayout.BeginArea(AspectUtility.screenRect);

		GUILayout.BeginArea(new Rect(AspectUtility.screenWidth / 2 - scalePx (100), AspectUtility.screenHeight - scalePx(100) , scalePx(200), scalePx(200)));

		GUI.SetNextControlName("0");
		if(GUILayout.Button("Return to Main Menu")) {
			goToMainMenu();
		}

		numberOfButtonsVisible = 1;

		GUILayout.EndArea();
		GUILayout.EndArea();

		checkKeyControlFocus();
	}

	private void goToMainMenu() {
		if(!buttonPressed) {
			FadeAndNext(Color.black, 2.0f, "0-03 Main menu", true);

			buttonPressed = true;
		}
	}

	public override void Update() {
		base.Update();

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
				goToMainMenu();
			} 
		}
		
	}

	public override void HandleSceneChange() {
		
	}
}
