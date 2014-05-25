using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class OrphanCombatant : EnemyCombatant {

	// jump parameters
	private float StartDelay = 0;
	private int JumpForce = 100;
	private float Interval = 0.5f;
	private float jumpTimer = 0;
	
	// Use this for initialization
	public override void Start () {

		base.Start ();

		Physics2D.IgnoreLayerCollision(10, 0); // disable collisions with other battlers

		// you can't kill an orphan! who would buy your knives then?
		immuneToDamage = true;

		//set up basic stats
		MaxHitPoints = 100;
		HitPoints = 100;

	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();

		// he is basically completely inert
		
	}

	// perform jump behavior
	void FixedUpdate() {
		if(!isSleeping) {
			jumpTimer += Time.fixedDeltaTime;
			
			if(jumpTimer > StartDelay) {
				if(jumpTimer > Interval) {
					rigidbody2D.AddForce(Vector2.up * JumpForce);
					
					jumpTimer = 0;
				}
			}
		}
	}

	public override void AutoAttack (List<BattleCombatant> targetList) {
		// orphan doesn't attack (note: perhaps he should heal FF?)

	}
	
}
