using UnityEngine;
using System.Collections;

public class FBHoverBehavior : MonoBehaviour {

	private float forceFactor = 0.1f;
	private float elapsedTime;

	// Use this for initialization
	void Start () {
		elapsedTime = 0;

		//rigidbody2D.AddTorque(50.0f);

	}
	
	void FixedUpdate () {
		elapsedTime += Time.fixedDeltaTime;

		if (elapsedTime < 2.0f) {
				rigidbody2D.AddForce (Vector2.up * forceFactor);
		} else {
			elapsedTime = 0.0f;
			forceFactor *= -1;
		}
	}
}
