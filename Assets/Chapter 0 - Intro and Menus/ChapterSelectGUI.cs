using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChapterSelectGUI : MonoBehaviour {

	public GUISkin guiSkin;
	public AudioClip MenuSelectSound;

	private List<ChapterInfo> chapters;
	private int currentSave;

	private struct ChapterInfo {
		public int Number;
		public string DisplayName;
		public string SceneName;
	}

	void Start() {
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
		c.SceneName = "2-04 France 1";
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
		c.DisplayName = "Final Battle";
		c.SceneName = "6-09 Final Battle";
		chapters.Add (c);

	}


	void OnGUI() {
		GUI.skin = guiSkin;
		scaleGUI(guiSkin);

		GUILayout.BeginArea(AspectUtility.screenRect);
		
		GUILayout.BeginArea(new Rect(scalePx(15), scalePx(60), Screen.width - scalePx(20), Screen.height - scalePx(40)));

		numberOfButtonsVisible = 0;

		for(int j = 0; j < 4; j++) {
			GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));

			for(int i = j * 3; i < j * 3 + 3; i++) {
				ChapterInfo c = chapters[i];

				if(currentSave < i) { 
					GUI.enabled = false;
					GUILayout.Button("Chapter " + c.Number + "\n<b>???</b>");
					GUI.enabled = true;

				} else {
					GUI.SetNextControlName (i.ToString());

					if(GUILayout.Button("Chapter " + c.Number + "\n<b>" + c.DisplayName + "</b>")) {
						jumpToLevel(c.SceneName);
					}
					numberOfButtonsVisible++;
				}
			}

			GUILayout.EndHorizontal();
		}

		GUILayout.EndArea();
		GUILayout.EndArea();

		checkKeyControlFocus();

	}


	bool levelSelected = false;


	private void jumpToLevel(string scene) {
		if(!levelSelected) {
			AudioSource.PlayClipAtPoint(MenuSelectSound, Vector3.zero);

			StartCoroutine(FadeAndNext(Color.black, 2.0f, scene));

			levelSelected = true;
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

	private bool input1IsDown = false;

	void Update() {
		input1IsDown = Input.GetButtonDown("Select");
	}

	// keyboard control globals
	private int numberOfButtonsVisible = 0;
	private int currentButtonSelection = 0;
	private bool dirKeyDownV = false;
	private bool dirKeyDownH = false;

	private void checkKeyControlFocus() {
		float v = Input.GetAxis("Vertical");
		
		if(!dirKeyDownV) { 
			if(v != 0) {
				int origSel = currentButtonSelection;

				if(v < 0f) {
					currentButtonSelection += 3;
				} else if (v > 0f) {
					currentButtonSelection -= 3;
				}
				
				if(currentButtonSelection < numberOfButtonsVisible && currentButtonSelection >= 0) {
					AudioSource.PlayClipAtPoint(MenuSelectSound, Camera.main.transform.position);
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

		float h = Input.GetAxis("Horizontal");
		
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
					AudioSource.PlayClipAtPoint(MenuSelectSound, Camera.main.transform.position);
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

		if(input1IsDown) {
			jumpToLevel(chapters[currentButtonSelection].SceneName);
		}
		
	}
}
