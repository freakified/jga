using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	/// <summary>
	/// The speed.
	/// </summary>
	public float Speed = 5f;

	private bool isKeyDown = false;

	protected Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		float inputX = Input.GetAxis("Horizontal");
	    
		if (Input.GetKeyDown (KeyCode.Space)) {
			isKeyDown = true;
		}

		if (Input.GetKeyUp (KeyCode.Space)) {
			isKeyDown = false;
		}

		animator.SetBool ("IsStabbing", isKeyDown);
	}
}
