using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class OrphanCombatant : EnemyCombatant {

	// Use this for initialization
	public override void Start () {
		base.Start ();

		//set up basic stats
		MaxHitPoints = 100;
		HitPoints = 100;

	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();

		// he is basically completely inert
		
	}

	public override void AutoAttack (List<BattleCombatant> targetList) {
		// orphan doesn't attack (note: perhaps he should heal FF?)

	}
	
}
