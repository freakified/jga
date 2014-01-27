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


	public bool yPositionAsOffset = false;
	private Transform shadowTransform;
	private Vector3 shadowPosition;
	private float shadowOffsetY;

	// Use this for initialization
	void Start () {
		//create the shadow
		shadowTransform = Instantiate(shadowPrefab) as Transform;
		shadowTransform.parent = transform;

		GameObject ground = GameObject.Find("Ground");

		if(ground != null) {
			shadowPositionY = ground.transform.position.y;
		}


	}
	
	// Update is called once per frame
	void Update () {
		shadowPosition = transform.position;
		shadowPosition.x = transform.position.x + shadowOffsetX;
		shadowPosition.z = transform.position.z;

		if(!yPositionAsOffset) {
			shadowPosition.y = shadowPositionY;
		} else {
			shadowPosition.y = transform.position.y + shadowOffsetY;
		}

		shadowTransform.position = shadowPosition;
	}

	/// <summary>
	/// Causes the shadow will move with the sprite along the Y axis.
	/// </summary>
	public void LockShadowY() {
		shadowOffsetY = shadowPositionY - transform.position.y;

		yPositionAsOffset = true;
	}

	public void UnlockShadowY() {
		yPositionAsOffset = false;
	}
}
