using UnityEngine;
using System.Collections;

/// <summary>
/// Creates a simple shadow under a sprite.
/// The shadow will always appear beneath the sprite at the specified Y position.
/// </summary>
public class SpriteShadow : MonoBehaviour {

	/// <summary>
	/// The sprite to use for the shadow.
	/// </summary>
	public Transform shadowPrefab;

	/// <summary>
	/// The offsets for the shadow.
	/// </summary>
	public float shadowOffsetX;

	/// <summary>
	/// The permanent Y position of the shadow.
	/// </summary>
	public float shadowPositionY;

	private Transform shadowTransform;
	private Vector2 shadowPosition;

	// Use this for initialization
	void Start () {
		//create the shadow
		shadowTransform = Instantiate(shadowPrefab) as Transform;

		
	}
	
	// Update is called once per frame
	void Update () {
		shadowPosition = transform.position;
		shadowPosition.x = transform.position.x + shadowOffsetX;
		shadowPosition.y = shadowPositionY;

		shadowTransform.position = shadowPosition;
	}
}
