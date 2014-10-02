using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BBallShieldCombatant : EnemyCombatant {

	private Rigidbody2D[] subBBalls;

	// Use this for initialization
	public override void Start () {

		base.Start ();

		subBBalls = GetComponentsInChildren<Rigidbody2D>();

		immuneToDamage = false;

		//set up basic stats
		MaxHitPoints = 10000;
		HitPoints = 10000;

	}


	public override string getName ()
	{
		return "Basketball Shield";
	}

	private bool dropped = false;

	// perform rotate behavior
	void FixedUpdate() {
		if(!isSleeping && HitPoints > 0) {
			if(dropped) {
				//undrop

				//first, enable negative gravity
				if(!subBBalls[0].isKinematic) {
					foreach(Rigidbody2D ball in subBBalls) {
						ball.gravityScale = -1;
						ball.GetComponent<BillboardScript>().enabled = true;
					}
				}
				// now, wait for them to float back to where they belong
				if(subBBalls[0].transform.localPosition.y > 0f) {

					foreach(Rigidbody2D ball in subBBalls) {
						ball.isKinematic = true;
						Vector3 temp = ball.transform.localPosition;
						temp.y = 0;
						ball.transform.localPosition = temp;
					}
					
					dropped = false;
				}

			}

			transform.Rotate(new Vector3(0, 100, 0) * Time.deltaTime);
		} else {
			if(!dropped) {
				foreach(Rigidbody2D ball in subBBalls) {
					ball.gravityScale = 1;
					ball.isKinematic = false;
					ball.GetComponent<BillboardScript>().enabled = false;
				}
				dropped = true;
			}
		}
	}

	public override void AutoAttack (List<BattleCombatant> targetList) {
		// shield doesn't attack 

	}
	
}
