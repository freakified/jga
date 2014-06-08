using UnityEngine;
using System.Collections;

public class Shoesplosion : MonoBehaviour {

	public Rigidbody2D Shoe1, Shoe2;

	public float InitRiftIntensity = 0.05f;
	public float FinalRiftIntensity = 10.0f;
	public float RiftExpansionDuration = 5.0f;
	public float RiftExpansionDelay = 2.0f;

	public ParticleSystem RiftPrefab;
	private ParticleSystem riftParticles;

	private int animationStep = 0;
	private float elapsedTime = 0.0f;

	// Use this for initialization
	void Start () {
		rigidbody2D.gravityScale = 0;
		Shoe1.gravityScale = 0;
		Shoe2.gravityScale = 0;

		riftParticles = Instantiate(RiftPrefab) as ParticleSystem;
		riftParticles.transform.parent = transform;
		riftParticles.transform.localPosition = new Vector3(0.0f, 0.0f, 1.0f);

		rigidbody2D.AddForce(50 * Vector2.up);
	}
	
	void FixedUpdate () {
		switch(animationStep) {
		case 0: // rise up

			if(rigidbody2D.transform.position.y > 0) {
				animationStep = 1;
				riftParticles.Play();
				elapsedTime = 0.0f;
			}

			break;
		case 1: //hover and start sparking
			elapsedTime += Time.fixedDeltaTime;

			//hold the shoes in midair
			if(rigidbody2D.velocity.y > 0) {
				rigidbody2D.AddForce(-1 * Vector2.up);
			} else {
				rigidbody2D.AddForce(1 * Vector2.up);
			}

			if(elapsedTime > RiftExpansionDelay) {
				float animTime = elapsedTime - RiftExpansionDelay;

				if(animTime < RiftExpansionDuration) {
					riftParticles.startLifetime = 
						Mathf.Lerp(InitRiftIntensity, FinalRiftIntensity, animTime / RiftExpansionDuration);
				}
			}




			break;
		}
	}
}
