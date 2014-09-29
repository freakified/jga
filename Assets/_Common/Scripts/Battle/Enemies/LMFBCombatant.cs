using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LMFBCombatant : EnemyCombatant {

	// Use this for initialization
	public override void Start () {

		base.Start ();

		//set up basic stats
		MaxHitPoints = 100;
		HitPoints = 100;

	}

	public override void PutToSleep (int numberOfTurns) {
		// evil basketball entities are not sold by your sales pitches
		//GameObject.Instantiate(
	}

	public override void Damage (int amount) {
		// you can't damage an evil basketball entity, that's just crazy you're crazy
	}

	public override string getName() {
		return "Sphaera Imperandi";
	}

	public override void AutoAttack (List<BattleCombatant> targetList) {
		// attack? why would we do that?
	}

}
