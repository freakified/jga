using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S602Puppeteer : CutscenePuppeteer {

	public AudioClip RainSound;

	private GameObject ChefTony, James;

	private List<Rigidbody2D> junk;

	private bool junkHasBeenDumped = false;
	
	// Use this for initialization
	void Start () {
		// get all the objects we'll need for the cutscene 
		ChefTony = GameObject.Find ("Chef Tony");
		James = GameObject.Find ("James");

		junk = new List<Rigidbody2D>(GameObject.Find ("RandomJunk").GetComponentsInChildren<Rigidbody2D>());

		playSound(RainSound, true);
		startTimer();

	}
	
	// Update is called once per frame
	public void FixedUpdate () {
		if(CurrentScene == 0) {
			if(James.transform.position.x > -10) {
				James.GetComponent<ConstantVelocity>().velocity = Vector2.zero;
				James.GetComponent<ConstantVelocity>().enabled = false;
				James.GetComponent<JamesGasBehavior>().DisableFlightMode();
				James.transform.position = new Vector3(66.31f, -0.76f, -4.065f);
				James.transform.localScale = new Vector3(-1, 1, 1);
				nextScene();
			}

		} else if(CurrentScene == 1) {
			if(ChefTony.transform.position.x > -10) {
				ChefTony.GetComponent<PlayerFreeze>().Freeze();
				nextScene();
			}
		} else if(CurrentScene == 3) {
			if(ChefTony.transform.position.x > 0) {
				ChefTony.GetComponent<PlayerFreeze>().Freeze();
				nextScene();
			}
		} else if(CurrentScene == 5) {
			if(ChefTony.transform.position.x > 10) {
				ChefTony.GetComponent<PlayerFreeze>().Freeze();
				nextScene();
			}
		} else if(CurrentScene == 7) {
			if(ChefTony.transform.position.x > 20) {
				ChefTony.GetComponent<PlayerFreeze>().Freeze();
				nextScene();
			}
		} else if(CurrentScene == 12) {
			if(ChefTony.transform.position.x > 30) {
				ChefTony.GetComponent<PlayerFreeze>().Freeze();
				nextScene();
			}
		} else if(CurrentScene == 14) {
			if(ChefTony.transform.position.x > 40) {
				ChefTony.GetComponent<PlayerFreeze>().Freeze();
				nextScene();
			}
		} else if(CurrentScene == 16) {
			if(ChefTony.transform.position.x > 50) {
				ChefTony.GetComponent<PlayerFreeze>().Freeze();
				nextScene();
			}
		} else if(CurrentScene == 18) {
			if(ChefTony.transform.position.x > 65.5f) {
				ChefTony.GetComponent<PlayerFreeze>().Freeze();
				nextScene();
			}
		} else if(CurrentScene == 21) {
			James.rigidbody2D.AddForce(new Vector2(2, 300) * Time.fixedDeltaTime);
			
			if(James.transform.position.y > 0.2f) {
				nextScene();
			}
		} else if(CurrentScene == 22) {
			James.rigidbody2D.AddForce(Vector2.right * Time.fixedDeltaTime * 400);
			
			if(James.transform.position.x > 72) {
				nextScene();
				James.GetComponent<JamesGasBehavior>().DisableFlightMode();
				James.rigidbody2D.Sleep();
			}
     	}

		if(!junkHasBeenDumped && CurrentScene >=16 && ChefTony.transform.position.x < -23.12f) {
			Physics2D.IgnoreLayerCollision(11, 12);
			dumpJunkEasterEgg();
		}
	}

	private void dumpJunkEasterEgg() {
		junk.ForEach(thing => thing.isKinematic = false);
	}

	public override void HandleSceneChange() {
		if(CurrentScene == 1) {
			GameObject textbox = GameObject.Find ("TextBox(Clone)");
			textbox.transform.localRotation = Quaternion.identity;
		} else if(CurrentScene == 3 || 
		          CurrentScene == 5 || 
		          CurrentScene == 7 || 
		          CurrentScene == 12 ||
		          CurrentScene == 14 || 
		          CurrentScene == 16 || 
		          CurrentScene == 18) {
			ChefTony.GetComponent<PlayerFreeze>().UnFreeze();
		} else if(CurrentScene == 21) {
			James.transform.localScale = new Vector3(1, 1, 1);
			James.GetComponent<JamesGasBehavior>().EnableFlightMode();
			James.rigidbody2D.gravityScale = 0.2f;
		} else if(CurrentScene == 23) {
			ChefTony.GetComponent<PlayerFreeze>().UnFreeze();
		}
	}

}
