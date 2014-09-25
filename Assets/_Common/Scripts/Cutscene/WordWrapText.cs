using UnityEngine;
using System.Collections;

public class WordWrapText : MonoBehaviour {

	public int wrapAtWidth = 466;
	public int targetResolutionWidth = 640;
	
	private GUIText textObject;


	// Use this for initialization
	void Start () {
		textObject = GetComponent<GUIText>();

		// scale the wrap width to the proper size for the screen
		wrapAtWidth = (wrapAtWidth * Screen.width) / targetResolutionWidth;

	}

	/// <summary>
	/// Sets the text, and wraps it; based on the example here:
	/// http://answers.unity3d.com/questions/428389/word-wrapping-guitext.html
	/// </summary>
	public void SetText (string newText) { 

		if(newText.Length > 0) {
			string[] words = newText.Split(' '); //Split the string into seperate words 
			string result = ""; 

			Rect textArea = new Rect();

			for(int i = 0; i < words.Length; i++) {

				// set the gui text to the current string including new word
				textObject.text = (result + words[i] + " ");
				
				// measure it
				textArea = textObject.GetScreenRect();
				
				// if it didn't fit, put word onto next line, otherwise keep it
				if(textArea.width > wrapAtWidth) {
					result += ("\n" + words[i] + " ");
				} else {
					result = textObject.text;
				}
			} 

			textObject.text = result;
		} else {
			textObject.text = "";
		}


	}
}
