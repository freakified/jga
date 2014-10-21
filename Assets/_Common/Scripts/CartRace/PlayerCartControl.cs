using UnityEngine;
using System.Collections;

/// <summary>
/// Provides simple left/right arrow key control of the player.
/// Based on the PlayerControl script from the 2D Unity Example Project
/// </summary>
public class PlayerCartControl : MonoBehaviour {

	
	public float forwardSpeed = 5f;				// The fastest the player can travel in the x axis.
	private float turnSpeed = 6f;

	public AudioClip smashSound, bigSmash;

	public int maxHP = 4;
	private int currentHP;
	
	private float minZ = -3.6f;
	private float maxZ = -0.2f;

	private bool levelIsEnding = false;
	private bool isFrozen = false;

	// how much smoke will the cart emit with 1 damage vs max damage
	public int smokeEmissionMin = 10;
	public int smokeEmissionMax = 100;

	public ParticleSystem explosionPrefab;
	
	private ParticleSystem explosion;

	private int emissionIncrement;

	void Start() {
		emissionIncrement = (smokeEmissionMax - smokeEmissionMin) / maxHP - 1;

		currentHP = maxHP;
	}
	
	
	void Update () {
		if(!isFrozen) {
			// Cache the vertical input.
			float v = Input.GetAxis("Vertical");

			// check for touch input
			if(Input.touchCount > 0) {
				Touch t = Input.GetTouch(0);

				if(t.position.x < AspectUtility.screenWidth / 2) {
					if(t.position.y > Camera.main.WorldToScreenPoint(transform.position).y + 2) {
						v = 1;
					} else if(t.position.y < Camera.main.WorldToScreenPoint(transform.position).y - 2) {
						v = -1;
					}
				}
			}

			// roll chef tony on down the hill
			transform.Translate(new Vector3(forwardSpeed * Time.fixedDeltaTime, 0, 0));

			// now move him if there is input
			Vector3 newPos = transform.position;
			float newZ = Mathf.Clamp(transform.position.z + v * turnSpeed * Time.fixedDeltaTime, minZ, maxZ); 
			newPos.z = newZ;
			transform.position = newPos;

			// scroll the camera from here, since the cart race uses different scrolling than anything else
			Camera.main.transform.position = new Vector3(transform.position.x + 2.61f, 
			                                    transform.position.y + 2.33f, 
	                                            Camera.main.transform.position.z);

			// chef if we're at the end of the level
			if(!levelIsEnding && transform.position.x > 208.4) {
				levelIsEnding = true;

				AudioSource.PlayClipAtPoint(bigSmash, Camera.main.transform.position);

				Camera.main.GetComponent<CameraFade>().FadeAndNext(Color.black, 2, "3-01 Limbo", true);
			} 
		
		}

	}

	public void FreezeCart() {
		isFrozen = true;
	}

	public void UnFreezeCart() {
		isFrozen = false;
	}

	void OnTriggerEnter (Collider obstacle) {
		AudioSource.PlayClipAtPoint(smashSound, Camera.main.transform.position);

		obstacle.gameObject.GetComponent<CartObstacle>().CrashAndBurn();

		damage ();
	}

	private void damage() {
		currentHP--;

		if(currentHP > 0) {
			if(!particleSystem.isPlaying) {
				particleSystem.emissionRate = smokeEmissionMin;
				particleSystem.Play();
			} else {
				particleSystem.emissionRate += emissionIncrement;
			}
		} else {
			//create and play the explosion animation
			if(!gameOverInProgress) {
				explosion = Instantiate(explosionPrefab) as ParticleSystem;
				explosion.transform.position = transform.position;
				explosion.transform.parent = transform;
				explosion.Play();
				//Destroy(explosion, explosion.duration);

				//because chef tony shall burn
				particleSystem.emissionRate *= 2; 

				forwardSpeed = 0;
				turnSpeed = 0;
			}

			//trigger the end
			gameOver();

		}
	}

	private bool gameOverInProgress = false;
	
	private void gameOver() {
		if(!gameOverInProgress) {
			Camera.main.GetComponent<CameraFade>().FadeAndNext(Color.black, 5, "x-01 Game Over", true);
			gameOverInProgress = true;
		}
	}

}
