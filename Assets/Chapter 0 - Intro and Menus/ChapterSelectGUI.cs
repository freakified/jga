using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChapterSelectGUI : BaseGUI {
	
	private List<ChapterInfo> chapters;
	private int currentSave;

	private bool levelSelected = false;

	private struct ChapterInfo {
		public int Number;
		public string DisplayName;
		public string SceneName;
	}

	public override void Start() {
		base.Start();

		enableGuiControl();

		// get saved chapter
		currentSave = PlayerPrefs.GetInt("HighestCompletedChapter", -1);

		//populate chapters list
		chapters = new List<ChapterInfo>();
		int currentNum = 1;

		ChapterInfo c;


		// 0
		c.Number = currentNum;
		c.DisplayName = "Opening";
		c.SceneName = "01 Elevator Entry";
		chapters.Add (c);
		currentNum++;

		// 1
		c.Number = currentNum;
		c.DisplayName = "Seek the Shoes";
		c.SceneName = "2-02 France 1";
		chapters.Add (c);
		currentNum++;

		// 2
		c.Number = currentNum;
		c.DisplayName = "Going Downhill";
		c.SceneName = "2-05 Cart Race";
		chapters.Add (c);
		currentNum++;

		// 3
		c.Number = currentNum;
		c.DisplayName = "A No-Kill Home";
		c.SceneName = "3-10 Orphanage";
		chapters.Add (c);
		currentNum++;

		// 4
		c.Number = currentNum;
		c.DisplayName = "The Prophesy";
		c.SceneName = "4-01 Shrine Exterior";
		chapters.Add (c);
		currentNum++;

		// 5
		c.Number = currentNum;
		c.DisplayName = "Infiltration";
		c.SceneName = "5-01 Outside Facility";
		chapters.Add (c);
		currentNum++;

		// 6
		c.Number = currentNum;
		c.DisplayName = "Déjà Vu";
		c.SceneName = "5-05 Elevator Entry";
		chapters.Add (c);
		currentNum++;

		// 7
		c.Number = currentNum;
		c.DisplayName = "Sacrifice";
		c.SceneName = "5-07 Control Room";
		chapters.Add (c);
		currentNum++;

		// 8
		c.Number = currentNum;
		c.DisplayName = "World in Ruin";
		c.SceneName = "6-01 World in ruin";
		chapters.Add (c);
		currentNum++;

		// 9
		c.Number = currentNum;
		c.DisplayName = "The Darkness Within";
		c.SceneName = "6-05 Entrance to EVIL";
		chapters.Add (c);
		currentNum++;

		// 10
		c.Number = currentNum;
		c.DisplayName = "The Ultimate Game";
		c.SceneName = "6-07 Fiddling contest";
		chapters.Add (c);
		currentNum++;

		// 11
		c.Number = currentNum;
		c.DisplayName = "True Power";
		c.SceneName = "6-09 The Final Battle";
		chapters.Add (c);

	}


	public override void OnGUI() {
		base.OnGUI();

		GUILayout.BeginArea(AspectUtility.screenRect);
		
		GUILayout.BeginArea(new Rect(scalePx(15), scalePx(40), Screen.width - scalePx(20), Screen.height - scalePx(40)));

		numberOfButtonsVisible = 0;

		for(int j = 0; j < 4; j++) {
			GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));

			for(int i = j * 3; i < j * 3 + 3; i++) {
				ChapterInfo c = chapters[i];

				if(currentSave < i) { 
					GUI.enabled = false;
					GUILayout.Button("Chapter " + c.Number + "\n<b>???</b>", guiSkin.customStyles[6]);
					GUI.enabled = true;

				} else {
					GUI.SetNextControlName (i.ToString());

					if(GUILayout.Button("Chapter " + c.Number + "\n<b>" + c.DisplayName + "</b>", guiSkin.customStyles[6])) {
						jumpToLevel(c.SceneName);
					}
					numberOfButtonsVisible++;
				}
			}

			GUILayout.EndHorizontal();
		}

		GUI.SetNextControlName (numberOfButtonsVisible.ToString());
		
		if(GUI.Button(new Rect(0,
		                       AspectUtility.screenHeight - scalePx(80),
		                       guiSkin.customStyles[6].fixedWidth,
		                       scalePx(30)),
			"\u2190 Back to Main Menu")) {
			Application.LoadLevel("0-03 Main Menu");
		}
		
		numberOfButtonsVisible++;


		GUILayout.EndArea();
		GUILayout.EndArea();



	}

	public override void Update() {
		base.Update();

		if(input1IsDown) {
			if(currentButtonSelection == chapters.Count) {
				Application.LoadLevel("0-03 Main Menu"); // the back button was pressed
			} else {
				jumpToLevel(chapters[currentButtonSelection].SceneName);
			}
		} else if(input2IsDown) {
			jumpToLevel(chapters[currentButtonSelection].SceneName);
		}
	}
	
	private void jumpToLevel(string scene) {
		if(!levelSelected) {
			AudioSource.PlayClipAtPoint(cursorMoveSound, Vector3.zero);

			GameObject.Find("BGM").GetComponent<MusicPlayer>().StopMusic(1.0f);
			Camera.main.GetComponent<CameraFade>().FadeAndNext(Color.black, 2.0f, scene);

			levelSelected = true;
		}	
	}

	// special horizontal keyboard control globals
	protected bool dirKeyDownV = false;
	protected bool dirKeyDownH = false;

	// special horizontal keyboard control
	protected override void checkKeyControlFocus() {
		float v = Input.GetAxisRaw("Vertical");
		
		if(!dirKeyDownV) { 
			if(v != 0) {
				int origSel = currentButtonSelection;

				if(v < 0f) {
					currentButtonSelection += 3;
				} else if (v > 0f) {
					currentButtonSelection -= 3;
				}
				
				if(currentButtonSelection < numberOfButtonsVisible && currentButtonSelection >= 0) {
					AudioSource.PlayClipAtPoint(cursorMoveSound, Camera.main.transform.position);
				} else {
					currentButtonSelection = origSel;
				}
				
				dirKeyDownV = true;
			}
		} else {
			if(v == 0) {
				dirKeyDownV = false;
			}
		}

		float h = Input.GetAxisRaw("Horizontal");
		
		if(!dirKeyDownH) { 

			if(h != 0) {
				int origSel = currentButtonSelection;

				if(h < 0f) {
					if(currentButtonSelection % 3 != 0) {
						currentButtonSelection -= 1;
					}	
				} else if (h > 0f) {
					if((currentButtonSelection + 1) % 3 != 0) {
						currentButtonSelection += 1;
					}
				}
				

				if(origSel != currentButtonSelection) {
					AudioSource.PlayClipAtPoint(cursorMoveSound, Camera.main.transform.position);
				}

				dirKeyDownH = true;
			}
		} else {
			if(h == 0) {
				dirKeyDownH = false;
			}
		}

		currentButtonSelection = Mathf.Clamp(currentButtonSelection, 0, numberOfButtonsVisible - 1);
		
		GUI.FocusControl(currentButtonSelection.ToString());

		
	}
}
