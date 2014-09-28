using UnityEngine;

/// <summary>
/// To be applied to a Sprite object
/// </summary>
public class SpriteFlicker : MonoBehaviour {

	/// <summary>
	/// The delay between each flicker, in seconds
	/// </summary>
	public float flickerDelay = 0.5f;

	/// <summary>
	/// The probability that a flicker occurs (between 0 and 1)
	/// </summary>
	public float flickerProbability = 0.2f;

	public float normalAlpha = 1.0f;
	public float flickerAlpha = 0.5f;


	private float timeSinceLastFlicker = 0;
	private bool isCurrentlyFlickering = false;
	private Color normalColor, flickerColor;

	// Initialization
	void Start () {
		normalColor = new Color (1f, 1f, 1f, normalAlpha);
		flickerColor = new Color (1f, 1f, 1f, flickerAlpha);
	}
	
	// Update is called once per frame
	void Update () {
		normalColor = new Color (1f, 1f, 1f, normalAlpha);
		flickerColor = new Color (1f, 1f, 1f, flickerAlpha);

		timeSinceLastFlicker += Time.deltaTime;

		if (timeSinceLastFlicker > flickerDelay) {

			// flicker for exactly one frame
			if(!isCurrentlyFlickering) {
				//should we actually flicker?
				if(Random.value < flickerProbability) {
					((SpriteRenderer)renderer).color = flickerColor;
					isCurrentlyFlickering = true;
				}

			} else {
				((SpriteRenderer)renderer).color = normalColor;
				timeSinceLastFlicker = 0;
				isCurrentlyFlickering = false;
			}

		}
	}
}