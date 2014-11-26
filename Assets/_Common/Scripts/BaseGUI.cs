using UnityEngine;
using System.Collections;

public class BaseGUI : MonoBehaviour {

	protected GUISkin guiSkin;
	protected AudioClip cursorMoveSound;

	// gui scaling info
	protected int targetScreenWidth = 640;

	// control (mostly keyboard/input) globals
	private bool guiControlEnabled = false;

	// this allows for multiple BaseGUIs to not constantly hide each others' cursors
	private static int cursorShowRequests = 0;

	protected int numberOfButtonsVisible = 0;
	protected int currentButtonSelection = 0;
	protected bool dirKeyDown = false;
	protected bool input1IsDown = false;
	protected bool input2IsDown = false;
	protected float elapsedTime = 0;

	public virtual void Start() {
		//load menu select sound
		cursorMoveSound = Resources.Load<AudioClip>("menu_select");

		//load gui skin
		guiSkin = Resources.Load<GUISkin>("DefaultSkin");
	}

	public virtual void OnDestroy() {
		disableGuiControl();
	}
	
	public virtual void Update() {
		if(guiControlEnabled) {
			elapsedTime += Time.deltaTime;

			if(cursorShowRequests > 0) {
				Screen.showCursor = true;
			} else  {
				Screen.showCursor = false;
			}

			//delay before accepting input, to prevent collisions with cutscene prompts
			if(elapsedTime > 0.5f) {
				if(!Input.GetKeyDown(KeyCode.Space)) {
					input1IsDown = Input.GetButtonDown("Select");
				}
				input2IsDown = Input.GetButtonDown("Cancel");
			}
			
			checkKeyControlFocus();
		} else {
			elapsedTime = 0;
		}
		

	}

	protected void enableGuiControl() {
		if(!guiControlEnabled) {
			guiControlEnabled = true;
			cursorShowRequests++;
		}
	}

	protected void disableGuiControl() {
		if(guiControlEnabled) {
			guiControlEnabled = false;
			cursorShowRequests--;
		}
	}

	public virtual void OnGUI() {
		GUI.skin = guiSkin;
		scaleGUI(guiSkin);

		if(guiControlEnabled) {
			GUI.FocusControl(currentButtonSelection.ToString());
		}
	}

	private void scaleGUI(GUISkin guiSkin) {
		//fonts
		guiSkin.label.fontSize = scalePx (16);
		guiSkin.customStyles[1].fontSize = scalePx (16);
		guiSkin.customStyles[2].fontSize = scalePx (14);
		guiSkin.customStyles[3].fontSize = scalePx (12);
		guiSkin.button.fontSize = scalePx (16);
		guiSkin.customStyles[4].fontSize = scalePx(31);
		guiSkin.customStyles[5].fontSize = scalePx(16);
		guiSkin.customStyles[6].fontSize = scalePx(16);
		
		//padding for label styles
		guiSkin.label.padding.top = scalePx (5);
		guiSkin.customStyles[1].padding.top = scalePx (5);
		guiSkin.customStyles[0].padding.top = scalePx (5);
		guiSkin.customStyles[0].padding.bottom = scalePx (10);
		guiSkin.customStyles[0].padding.left = scalePx (10);
		guiSkin.customStyles[0].padding.right = scalePx (10);
		
		//padding for buttons
		guiSkin.button.margin.left = scalePx (20);
		guiSkin.button.margin.top = scalePx (4);
		guiSkin.button.margin.bottom = scalePx (4);
		guiSkin.button.padding.left = scalePx (10);
		guiSkin.button.padding.right = scalePx (10);
		guiSkin.button.padding.top = scalePx (3);
		guiSkin.button.padding.bottom = scalePx (3);
		
		// custom buttons
		guiSkin.customStyles[5].fixedWidth = scalePx (200);
		guiSkin.customStyles[5].margin.bottom = scalePx (10);
		guiSkin.customStyles[5].padding.left = scalePx (10);
		guiSkin.customStyles[5].padding.right = scalePx (10);
		guiSkin.customStyles[5].padding.top = scalePx (10);
		guiSkin.customStyles[5].padding.bottom = scalePx (10);

		guiSkin.customStyles[6].margin.right = scalePx (10);
		guiSkin.customStyles[6].margin.left = scalePx (10);
		guiSkin.customStyles[6].margin.top = scalePx (5);
		guiSkin.customStyles[6].margin.bottom = scalePx (5);
		guiSkin.customStyles[6].padding.left = scalePx (10);
		guiSkin.customStyles[6].padding.right = scalePx (10);
		guiSkin.customStyles[6].padding.top = scalePx (10);
		guiSkin.customStyles[6].padding.bottom = scalePx (10);
		guiSkin.customStyles[6].fixedWidth = (AspectUtility.screenWidth - scalePx(50)) / 3;
	}

	protected int scalePx(int targetSize) {
		return (int)((targetSize * AspectUtility.screenWidth) / targetScreenWidth);
	}

	protected virtual void checkKeyControlFocus() {
		float v = Input.GetAxisRaw("Vertical");
		
		if(!dirKeyDown) { 
			if(v != 0) {
				if(v < 0) {
					currentButtonSelection++;
				} else {
					currentButtonSelection--;
				}
				
				if(currentButtonSelection < numberOfButtonsVisible && currentButtonSelection >= 0) {
					//correct for altered time scale (this allows the sound to play even when paused)
					float tempTimeScale = Time.timeScale;
					Time.timeScale = 1;
					AudioSource.PlayClipAtPoint(cursorMoveSound, Camera.main.transform.position);
					Time.timeScale = tempTimeScale;
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
		

	}
}
