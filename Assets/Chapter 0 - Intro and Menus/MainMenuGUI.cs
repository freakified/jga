using UnityEngine;
using System.Collections;

public class MainMenuGUI : BaseGUI {
	private int currentSave;
	private bool actionTaken = false;

	public override void Start() {
		base.Start();

		enableGuiControl();

		currentSave = PlayerPrefs.GetInt("HighestCompletedChapter", -1);
	}

	public override void OnGUI() {
		base.OnGUI();

		GUILayout.BeginArea(AspectUtility.screenRect);

		GUILayout.BeginArea(new Rect(scalePx(20), (AspectUtility.screenHeight - scalePx(150)) , scalePx(200), scalePx(200)));

		numberOfButtonsVisible = 0;

		GUI.SetNextControlName("0");
		if(GUILayout.Button("Start New Game", guiSkin.customStyles[5])) {
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

		if(GUILayout.Button("Chapter Selection", guiSkin.customStyles[5])) {
			goToChapterSelect();
		}

		GUI.enabled = true;
		GUI.SetNextControlName(numberOfButtonsVisible.ToString());
		if(GUILayout.Button("Quit JGA", guiSkin.customStyles[5])) {
			Application.Quit();
		}
		
		numberOfButtonsVisible++;

		GUILayout.EndArea();
		GUILayout.EndArea();
	}


	public override void Update() {
		base.Update();

		if(input1IsDown) {
			if(currentButtonSelection == 0) {
				beginGame();
			} else if(currentButtonSelection == 1) {
				if(currentSave >= 0) {
					goToChapterSelect();
				} else {
					Application.Quit();
				}
			} 
		}
	}

	private void beginGame() {
		if(!actionTaken) {
			GameObject.Find("BGM").GetComponent<MusicPlayer>().StopMusic(1.0f);
			Camera.main.GetComponent<CameraFade>().FadeAndNext(Color.black, 2.0f, "01 Elevator Entry");
			
			actionTaken = true;
		}
	}
	
	private void goToChapterSelect() {
		if(!actionTaken) {
			AudioSource.PlayClipAtPoint(cursorMoveSound, Vector3.zero);
			Application.LoadLevel("0-04 Chapter Selection");
			
			actionTaken = true;
		}	
	}
	
}
