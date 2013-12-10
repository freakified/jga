using UnityEngine;
using System.Collections;

public class NpcMovement : MonoBehaviour {

	public bool facingRight = true;
	
	public float moveForce = 365f;
	public float maxSpeed = 1f;	

	private Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();


	}
	
	void FixedUpdate () {


	}

	void Flip () {
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
		
	}
}
