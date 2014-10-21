using UnityEngine;
using System.Collections;

/// <summary>
/// Attach this behavior to a GUIText object to cause it to automatically
/// resize based on the screen size at Start.  
/// 
/// Note: currently NOT compatible with screen resizing while the program is running.
/// </summary>
public class TextScalerScript : MonoBehaviour {

	public int targetFontSize = 16;
	public int targetResolutionWidth = 640;

	private GUIText text;

	void Start () {
		text = GetComponent<GUIText>();

		int calcFontSize = (targetFontSize * AspectUtility.screenWidth) / targetResolutionWidth;

		text.fontSize = calcFontSize;
	}
}
