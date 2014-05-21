using UnityEngine;
using System.Collections;

public class OrphanBehavior : MonoBehaviour {

	public float StartDelay = 0;
	public int JumpForce = 100;
	public float Interval = 0.5f;

	private float time = 0;

	void Start () {
		Physics2D.IgnoreLayerCollision(10, 0); // disable collisions with other battlers
	}

	void FixedUpdate() {
		time += Time.fixedDeltaTime;

		if(time > StartDelay) {
			if(time > Interval) {
				rigidbody2D.AddForce(Vector2.up * JumpForce);

				time = 0;
			}
		}
	}
	
}
