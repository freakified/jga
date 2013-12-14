using UnityEngine;
using System.Collections;

/// <summary>
/// Allows scripts to freeze or unfreeze the player.
/// </summary>
public class PlayerFreeze : MonoBehaviour {

	public bool FrozenAtStart = false;

	private PlayerControl Ctl;
	private Animator Anim;

	// Use this for initialization
	void Start () {
		Ctl = GetComponent<PlayerControl>();
		Anim = GetComponent<Animator>();

		if(FrozenAtStart) {
			Freeze();
		}
	}

	/// <summary>
	/// Stops the player from moving, and disables player control.
	/// </summary>
	public void Freeze() {
		Ctl.enabled = false;
		rigidbody2D.velocity = Vector2.zero;
		Anim.SetFloat("Speed", 0);
		
	}

	/// <summary>
	/// Undos the freeze.
	/// </summary>
	public void UnFreeze() {
		Ctl.enabled = true;
		
	}

}
