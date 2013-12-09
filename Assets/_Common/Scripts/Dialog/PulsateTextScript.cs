using UnityEngine;
using System.Collections;

/// <summary>
/// Makes a GUIText object pulsate.
/// </summary>
public class PulsateTextScript : MonoBehaviour {

	/// <summary>
	/// The fade speed in seconds
	/// </summary>
	public float fadeSpeed = 1.5f;

	private GUIText theText;
	private Color currentColor;

	private bool isFadingIn = true;

	// Use this for initialization
	void Start () {
		theText = GetComponent<GUIText>();

		currentColor = theText.color;
		currentColor.a = 0.0f;
		theText.color = currentColor;
		
	}
	
	// Update is called once per frame
	void Update () {
		if(theText.enabled) {

			if(isFadingIn) {
				currentColor.a += Time.deltaTime / fadeSpeed;
			} else {
				currentColor.a -= Time.deltaTime / fadeSpeed;
			}

			//go slightly out of bounds for a more pleasant fade effect
			if (currentColor.a <= -0.2f && !isFadingIn) {
				isFadingIn = true;
			}

			if (currentColor.a >= 1.2f && isFadingIn) {
				isFadingIn = false;
			}
		} else {
			currentColor.a = 0.0f;
		}

		theText.color = currentColor;

	}
}
