using UnityEngine;
using System.Collections;

public class Shoesplosion : MonoBehaviour {

	public Rigidbody2D Shoe1, Shoe2;

	private int animationStep = 0;

	// Use this for initialization
	void Start () {
		rigidbody2D.gravityScale = 0;
		Shoe1.gravityScale = 0;
		Shoe2.gravityScale = 0;

		rigidbody2D.AddForce(50 * Vector2.up);
	}
	
	void FixedUpdate () {
		switch(animationStep) {
		case 0: // rise up

			if(rigidbody2D.transform.position.y > 0) {
				animationStep = 1;
			}

			break;
		case 1: //hover and start sparking

			if(rigidbody2D.velocity.y > 0) {
				rigidbody2D.AddForce(-1 * Vector2.up);
			} else {
				rigidbody2D.AddForce(1 * Vector2.up);
			}
		}
	}
}
