using UnityEngine;
using System.Collections;

public class WordWrapText : MonoBehaviour {

	public int wrapAtWidth = 466;
	public int targetResolutionWidth = 640;

	private GUIText textObject;

	private int numberOfLines = 0;
	private string previousText;

	// Use this for initialization
	void Start () {

		textObject = GetComponent<GUIText>();
		previousText = textObject.text;

		// scale the wrap width to the proper size for the screen
		wrapAtWidth = (wrapAtWidth * Screen.width) / targetResolutionWidth;

	}

	void Update () {

		// if the text has been changed, re-wrap it
		if(previousText != textObject.text) {
			wrapText();

			numberOfLines = 0;
			previousText = textObject.text;
		}
	}

	/// <summary>
	/// Wraps the text; based on the example here:
	/// http://answers.unity3d.com/questions/428389/word-wrapping-guitext.html
	/// </summary>
	void wrapText () { 

		string[] words = textObject.text.Split(' '); //Split the string into seperate words 
		string result = ""; 

		for(int i = 0; i < words.Length; i++) {

			string word = words[i].Trim();

			if (i == 0) {
				result = words[0]; 
				textObject.text = result;
			} else {
				result += " " + word;
				textObject.text = result;
			}

			Rect size = textObject.GetScreenRect(); 

			if (size.width > wrapAtWidth) { 

				result = result.Substring(0, result.Length - (word.Length)); 
				result += "\n" + word; 
				numberOfLines += 1;
				textObject.text = result;
			} 
		} 
	}
}
