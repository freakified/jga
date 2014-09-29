using UnityEngine;
using System.Collections;

/// <summary>
/// Makes a GUIText object pulsate.
/// </summary>
public class TextFadeOutScript : MonoBehaviour {

	/// <summary>
	/// The fade speed in seconds
	/// </summary>
	public float fadeSpeed = 1.5f;

	public float FadeAfter = 0.5f;

	private GUIText theText;
	private Color currentColor;

	private float elapsedTime;
	
	// Use this for initialization
	void Start () {
		theText = GetComponent<GUIText>();

		currentColor = theText.color;
		currentColor.a = 1.0f;
		theText.color = currentColor;
	}

	public void ShowText() {
		currentColor.a = 1.0f;
		elapsedTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(theText.enabled) {
			elapsedTime += Time.deltaTime;

			if(elapsedTime > FadeAfter) {
				currentColor.a -= Time.deltaTime / fadeSpeed;
			}
		} else {
			currentColor.a = 0.0f;
		}


		theText.color = currentColor;

	}
}
