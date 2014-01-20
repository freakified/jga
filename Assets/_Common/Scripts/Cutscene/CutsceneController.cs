using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class CutsceneController : MonoBehaviour {


	/// <summary>
	/// The text to be parsed for the cutscene dialog.
	/// </summary>
	public TextAsset screenplay;

	public GUIText dialogSpeakerText;
	public GUIText dialogText;
	public Transform textBox;
	public GUIText dialogNextText;

	[HideInInspector]
	public delegate void ChangedEventHandler(int newCutscenePosition);
	[HideInInspector]
	public static event ChangedEventHandler OnCutsceneChange;


	private Vector3 speakerTextPos = new Vector3(0.04f, 0.95f, 8f);
	private Vector3 dialogTextPos = new Vector3(0.04f, 0.89f, 8f);
	private Vector3 textBoxPos = new Vector3(-0.71f, 1.31f, 8f);
	private Vector3 dialogNextTextPos = new Vector3(0.64f, 0.82f, 8f);

	private WordWrapText dialogTextWrapper;

	/// <summary>
	/// Should the cutscene begin automatically when the scene starts?
	/// </summary>
	public bool autoplayAtStart = true;
	public float delayBeforeAutoplay = 2f;
	
	private List<CutsceneElement> cutsceneElements;

	/// <summary>
	/// What position in the cutscene are we at?
	/// (0 means the cutscene hasn't started yet)
	/// </summary>
	private int cutscenePosition = 0;

	/// <summary>
	/// The text speed, in characters per second
	/// </summary>
	public float textSpeed = 50.0f;
	private float currentChar = 0.0f;

	void Start () {
		createDialogObjects();

		setDialogVisibility(false);

		populateCutsceneElements();

		if(autoplayAtStart)
			Invoke("playNext", delayBeforeAutoplay);
	}
	
	// Update is called once per frame
	void Update () {

		//if the [spacebar] indicator is displayed, and the person presses Fire1, 
		// then go to the next dialog
		if(Input.GetButtonDown("Select") && dialogNextText.enabled) {
			dialogNextText.enabled = false;
			currentChar = 0;
			playNext();
		}

		//if we're currently showing dialog, then start scrolling it
		if(dialogText.enabled) {

			// if there's still text left to show
			if(currentChar < cutsceneElements[cutscenePosition - 1].dialogText.Length) {

				//ensure that we don't accidentally blow past the end of the string
				currentChar = Mathf.Min(
					currentChar + textSpeed * Time.deltaTime,
					cutsceneElements[cutscenePosition - 1].dialogText.Length);

				dialogTextWrapper.SetText(
					cutsceneElements[cutscenePosition - 1].dialogText.Substring(0, (int)currentChar)
				);
			} else {
				if(cutsceneElements[cutscenePosition - 1].allowPlayerAdvance)
					dialogNextText.enabled = true;
			}
		}

	}

	/// <summary>
	/// Creates the dialog objects.
	/// </summary>
	public void createDialogObjects() {

		dialogSpeakerText = Instantiate(dialogSpeakerText) as GUIText;
		dialogSpeakerText.transform.position = speakerTextPos;
		
		dialogText = Instantiate(dialogText) as GUIText;
		dialogText.transform.position = dialogTextPos;
		dialogTextWrapper = dialogText.GetComponent<WordWrapText>();

		textBox = Instantiate(textBox) as Transform;
		textBox.transform.parent = GameObject.Find("Main Camera").transform;
		textBox.transform.localPosition = textBoxPos;


		dialogNextText = Instantiate(dialogNextText) as GUIText;
		dialogNextText.transform.position = dialogNextTextPos;

	}

	/// <summary>
	/// Plays the next cutscene element.
	/// 
	/// </summary>
	public void playNext() {
		// increment the cutscene counter
		cutscenePosition++;

		if(cutscenePosition <= cutsceneElements.Count) {

			CutsceneElement currentCutsceneElement = cutsceneElements[cutscenePosition - 1];

			if(currentCutsceneElement.hasDialog) {
				setDialogVisibility(true);

				dialogSpeakerText.text = currentCutsceneElement.speakerName;
				dialogText.text = currentCutsceneElement.dialogText;

			} else {
				setDialogVisibility(false);
			}
		} else {
			setDialogVisibility(false);
		}

		//notify registered listeners of the new position
		if(OnCutsceneChange != null)
			OnCutsceneChange(cutscenePosition);

	}

	/// <summary>
	/// Populates the scene list by reading the text resource and parsing it.
	/// </summary>
	private void populateCutsceneElements() {
		cutsceneElements = new List<CutsceneElement>();
		
		// read the Script into the scene list
		StringReader reader = new StringReader (screenplay.text);
		
		string line = reader.ReadLine (); 
		
		while (line != null) {
			cutsceneElements.Add(readSceneElement(line));
			
			line = reader.ReadLine();
		}
	}

	/// <summary>
	/// Parses a scene element from a given string, and returns the result
	/// </summary>
	/// <returns>The scene element.</returns>
	/// <param name="line">The line to parse</param>
	private CutsceneElement readSceneElement(string line) {

		CutsceneElement newElement = new CutsceneElement();

		// if the line is not denoted as blank...
		if (line != "***") {
			string[] splitLine = line.Split (new char[] {':'}, 2);

			// should the player be allowed to advance manually?
			if(splitLine[0][0] == '*') {
				newElement.allowPlayerAdvance = false;
				newElement.speakerName = splitLine [0].Substring(1).Trim ();
			} else {
				newElement.speakerName = splitLine [0].Trim ();
			}

			newElement.hasDialog = true;

			newElement.dialogText = splitLine [1].Trim ();
		}

		return newElement;

	}

	/// <summary>
	/// Shows or hides the dialog elements.
	/// </summary>
	/// <param name="state">If set to <c>true</c>, shows the dialog elements.</param>
	private void setDialogVisibility(bool state) {

		dialogSpeakerText.enabled = state;
		dialogText.enabled = state;
		textBox.renderer.enabled = state;

		// when the visibility is set to "true", we don't want to show the [spacebar] prompt
		// right away
		if(!state) {
			dialogNextText.enabled = state;
			dialogText.text = "";
		}

	}

	private class CutsceneElement {
		public bool hasDialog = false;
		public bool allowPlayerAdvance = true;
		public string speakerName;
		public string dialogText;

		public override string ToString () {
			return  "(" + this.hasDialog + ")" + this.speakerName + "::" + this.dialogText + "\n";
		}
	}

}
